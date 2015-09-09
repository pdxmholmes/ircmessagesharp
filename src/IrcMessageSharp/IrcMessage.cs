#region License

// Copyright (c) 2015, Matt Holmes
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the project nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT  LIMITED TO, THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
// THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT  LIMITED TO, PROCUREMENT 
// OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
// TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IrcMessageSharp {
    // C# implementation of Fionn Kelleher's irc-message for JavaScript:
    // https://github.com/expr/irc-message
    //
    // Which is itself an impelemntation of RFC 2812: http://tools.ietf.org/html/rfc2812
    // http://ircv3.atheme.org/specification/message-tags-3.2
    public class IrcMessage {
        private static readonly Regex HostmaskRegex = new Regex ("[!@]", RegexOptions.Compiled);

        public IrcMessage () {
            Command = String.Empty;
            Prefix = String.Empty;
            Params = new List<string> ();
            Tags = new Dictionary<string, string> ();
        }

        public string Command { get; set; }

        public bool IsPrefixHostmask {
            get {
                return !String.IsNullOrWhiteSpace (Prefix)
                       && Prefix.Contains ("@")
                       && Prefix.Contains ("!");
            }
        }

        public bool IsPrefixServer {
            get {
                return !String.IsNullOrWhiteSpace (Prefix)
                       && !IsPrefixHostmask
                       && Prefix.Contains (".");
            }
        }

        public IList<string> Params { get; private set; }
        public string Prefix { get; set; }
        public IDictionary<string, string> Tags { get; private set; }

        public static IrcMessage Parse (string line) {
            if (String.IsNullOrWhiteSpace (line)) {
                throw new FormatException ("Invalid IRC message: message is empty");
            }

            var message = new IrcMessage ();
            int nextspace;
            var position = 0;

            // The first thing we check for is IRCv3.2 message tags.
            // http://ircv3.atheme.org/specification/message-tags-3.2
            if (line[0] == '@') {
                nextspace = line.IndexOf (' ');
                if (nextspace == -1) {
                    throw new FormatException ("Invalid IRC message: malformed message parsing tags");
                }

                var rawTags = line.Substring (1, nextspace - 1).Split (';');
                foreach (var pair in rawTags.Select (tag => tag.Split ('='))) {
                    message.Tags[pair[0]] = pair.Length > 1 ? pair[1] : "true";
                }
                position = nextspace + 1;
            }

            position = SkipSpaces (line, position);

            // Extract the message's prefix if present. Prefixes are prepended
            // with a colon.
            if (line[position] == ':') {
                nextspace = line.IndexOf (' ', position);
                if (nextspace == -1) {
                    throw new FormatException ("Invalid IRC message: malformed message parsing prefix");
                }

                message.Prefix = line.Substring (position + 1, (nextspace - position) - 1);
                position = nextspace + 1;
                position = SkipSpaces (line, position);
            }

            // If there's no more whitespace left, extract everything from the
            // current position to the end of the string as the command.
            nextspace = line.IndexOf (' ', position);
            if (nextspace == -1) {
                if (line.Length > position) {
                    message.Command = line.Substring (position);
                }
                return message;
            }

            // Else, the command is the current position up to the next space. After
            // that, we expect some parameters.
            message.Command = line.Substring (position, nextspace - position);
            position = nextspace + 1;
            position = SkipSpaces (line, position);

            while (position < line.Length) {
                nextspace = line.IndexOf (' ', position);

                // If the character is a colon, we've got a trailing parameter.
                // At this point, there are no extra params, so we push everything
                // from after the colon to the end of the string, to the params array
                // and break out of the loop.
                if (line[position] == ':') {
                    message.Params.Add (line.Substring (position + 1));
                    break;
                }

                // If we still have some whitespace...
                if (nextspace != -1) {
                    // Push whatever's between the current position and the next
                    // space to the params array.
                    message.Params.Add (line.Substring (position, nextspace - position));
                    position = nextspace + 1;
                    // Skip any trailing whitespace and continue looping.
                    position = SkipSpaces (line, position);
                    continue;
                }

                // If we don't have any more whitespace and the param isn't trailing,
                // push everything remaining to the params array.
                if (nextspace != -1) {
                    continue;
                }

                message.Params.Add (line.Substring (position));
                break;
            }

            return message;
        }

        public static bool TryParse (string line, out IrcMessage message) {
            message = null;
            try {
                message = Parse (line);
                return true;
            }
            catch {
                return false;
            }
        }

        public Hostmask GetHostmaskFromPrefix () {
            if (!IsPrefixHostmask) {
                return null;
            }

            var parts = HostmaskRegex.Split (Prefix);
            return new Hostmask {
                Nickname = parts[0],
                Username = parts[1],
                Hostname = parts[2]
            };
        }

        public override string ToString () {
            if (String.IsNullOrWhiteSpace (Command)) {
                return String.Empty;
            }

            var parts = new List<string> ();
            if (Tags.Count > 0) {
                var tags = String.Join (";", Tags.Where (kvp => !String.IsNullOrWhiteSpace (kvp.Key)).Select (kvp =>
                    String.IsNullOrWhiteSpace (kvp.Value) || kvp.Value.Equals ("true", StringComparison.InvariantCultureIgnoreCase)
                        ? kvp.Key.Trim ()
                        : String.Format ("{0}={1}", kvp.Key.Trim (), kvp.Value.Trim ())));
                parts.Add (String.Format ("@{0}", tags));
            }

            if (!String.IsNullOrWhiteSpace (Prefix)) {
                parts.Add (String.Format (":{0}", Prefix.Trim ()));
            }

            parts.Add (Command.Trim ());

            if (Params.Count > 0) {
                var processedParams = Params.Where (p => !String.IsNullOrWhiteSpace (p)).Select (p => p.Trim ()).ToList ();
                // We have at least one parameter that isn't blank or empty
                if (processedParams.Count > 0) {
                    var lastHasSpaces = processedParams.Last ().IndexOf (' ') != -1;
                    parts.AddRange (
                        processedParams.Take (processedParams.Count - (lastHasSpaces ? 1 : 0)).SelectMany (p => p.IndexOf (' ') == -1
                            ? new[] { p }
                            : p.Split (' ').Where (s => !String.IsNullOrWhiteSpace (s)))
                        );

                    if (lastHasSpaces) {
                        parts.Add (String.Format (":{0}", processedParams.Last ()));
                    }
                }
            }

            return String.Join (" ", parts);
        }

        private static int SkipSpaces (string text, int position) {
            while (position < text.Length && text[position] == ' ') {
                position++;
            }

            return position;
        }

        public class Hostmask {
            public string Hostname { get; set; }
            public string Nickname { get; set; }
            public string Username { get; set; }
        }
    }
}
