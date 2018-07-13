# ReWork
A Reactive Networking library that makes the messaging through TCP easy

## Built in messages
When a system event occurs a message is sent to clients they are handled internally but can also be implemented by your code

### InitiateHandshakeMessage
This is sent from the client to the server and can be implemented in the server. It is fired when a client is connecting and is trying to identify itself.

### CompleteHandshakeMessage 
Is sent from the server to the client when the handshake has succeded.

