# ``IrcMessage#``
> A fast IRC message parser written in C# for the .NET Framework

** 9/8/2015 **: After some code clean up, I now consider this complete software. 

## Installation
We currently only support NuGet for installation

    Install-Package IrcMessageSharp

## Usage

##### Basic usage as a parser
```C#
var line = ":matt!brainling@cloak PRIVMSG #csharp :Hello world!"

// Can throw FormatException
var message = IrcMessage.Parse(line);

// Exception safe
IrcMessage message;
if(IrcMessage.TryParse(line, out message)) {
    // We have a valid message here
}

// Create a new message
var message = new IrcMessage {
    Prefix = "matt!brainling@cloak"
    Command = "PRIVMSG"
}
message.Params.Add("#csharp");
message.Params.Add("Hello world!");

// Writes: :matt!brainling@cloak PRIVMSG #csharp :Hello world!
Console.WriteLine(message.ToString());

```

The IrcMessage object has the following properties:
- `Command string`: The command parsed from the IRC message
- `Prefix string`: A prefix if one was parsed from the message, otherwise String.Empty
- `Params IList<string>`: The parameters parsed from the message. Can be empty.
- `Tags IDictionary<string, string>`: The tags parsed from the message. Can be empty. Tags with no value (flags) will have the value 'true' in the dictionary.

In addition, the IrcMessage class offers some utility properties and functions.

- `IsPrefixHostmask bool`: Returns true if message has a prefix and that prefix is a hostmask in the form nick!user@host.
```C#
var message = IrcMessage.Parse(":brainling!matt@cloak PING");
var isHostMask = message.IsPrefixHostmask; // true

```

- `IsPrefixServer bool`: Returns true if message has a prefix and that prefix is a server in the form server.tld.*.
```C#
var message = IrcMessage.Parse(":test.irc.com PING");
var isServer = message.IsPrefixServer; // true

```

- `GetHostmaskFromPrefix Hostmask`: Returns a parsed hostmask object if the message has a prefix and that prefix is a hostmask, otherwise null.
```C#
var message = IrcMessage.Parse(":brainling!matt@cloak PING");
var hostmask = message.GetHostmaskFromPrefix();
Console.WriteLine(hostmask.Nickname); // brainling
Console.WriteLine(hostmask.Username); // matt
Console.WriteLine(hostmask.Hostname); // cloak
```

Hostmask has the following properties:
- `Nickname string`: Nickname provided in the hostmask (nick!*@*)
- `Username string`: Username provided in the hostmask (*!user@*)
- `Hostname string`: Hostname provided in the hostmask (*!*@host)

## Credit
Based on the work of **Fionn Kelleher** ([sigkell](https://github.com/sigkell)) and his excellent [irc-message JavaScript library](https://github.com/sigkell/irc-message)

## Tests
IrcMessage# has a full test suite built using SpecFlow. You can find all the specs that IrcMessage# passes in the 'spec' directory. As of this writing (11/25/2014) IrcMessage# passes the same minimum set of specs as irc-message.

## Suport
If you find a bug or issue, please report it here on GitHub and I'll fix it ASAP.
