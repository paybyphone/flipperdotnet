# FlipperDotNet

A .Net port of the client portion of the feature flagging [Flipper](https://github.com/jnunemaker/flipper) project.

In addition to fully enabling or disabling features, it allows enabling for a percentage of users, a percentage of time,
or specific users.

The current backend stores are in-memory and [Consul](https://www.consul.io).

TODO: enable by groups.

## Installation

Currently available at <https://github.com/paybyphone/flipperdotnet>

TODO: Make it a nuget package.

## General Usage

Each instance of the client caches values to reduce traffic. Each request should create a new client.

### Instantiating the client

    using FlipperDotNet;
    using FlipperDotNet.Adapter;
    
    var adapter = new MemoryAdapter();
    var flipper = new Flipper(adapter);

That's it! You will be using an in-memory adapter for the client.

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

## Using the Consul Adapter

The Consul adapter uses the [Consul.NET](https://github.com/PlayFab/consuldotnet/) client.

    using FlipperDotNet.ConsulAdapter;
    
    var client = new Consul.Client();
    var adapter = new ConsulAdapter(client);
    var flipper = new Flipper(adapter);

Since Consul is intended to be set up as a single global cluster in one datacentre, with possible replication between
datacentres, the flipper data can (and should) be namespaced:

    var adapter = new ConsulAdapter(client, "my/flipper/name/space");
