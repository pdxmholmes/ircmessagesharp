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
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IrcMessageSharp.Spec {
    [Binding]
    public class IrcMessageSteps {
        private IrcMessage.Hostmask _hostmask;
        private string _message;
        private IrcMessage _parsedMessage;

        [Given (@"IRC message '(.*)'")]
        public void GivenIrcMessage (string p0) {
            _message = p0;
        }

        [Then (@"command equals '(.*)'")]
        public void ThenCommandEquals (string p0) {
            Assert.AreEqual (p0, _parsedMessage.Command);
        }

        [Then (@"hostmask has hostname '(.*)'")]
        public void ThenHostmaskHasHostname (string p0) {
            Assert.AreEqual (p0, _hostmask.Hostname);
        }

        [Then (@"hostmask has nickname '(.*)'")]
        public void ThenHostmaskHasNickname (string p0) {
            Assert.AreEqual (p0, _hostmask.Nickname);
        }

        [Then (@"hostmask has username '(.*)'")]
        public void ThenHostmaskHasUsername (string p0) {
            Assert.AreEqual (p0, _hostmask.Username);
        }

        [Then (@"param (\d+) equals '(.*)'")]
        public void ThenParamNEquals (int p0, string p1) {
            Assert.AreEqual (_parsedMessage.Params[p0 - 1], p1);
        }

        [Then (@"params has (\d+) items")]
        public void ThenParamsHasNItems (int p0) {
            Assert.AreEqual (p0, _parsedMessage.Params.Count);
        }

        [Then (@"params is empty")]
        public void ThenParamsIsEmpty () {
            Assert.IsEmpty (_parsedMessage.Params);
        }

        [Then (@"prefix equals '(.*)'")]
        public void ThenPrefixEquals (string p0) {
            Assert.AreEqual (p0, _parsedMessage.Prefix);
        }

        [Then (@"prefix is a hostmask")]
        public void ThenPrefixIsAHostmask () {
            Assert.IsTrue (_parsedMessage.IsPrefixHostmask);
        }

        [Then (@"prefix is a server")]
        public void ThenPrefixIsAServer () {
            Assert.IsTrue (_parsedMessage.IsPrefixServer);
        }

        [Then (@"prefix is blank")]
        public void ThenPrefixIsBlank () {
            Assert.IsTrue (String.IsNullOrEmpty (_parsedMessage.Prefix));
        }

        [Then (@"tag (.*) equals '(.*)'")]
        public void ThenTagSEquals (string p0, string p1) {
            Assert.AreEqual (_parsedMessage.Tags[p0], p1);
        }

        [Then (@"tags has (\d+) items")]
        public void ThenTagsHasNItems (int p0) {
            Assert.AreEqual (p0, _parsedMessage.Tags.Count);
        }

        [Then (@"tags is empty")]
        public void ThenTagsIsEmpty () {
            Assert.IsEmpty (_parsedMessage.Tags);
        }

        [When (@"hostmask is requested")]
        public void WhenHostmaskIsRequested () {
            _parsedMessage = IrcMessage.Parse (_message);
            _hostmask = _parsedMessage.GetHostmaskFromPrefix ();
        }

        [When (@"message is parsed")]
        public void WhenMessageIsParsed () {
            _parsedMessage = IrcMessage.Parse (_message);
        }
    }
}
