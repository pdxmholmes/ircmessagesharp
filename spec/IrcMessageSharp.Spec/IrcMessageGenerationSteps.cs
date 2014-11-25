#region License

// Copyright (c) 2011, Matt Holmes
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

using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IrcMessageSharp.Spec {
    [Binding]
    public class IrcMessageGenerationSteps {
        #region Fields

        private IrcMessage _message;
        private string _messageString;

        #endregion

        #region Public Methods

        [Given (@"a message with command '(.*)'")]
        public void GivenAMessageObjectWithCommand (string p0) {
            _message = new IrcMessage { Command = p0 };
        }

        [Given (@"a message with no command")]
        public void GivenAMessageWithNoCommand () {
            _message = new IrcMessage ();
        }

        [Given (@"message has flag tag '(.*)'")]
        public void GivenMessageHasTag (string p0) {
            _message.Tags.Add (p0, "true");
        }

        [Given (@"message has tag '(.*)' which is '(.*)'")]
        public void GivenMessageHasTagWhichis (string p0, string p1) {
            _message.Tags.Add (p0, p1);
        }

        [Given (@"message has param '(.*)'")]
        public void GivenMessageObjectHasParam (string p0) {
            _message.Params.Add (p0);
        }

        [Given (@"message has prefix '(.*)'")]
        public void GivenMessageObjectHasPrefix (string p0) {
            _message.Prefix = p0;
        }

        [Then (@"message string equals '(.*)'")]
        public void ThenMessageStringEquals (string p0) {
            Assert.AreEqual (p0, _messageString);
        }

        [When (@"message string generated")]
        public void WhenMessageStringGenerated () {
            _messageString = _message.ToString ();
        }

        #endregion
    }
}
