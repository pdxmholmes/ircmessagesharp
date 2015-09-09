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

using System.IO;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IrcMessageSharp.Spec {
    [Binding]
    public class IrcStreamReaderSteps {
        private IrcMessage _message;
        private IrcStreamReader _reader;
        private MemoryStream _stream;
        private StreamWriter _writer;

        [Given (@"stream has line '(.*)'")]
        public void GivenStreamHasLine (string p0) {
            _writer.WriteLine (p0);
            _writer.Flush ();
            _stream.Position = 0;
        }

        [Given (@"stream has text '(.*)'")]
        public void GivenStreamHasText (string p0) {
            _writer.Write (p0);
            _writer.Flush ();
            _stream.Position = 0;
        }

        [Given (@"string stream")]
        public void GivenStringStream () {
            _stream = new MemoryStream ();
            _writer = new StreamWriter (_stream);
            _reader = new IrcStreamReader (_stream);
        }

        [Then (@"message has command '(.*)'")]
        public void ThenMessageHasCommand (string p0) {
            Assert.AreEqual (p0, _message.Command);
        }

        [Then (@"message has prefix '(.*)'")]
        public void ThenMessageHasPrefix (string p0) {
            Assert.AreEqual (p0, _message.Prefix);
        }

        [Then (@"message is null")]
        public void ThenMessageIsNull () {
            Assert.IsNull (_message);
        }

        [When (@"message is extracted from stream")]
        public void WhenMessageIsExtractedFromStream () {
            _message = _reader.ReadMessage ();
        }

        [When (@"message is extracted from stream asynchronously")]
        public async void WhenMessageIsExtractedFromStreamAsynchronously () {
            _message = await _reader.ReadMessageAsync ();
        }
    }
}
