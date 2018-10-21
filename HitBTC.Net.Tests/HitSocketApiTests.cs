using System;
using System.Threading;
using System.Threading.Tasks;
using HitBTC.Net.Communication;
using HitBTC.Net.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HitBTC.Net.Tests
{
    [TestClass]
    public class HitSocketApiTests
    {
        [TestMethod]
        public void ConnectAsyncDisconnectAsyncTest()
        {
            var timeout = TimeSpan.FromSeconds(10);

            var cancellationTokenSource = new CancellationTokenSource(timeout);

            var lastConnectionState = HitConnectionState.PrepareToConnect;

            void ClientConnectionStateChanged(HitSocketApi hitSocketApi, HitEventArgs e)
            {
                lastConnectionState = e.ConnectionState;

                if (lastConnectionState == HitConnectionState.Connected || lastConnectionState == HitConnectionState.Disconnected || lastConnectionState == HitConnectionState.Failed)
                    cancellationTokenSource.Cancel();
            }

            var client = new HitSocketApi();
            client.ConnectionStateChanged += ClientConnectionStateChanged;

            client.ConnectAsync().Wait(cancellationTokenSource.Token);

            try
            {
                Task.Delay(timeout, cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
            }
            catch
            { }

            Assert.AreEqual(HitConnectionState.Connected, lastConnectionState);

            client.DisconnectAsync().Wait();

            Assert.AreEqual(HitConnectionState.Disconnected, lastConnectionState);
        }

        [TestMethod]
        public void GetCurrencyAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.GetCurrencyAsync("eth").Result;

            ResponseBasicCheck(response);
        }

        [TestMethod]
        public void GetCurrenciesAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.GetCurrenciesAsync().Result;

            ResponseBasicCheck(response);
        }

        [TestMethod]
        public void GetSymbolAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.GetSymbolAsync("ETHBTC").Result;

            ResponseBasicCheck(response);
        }

        [TestMethod]
        public void GetSymbolsAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.GetSymbolsAsync().Result;

            ResponseBasicCheck(response);
        }

        [TestMethod]
        public void SubscribeUnsubscribeTickerAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.SubscribeTickerAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);

            response = socketApi.UnsubscribeTickerAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);
        }

        [TestMethod]
        public void SubscribeUnsubscribeOrderbookAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.SubscribeOrderbookAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);

            response = socketApi.UnsubscribeOrderbookAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);
        }

        [TestMethod]
        public void SubscribeUnsubscribeTradesAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.SubscribeTradesAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);

            response = socketApi.UnsubscribeTradesAsync("ETHBTC").Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);
        }

        [TestMethod]
        public void SubscribeUnsubscribeCandlesAsyncTest()
        {
            var socketApi = CreateConnectedApi();

            var response = socketApi.SubscribeCandlesAsync("ETHBTC", HitPeriod.Minute1).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);

            response = socketApi.UnsubscribeCandlesAsync("ETHBTC", HitPeriod.Minute1).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(true, response.Result);
        }

        //[TestMethod]
        //public void GetTradesByTimestampAsyncTest()
        //{
        //    var socketApi = CreateConnectedApi();
        //    string symbol = "ETHBTC";
        //    //DateTime from = new DateTime(2018, 5, 5, 12, 59, 0, DateTimeKind.Utc);
        //    //DateTime till = new DateTime(2018, 5, 5, 13, 0, 0, DateTimeKind.Utc);

        //    var response = socketApi.GetTradesByTimestampAsync(symbol, HitSort.Desc).Result;

        //    ResponseBasicCheck(response);

        //    Assert.AreNotEqual(0, response.Result.Length);
        //}

        //[TestMethod]
        //public void GetTradesByIdAsyncTest()
        //{
        //    var socketApi = CreateConnectedApi();
        //    string symbol = "ETHBTC";

        //    var response = socketApi.GetTradesByIdAsync(symbol, HitSort.Desc, limit: 3).Result;

        //    ResponseBasicCheck(response);

        //    Assert.AreNotEqual(0, response.Result.Length);
        //}

        #region Misc
        private static HitSocketApi CreateConnectedApi()
        {
            var socketApi = new HitSocketApi();

            socketApi.ConnectAsync().Wait();

            Assert.AreEqual(HitConnectionState.Connected, socketApi.ConnectionState);

            return socketApi;
        }

        private static void ResponseBasicCheck<T>(HitResponse<T> hitResponse)
        {
            Assert.IsNotNull(hitResponse);
            Assert.IsNull(hitResponse.Error);
            Assert.IsNotNull(hitResponse.Result);
        }
        #endregion
    }
}
