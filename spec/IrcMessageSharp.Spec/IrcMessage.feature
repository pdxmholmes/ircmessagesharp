Feature: IrcMessage
	In order to correctly operate I need to parse IRC messages

Scenario: a message with command only
	Given IRC message 'FOO'
	When message is parsed
	Then tags is empty
	Then params is empty
	Then prefix is blank
	Then command equals 'FOO'

Scenario: a message with prefix and command
	Given IRC message ':test.prefix FOO'
	When message is parsed
	Then tags is empty
	Then params is empty
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with prefix and command and trailing spaces
	Given IRC message ':test.prefix FOO      '
	When message is parsed
	Then tags is empty
	Then params is empty
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with command, middle and trailing parameters
	Given IRC message 'FOO #Test :Test parameter'
	When message is parsed
	Then tags is empty
	Then params has 2 items
	Then param 1 equals '#Test'
	Then param 2 equals 'Test parameter'
	Then prefix is blank
	Then command equals 'FOO'

Scenario: a message with prefix, command, middle and trailing parameters
	Given IRC message ':test.prefix FOO #Test :Test parameter'
	When message is parsed
	Then tags is empty
	Then params has 2 items
	Then param 1 equals '#Test'
	Then param 2 equals 'Test parameter'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with prefix, command, middle and trailing parameters with spaces
	Given IRC message ':test.prefix FOO #Test :Test     parameter    '
	When message is parsed
	Then tags is empty
	Then params has 2 items
	Then param 1 equals '#Test'
	Then param 2 equals 'Test     parameter    '
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with prefix, command, middle and trailing parameters with a lot of extra spaces
	Given IRC message ':test.prefix             FOO               #Test              :Test parameter'
	When message is parsed
	Then tags is empty
	Then params has 2 items
	Then param 1 equals '#Test'
	Then param 2 equals 'Test parameter'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with prefix, command and multiple middle parameters
	Given IRC message ':test.prefix FOO test1 test2 test3'
	When message is parsed
	Then tags is empty
	Then params has 3 items
	Then param 1 equals 'test1'
	Then param 2 equals 'test2'
	Then param 3 equals 'test3'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with command and multiple middle parameters
	Given IRC message 'FOO test1 test2 test3'
	When message is parsed
	Then tags is empty
	Then params has 3 items
	Then param 1 equals 'test1'
	Then param 2 equals 'test2'
	Then param 3 equals 'test3'
	Then prefix is blank
	Then command equals 'FOO'

Scenario: a message with command and multiple middle parameters with spaces
	Given IRC message 'FOO     test1        test2         test3'
	When message is parsed
	Then tags is empty
	Then params has 3 items
	Then param 1 equals 'test1'
	Then param 2 equals 'test2'
	Then param 3 equals 'test3'
	Then prefix is blank
	Then command equals 'FOO'

Scenario: a message with command, multiple middle and trailing parameters
	Given IRC message 'FOO test1 test2 test3 :Test parameter'
	When message is parsed
	Then tags is empty
	Then params has 4 items
	Then param 1 equals 'test1'
	Then param 2 equals 'test2'
	Then param 3 equals 'test3'
	Then param 4 equals 'Test parameter'
	Then prefix is blank
	Then command equals 'FOO'

Scenario: a message with prefix, command middle parameter with colon and trailing paremter
	Given IRC message ':test.prefix FOO #Te:st :Test parameter'
	When message is parsed
	Then tags is empty
	Then params has 2 items
	Then param 1 equals '#Te:st'
	Then param 2 equals 'Test parameter'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with tags, prefix, command, middle and trailing parameters
	Given IRC message '@tag1=test;tag2 :test.prefix FOO #Test :Test parameter'
	When message is parsed
	Then tags has 2 items
	Then tag tag1 equals 'test'
	Then tag tag2 equals 'true'
	Then params has 2 items
	Then param 1 equals '#Test'
	Then param 2 equals 'Test parameter'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with tags, prefix, command, multiple middle and trailing parameters
	Given IRC message '@tag1=test;tag2 :test.prefix FOO test1 test2 test3 :Test parameter'
	When message is parsed
	Then tags has 2 items
	Then tag tag1 equals 'test'
	Then tag tag2 equals 'true'
	Then params has 4 items
	Then param 1 equals 'test1'
	Then param 2 equals 'test2'
	Then param 3 equals 'test3'
	Then param 4 equals 'Test parameter'
	Then prefix equals 'test.prefix'
	Then command equals 'FOO'

Scenario: a message with hostmask prefix and command, testing prefix for hostmask
	Given IRC message ':nick!user@host FOO'
	When message is parsed
	Then prefix is a hostmask

Scenario: a message with server prefix and command, testing prefix for server
	Given IRC message ':test.irc.com FOO'
	When message is parsed
	Then prefix is a server

Scenario: a message with hostmask prefix and command, getting hostmask from prefix
	Given IRC message ':nick!user@host FOO'
	When hostmask is requested
	Then hostmask has nickname 'nick'
	Then hostmask has username 'user'
	Then hostmask has hostname 'host'
