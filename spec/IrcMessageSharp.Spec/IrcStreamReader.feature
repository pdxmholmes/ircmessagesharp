Feature: IrcStreamReader
	In order to easily get messages from a network stream, I need a stream
	reader which handles IRC messages syncronously and asynchronously

Scenario: a stream with a valid IRC message
	Given string stream
	Given stream has line ':test.prefix FOO'
	When message is extracted from stream
	Then message has prefix 'test.prefix'
	Then message has command 'FOO'

Scenario: a stream with a valid IRC message read asynchronously
	Given string stream
	Given stream has line ':test.prefix FOO'
	When message is extracted from stream asynchronously
	Then message has prefix 'test.prefix'
	Then message has command 'FOO'
