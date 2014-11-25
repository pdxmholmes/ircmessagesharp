Feature: IrcMessageGeneration
	To make network operations easier, I'd like to be able to generate a valid IRC 
	message string from a message object

Scenario: a message object with no command
	Given a message with no command
	When message string generated
	Then message string equals ''

Scenario: a message with command that is only whitspace
	Given a message with command '            '
	When message string generated
	Then message string equals ''

Scenario: a message with command that has whitespace
	Given a message with command '   FOO   '
	When message string generated
	Then message string equals 'FOO'

Scenario: a message object with command only
	Given a message with command 'FOO'
	When message string generated
	Then message string equals 'FOO'

Scenario: a message object with prefix and command
	Given a message with command 'FOO'
	Given message has prefix 'test.prefix'
	When message string generated
	Then message string equals ':test.prefix FOO'

Scenario: a message object with prefix, command and middle parameters
	Given a message with command 'FOO'
	Given message has prefix 'test.prefix'
	Given message has param 'test1'
	Given message has param 'test2'
	When message string generated
	Then message string equals ':test.prefix FOO test1 test2'

Scenario: a message object with prefix, command and middle parameters with spaces
	Given a message with command 'FOO'
	Given message has prefix 'test.prefix'
	Given message has param 'test        1         '
	Given message has param 'test2'
	When message string generated
	Then message string equals ':test.prefix FOO test 1 test2'

Scenario: a message object with prefix, command, middle parameters and trailing parameter
	Given a message with command 'FOO'
	Given message has prefix 'test.prefix'
	Given message has param 'test1'
	Given message has param 'test2'
	Given message has param 'Test parameter'
	When message string generated
	Then message string equals ':test.prefix FOO test1 test2 :Test parameter'

Scenario: a message object with prefix, command, middle parameters with spaces and trailing parameter
	Given a message with command 'FOO'
	Given message has prefix 'test.prefix'
	Given message has param 'test    1     '
	Given message has param 'test2'
	Given message has param 'Test parameter'
	When message string generated
	Then message string equals ':test.prefix FOO test 1 test2 :Test parameter'

Scenario: a message object with tags, prefix and command	
	Given a message with command 'FOO'
	Given message has tag 'tag1' which is 'test'
	Given message has flag tag 'tag2'
	Given message has prefix 'test.prefix'
	When message string generated
	Then message string equals '@tag1=test;tag2 :test.prefix FOO'

Scenario: a message object with tags, prefix, command and middle parameters
	Given a message with command 'FOO'
	Given message has tag 'tag1' which is 'test'
	Given message has flag tag 'tag2'
	Given message has prefix 'test.prefix'
	Given message has param 'test1'
	Given message has param 'test2'
	When message string generated
	Then message string equals '@tag1=test;tag2 :test.prefix FOO test1 test2'

Scenario: a message object with tags, prefix, command, middle and trailing parameters
	Given a message with command 'FOO'
	Given message has tag 'tag1' which is 'test'
	Given message has flag tag 'tag2'
	Given message has prefix 'test.prefix'
	Given message has param 'test1'
	Given message has param 'test2'
	Given message has param 'Test parameter'
	When message string generated
	Then message string equals '@tag1=test;tag2 :test.prefix FOO test1 test2 :Test parameter'

Scenario: a message object with tags, prefix, command, middle and trailing parameterse with spaces
	Given a message with command '  FOO   '
	Given message has tag 'tag1' which is '  test   '
	Given message has flag tag '   tag2   '
	Given message has prefix '   test.prefix  '
	Given message has param ' test1  '
	Given message has param 'test  2'
	Given message has param 'Test parameter'
	When message string generated
	Then message string equals '@tag1=test;tag2 :test.prefix FOO test1 test 2 :Test parameter'
