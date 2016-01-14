# FlipperDotNet

A .Net port of the client portion of the feature flagging [Flipper](https://github.com/jnunemaker/flipper) project.

In addition to fully enabling or disabling features, it allows enabling for a percentage of users, a percentage of time,
or specific users.

The current backend stores are in-memory, [Consul](https://www.consul.io), and [Redis](http://redis.io).

TODO: enable by groups.

## Installation

FlipperDotNet is available as a NuGet package. Include the adapter package that you are using, either
[FlipperDotNet.Consul](https://www.nuget.org/packages/FlipperDotNet.Consul) or
[FlipperDotNet.Redis](https://www.nuget.org/packages/FlipperDotNet.Redis/). They will include the core package
[FlipperDotNet.Core](https://www.nuget.org/packages/FlipperDotNet.Core/)

Source code is available at <https://github.com/paybyphone/flipperdotnet>

## General Usage

Each instance of the client caches values to reduce traffic. Each request should create a new client.

### Instantiating the client

    using FlipperDotNet;
    using FlipperDotNet.Adapter;
    
    var adapter = new MemoryAdapter();
    var flipper = new Flipper(adapter);

That's it! You will be using an in-memory adapter for the client.

### Adapter Exceptions

All exceptions thrown by the underlying adapter are wrapped in an `AdapterRequestException`, with the
original being passed in the `InnerException` property.

### Setting Features

Features can be fully enabled or disabled from the client using

    flipper.Enable("search");
    flipper.Disable("search");

For more complex settings, use access the feature

    var searchFeature = flipper.Feature("search");
    
    searchFeature.Enable();
    searchFeature.Disable();
    
    searchFeature.EnablePercentageOfTime(10);
    searchFeature.DisablePercentageOfTime();
    
    searchFeature.EnablePercentageOfActors(25);
    searchFeature.DisablePercentageOfActors();

"Actors" are a generic term for users. Non-person users of a service can also be represented by a Actor. Actor classes
implement the `IFlipperActor` interface.

    public interface IFlipperActor
    {
        string FlipperId { get; }
    }

`FlipperId` returns a string representation of the Id for the actor.

    class MyActor : IFlipperActor { ... }
    
    var myActor = new MyActor(...);
    searchFeature.EnableActor(myActor);
    searchFeature.DisableActor(myActor);

### Checking Features

To check if a feature is enabled

    searchFeature.IsEnabled;

will return `True` if the feature is fully enabled or if the percentage of time check matches.

To check for an actor

    searchFeature.IsEnabledFor(myActor);

will return `True` if the feature is fully enabled or any of the following are enabled:

* for the percentage of time,
* for the percentage of actors, or
* for the specific actor

### Collecting Statistics

FlipperDotNet can collect timing and count statistics using [Statsd](https://github.com/etsy/statsd/) or compatible systems.

Pass an Instrumenter when creating the Flipper object.

    var statsdClient = new Statsd("localhost", 8125);
    var instrumenter = new StatsdInstrumenter(statsdClient);
    var flipper = new Flipper(adapter, instrumenter);
  

## Using the Consul Adapter

The Consul adapter uses the [Consul.NET](https://github.com/PlayFab/consuldotnet/) client.

    using FlipperDotNet.ConsulAdapter;
    
    var client = new Consul.Client();
    var adapter = new ConsulAdapter(client);
    var flipper = new Flipper(adapter);

Since Consul is intended to be set up as a single global cluster in one datacentre, with possible replication between
datacentres, the flipper data can (and should) be namespaced:

    var adapter = new ConsulAdapter(client, "my/flipper/name/space");

## Using the Redis Adapter

The Redis adapter uses the [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) client.

Note: The NuGet package does not work on Mono. According to
[this issue](https://github.com/StackExchange/StackExchange.Redis/issues/233), when using Mono, download the code
and build it using `monobuild.bash`.

    using FlipperDotNet.RedisAdapter;
    
    var multiplexer = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
    
    var	database = multiplexer.GetDatabase();
    var adapter = new RedisAdapter(database);
    var flipper = new Flipper(adapter);
