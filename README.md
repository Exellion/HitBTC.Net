# HitBTC.Net
The simple API wrapper for world-wide crypto exchange HitBTC (based on [API v2.0](https://api.hitbtc.com))

## Introduction
The API are separate on two global parts: REST and WebSockets. Each part provides market data access, that don't require any authentication. So, you can receive currencies, symbols, tickers, trades, orderbook and candles without any additional movements. But if you want to trade, receive personal account information and manage your funds, you must create an API key on https://hitbtc.com (Settings -> API keys -> New API key).

## Installation
Using [Nuget](https://www.nuget.org/packages/HitBTC.Net/) Package Manager:
```
PM> Install-Package HitBTC.Net
```

## Samples
### Rest API
Create REST API wrapper:
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi();
```

Create REST API with authentication:
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi(new HitConfig
{
    ApiKey = "your API key here",
    Secret = "your secret here"
});
```

Receive all currencies:
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi();

// Send request for all currencies and wait result. 
// Note, that calling method must be marked as async, or you can wait result in synchronous manner
var response = await restApi.GetCurrenciesAsync();

// Error handling
if (response.Error != null)
{
    Console.WriteLine($"Error. {response.Error}");
}
// Successful result
else
{
    foreach(var currency in response.Result)
    {
        Console.WriteLine($"Currency. {currency.Id} {currency.FullName}");
    }
}
```

Receive last 500 candles for symbol BTCUSD and 15 minutes period:
```c#
using HitBTC.Net;
using HitBTC.Net.Models;

// Create REST API wrapper
var restApi = new HitRestApi();

// Receive candles in synchronous manner
var response = restApi.GetCandlesAsync("BTCUSD", HitPeriod.Minute15, 500).Result;

// Handle response like in a sample above
```
