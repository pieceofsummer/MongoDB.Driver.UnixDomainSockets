# MongoDB.Driver.UnixDomainSockets

MongoDB server can be configured to listen for local connections on a Unix domain socket, e.g. `/tmp/mongodb-27017.sock` (which should be somewhat faster than standard TCP socket).
Unfortunately, the official [MongoDB.Driver](https://github.com/mongodb/mongo-csharp-driver) doesn't support connecting to Unix sockets yet.

This package attempts to fix this, until the proper native support is added to the driver.

## How it works?

Even though it is hardly used in typical client scenarios, `MongoClientSettings` provides a callback to configure `ClusterBuilder` before actually connecting to a server/cluster.
And `ClusterBuilder` allows to register a custom stream factory implementation (on top of standard `SslStreamFactory`/`TcpStreamFactory`).

## Typical setup

In most cases, you just need to add `.WithUnixDomainSockets()` call when passing `MongoClientSettings` to a `MongoClient` constructor:

```c#
var url = MongoUrl.Create("mongodb://%2Ftmp%2Fmongodb-27017.sock/admin");
var settings = MongoClientSettings.FromUrl(url);
var client = new MongoClient(settings.WithUnixDomainSockets());
```

## Advanced setup

`WithUnixDomainSockets` extension method creates a frozen copy of `MongoClientSettings`, so you don't mess with `ClusterConfigurator` callback afterwards.

If (for whatever weird reason) you have own `ClusterConfigurator` and don't want to create a frozen copy of `MongoClientSettings`, you can use another extension method:

```c#
var settings = new MongoClientSettings();
// ...
settings.ClusterConfigurator = clusterBuilder =>
{
    clusterBuilder.EnableUnixDomainSockets();
    // your custom actions
};
var client = new MongoClient(settings);
```
