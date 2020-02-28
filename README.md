# ReCom
A Reactive Networking library that makes the messaging through TCP easy
ReCom is currently in a very early devlopment stage with very limited functionality.

## Getting started
Getting started is easy, we just need to configure 3 things:
- Contracts
- Server 
- Client

### Contract
First thing you'll need your contract, this need to be available to both server and client.
In this example we have a `PingPongCommand`
 ```
public class PingPongCommand
{
    public string Message { get; set; }
}
```
### Server
In the server we need to configure a couple of things:
- A command handler
- IoC Container (activator)
- ReCom it self

#### Command Handler
In the handler below we implement `ICommand<>` and send the same message back to the client.

```
public class PingPongCommandHandler : ICommand<PingPongCommand>
{
    public async Task Handle(PingPongCommand command, Connection connection)
    {
        Console.WriteLine(command.Message);
        await connection.Send(new PingPongCommand() {Message = "Pong!"});
    }
}
```

#### IoC Container
In order to not have to many dependencies ReCom has a custom IoC container, this is called the DefaultActivator it does the most basic this that a other container does as well.

- Constructor Injection
- Circular dependency detection.
- Factories
- Lifetime management.
- Service location
- instance registration

*Registering dependencies*
Create a new container and register the handler, if the handler has any dependencies those should be registered as well.

```
  var activator = new DefaultActivator();
  activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
```
#### ReCom
This is the easiest part you just `Configure.With()` the activator an start with the server role

```
Configure.With(activator).Start(new ReComServerRole(IPAddress.Any, 13000));
```

### Client 
The client is configure in exacly the same way as the server the only difference is the Role

#### Command handler 
This is only here to give a complete overview.
```
public class PingPongCommandHandler : ICommand<PingPongCommand>
{
    public Task Handle(PingPongCommand command, Connection connection)
    {
        Console.WriteLine(command.Message);
        return Task.CompletedTask;
    }
}
```
#### ReCom Client Setup
The start with client role will this time return a connection that can be used to send messages to the server.

```
var activator = new DefaultActivator();
activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
var connection = Configure
    .With(activator)
    .Start(new ReComClientRole("127.0.0.1", 13000));

while (true)
{
    //Sending ping pong command to server and sleeping for a second
    connection.Send(new PingPongCommand() { Message = "Ping!" }).Wait();
    Thread.Sleep(1000);
}
```
## Built in messages
When a system event occurs a message is sent to clients they are handled internally but can also be implemented by your code

### InitiateHandshakeMessage
This is sent from the client to the server and can be implemented in the server. It is fired when a client is connecting and is trying to identify itself.

### CompleteHandshakeMessage 
Is sent from the server to the client when the handshake has succeded.

