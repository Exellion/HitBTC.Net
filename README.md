# HitBTC.Net
The simple API wrapper for world-wide crypto exchange HitBTC (based on [API v2.0](https://api.hitbtc.com))

## Introduction
The API are separate on two global parts: REST and WebSockets. Each part provides market data access, that don't require any authentication. So, you can receive currencies, symbols, tickers, trades, orderbook and candles without any additional movements. But if you want to trade, receive personal account information and manage your funds, you must create an API key on https://hitbtc.com (Settings -> API keys -> New API key).

## Installation
Using [Nuget](https://www.nuget.org/packages/HitBTC.Net/) Package Manager:
```
PM> Install-Package HitBTC.Net -Version 1.0.7-beta
```

## Samples
### I. Rest API
#### 1) Market data
***Create REST API wrapper:***
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi();
```

***Receive all currencies:***
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

***Receive last 500 candles for symbol BTCUSD and 15 minutes period:***
```c#
using HitBTC.Net;
using HitBTC.Net.Models;

// Create REST API wrapper
var restApi = new HitRestApi();

// Receive candles in synchronous manner
var response = restApi.GetCandlesAsync("BTCUSD", HitPeriod.Minute15, 500).Result;

// Handle response like in a sample above
```

You can receive all other market data in the same manner:
```c#
using HitBTC.Net;
using HitBTC.Net.Models;

// Create REST API wrapper
var restApi = new HitRestApi();

// Receive specific currency
var response = restApi.GetCurrencyAsync("BTC").Result;

// Receive all available symbols
var response = restApi.GetSymbolsAsync().Result;

// Receive specific symbol
var response = restApi.GetSymbolAsync("BTCUSD").Result;

// Receive tickers for all symbols
var response = restApi.GetTickersAsync().Result;

// Receive ticker for specific symbol
var response = restApi.GetTickerAsync("BTCUSD").Result;

// Recieve trades history by timestamp borders
var response = restApi.GetTradesByTimestampAsync("BTCUSD", HitSort.Desc, DateTime.Now.AddHours(-1), DateTime.Now, 500).Result;

// Recieve trades history by ids borders
var response = restApi.GetTradesByIdsAsync("BTCUSD", HitSort.Desc, 1500, 2000, 500).Result;

// Receive orderbook
var response = restApi.GetOrderBookAsync("BTCUSD").Result;
```

#### 2) Trading and account management
***Create REST API with authentication:***
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi(new HitConfig
{
    ApiKey = "your API key here",
    Secret = "your secret here"
});
```

***Receive account trading balances:***
```c#
using HitBTC.Net;

// Create REST API wrapper
var restApi = new HitRestApi(new HitConfig
{
    ApiKey = "your API key here",
    Secret = "your secret here"
});

// Receive account trading balances
var response = await restApi.GetTradingBalancesAsync();

// Error handling
if (response.Error != null)
{
    Console.WriteLine($"Error. {response.Error}");
}
// Successful result
else
{
    foreach (var balance in response.Result)
    {
        Console.WriteLine($"Balance. {balance.Currency} {balance.Available} {balance.Reserved}");
    }
}

```

***Place new order:***
```c#
using HitBTC.Net;
using HitBTC.Net.Models;

// Create REST API wrapper
var restApi = new HitRestApi(new HitConfig
{
    ApiKey = "your API key here",
    Secret = "your secret here"
});

// Place buy limit order.
// Order type depends on parameters that you transfer:
// if you set only price - there will be limit order;
// only stopPrice - stop market order;
// both price and stopPrice - stop limit order;
// otherwise, without any price - market order.
var response = await restApi.CreateNewOrderAsync("BTCUSD", HitSide.Buy, 0.005m, price: 4500);

// Error handling
if (response.Error != null)
{
    Console.WriteLine($"Error. {response.Error}");
}
// Successful result
else
{
    var order = response.Result;

    // Do something with order
}

```

### II. WebSocket API
#### 1) Market data

***Receive all currencies:***
```c#
using HitBTC.Net;

// Create WebSocket API wrapper
var socketApi = new HitSocketApi();

// Connect to the remote server
await socketApi.ConnectAsync();

// Send request for all currencies and wait result. 
// Note, that calling method must be marked as async, or you can wait result in synchronous manner
var response = await socketApi.GetCurrenciesAsync();

// Error handling
if (response.Error != null)
{
    Console.WriteLine($"Error. {response.Error}");
}
// Successful result
else
{
    foreach (var currency in response.Result)
    {
        Console.WriteLine($"Currency. {currency.Id} {currency.FullName}");
    }
}

```

Also, you able to receive specific currency by name, all symbols list and specific symbol by name

***Subscribe to realtime notifications:***
```c#
using HitBTC.Net;
using HitBTC.Net.Communication;
using HitBTC.Net.Models;

// Create WebSocket API wrapper
var socketApi = new HitSocketApi();
socketApi.Notification += this.SocketApi_Notification;

// Connect to the remote server
await socketApi.ConnectAsync();

// Subscribe to ticker updates
await socketApi.SubscribeTickerAsync("BTCUSD");

// Subscribe to order book updates
await socketApi.SubscribeOrderbookAsync("BTCUSD");

// Subscribe to trades updates
await socketApi.SubscribeTradesAsync("BTCUSD");

// Subscribe to candles updates
await socketApi.SubscribeCandlesAsync("BTCUSD", HitPeriod.Day1);

private void SocketApi_Notification(HitSocketApi hitSocketApi, HitEventArgs e)
{
    if (e.SocketError != null)
    {
        // Handle error

        return;
    }

    switch(e.NotificationMethod)
    {
        case HitNotificationMethod.Ticker:
            var ticker = e.Ticker;

            // Your code;
            break;
        case HitNotificationMethod.SnapshotOrderBook:
            var orderBookSnapshot = e.OrderBook;

            // Your code;
            break;
        case HitNotificationMethod.UpdateOrderBook:
            var orderBookUpdate = e.OrderBook;

            // Your code;
            break;
        case HitNotificationMethod.SnapshotTrades:
            var tradesSnapshot = e.Trades;

            // Your code;
            break;
        case HitNotificationMethod.UpdateTrades:
            var tradesUpdate = e.Trades;

            // Your code;
            break;
        case HitNotificationMethod.SnapshotCandles:
            var candlesSnapshot = e.Candles;

            // Your code;
            break;
        case HitNotificationMethod.UpdateCandles:
            var candlesUpdate = e.Candles;

            // Your code;
            break;
        case HitNotificationMethod.ActiveOrders:
            var activeOrders = e.ActiveOrders;

            // Your code;
            break;
    }
}
```

#### 2) Trading
***Place order and handle realtime order updates***
```c#
using HitBTC.Net;
using HitBTC.Net.Communication;
using HitBTC.Net.Models;

// Create WebSocket API wrapper
var socketApi = new HitSocketApi(new HitConfig
{
    ApiKey = "your API key here",
    Secret = "your secret here"
});
socketApi.Notification += this.SocketApi_Notification;

// Connect to the remote server
await socketApi.ConnectAsync();

// Subscribe to reports updates (any order updates such as open, modify, cancel, etc.)
await socketApi.SubscribeReportsAsync();

// Place buy limit order.
// Order type depends on parameters that you transfer:
// if you set only price - there will be limit order;
// only stopPrice - stop market order;
// both price and stopPrice - stop limit order;
// otherwise, without any price - market order.
var response = await socketApi.PlaceNewOrderAsync("BTCUSD", HitSide.Buy, 0.005m, price: 4500);

// Error handling
if (response.Error != null)
{
    Console.WriteLine($"Error. {response.Error}");
}
// Successful result
else
{
    var report = response.Result;

    Console.WriteLine($"Order update. {report.ClientOrderId} {report.Symbol} {report.OrderType} {report.TimeInForce} {report.Price}");
}

private void SocketApi_Notification(HitSocketApi hitSocketApi, HitEventArgs e)
{
    if (e.SocketError != null)
    {
        // Handle error

        return;
    }

    switch(e.NotificationMethod)
    {
        case HitNotificationMethod.Report:
            var report = e.Report;

            Console.WriteLine($"Order update. {report.ClientOrderId} {report.Symbol} {report.OrderType} {report.TimeInForce} {report.Price}");
            break;
    }
}
```
