using HitBTC.Net.Communication;
using HitBTC.Net.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HitBTC.Net.Tests
{
    [TestClass]
    public class HitRestApiTests
    {
        [TestMethod]
        public void GetCurrenciesAsyncTest()
        {
            var client = new HitRestApi();

            var response = client.GetCurrenciesAsync().Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetCurrencyAsyncTest()
        {
            var client = new HitRestApi();
            var currency = "BTC";

            var response = client.GetCurrencyAsync(currency).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(currency, response.Result.Id);
        }

        [TestMethod]
        public void GetSymbolsAsyncTest()
        {
            var client = new HitRestApi();

            var response = client.GetSymbolsAsync().Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetSymbolAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "ETHBTC";

            var response = client.GetSymbolAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(symbol, response.Result.Id);
        }

        [TestMethod]
        public void GetTickersAsyncTest()
        {
            var client = new HitRestApi();

            var response = client.GetTickersAsync().Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetTickerAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetTickerAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(symbol, response.Result.Symbol);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetTradesByTimestampAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncDescSortingTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var sort = HitSort.Desc;

            var response = client.GetTradesByTimestampAsync(symbol, sort).Result;

            ResponseBasicCheck(response);

            Assert.IsTrue(response.Result.Length >= 2);
            Assert.IsTrue(response.Result[0].Timestamp > response.Result[1].Timestamp);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncAscSortingTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var sort = HitSort.Asc;

            var response = client.GetTradesByTimestampAsync(symbol, sort).Result;

            ResponseBasicCheck(response);

            Assert.IsTrue(response.Result.Length >= 2);
            Assert.IsTrue(response.Result[0].Timestamp < response.Result[1].Timestamp);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncFromTimestampTillTimestampTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var from = new DateTime(2018, 5, 5, 12, 59, 0, DateTimeKind.Utc);
            var till = new DateTime(2018, 5, 5, 13, 0, 0, DateTimeKind.Utc);
            var expectedTradesCount = 19; // tested experimentally

            var response = client.GetTradesByTimestampAsync(symbol, from: from, till: till).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(expectedTradesCount, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncLimitTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var limit = 555;

            var response = client.GetTradesByTimestampAsync(symbol, limit: limit).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(limit, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByTimestampAsyncOffsetTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var offset = 10000;

            var response = client.GetTradesByTimestampAsync(symbol, offset: offset).Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetTradesByIdsAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncDescSortingTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var sort = HitSort.Desc;

            var response = client.GetTradesByTimestampAsync(symbol, sort).Result;

            ResponseBasicCheck(response);

            Assert.IsTrue(response.Result.Length >= 2);
            Assert.IsTrue(response.Result[0].Timestamp < response.Result[1].Timestamp);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncAscSortingTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var sort = HitSort.Asc;

            var response = client.GetTradesByIdsAsync(symbol, sort).Result;

            ResponseBasicCheck(response);

            Assert.IsTrue(response.Result.Length >= 2);
            Assert.IsTrue(response.Result[0].Timestamp < response.Result[1].Timestamp);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncFromIdTillIdTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            long fromId = 287719394;
            long tillId = 287720650;

            var response = client.GetTradesByIdsAsync(symbol, from: fromId, till: tillId).Result;

            ResponseBasicCheck(response);

            Assert.IsTrue(response.Result.Length >= 2);
            Assert.AreEqual(tillId, response.Result.First().Id);
            Assert.AreEqual(fromId, response.Result.Last().Id);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncLimitTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var limit = 555;

            var response = client.GetTradesByTimestampAsync(symbol, limit: limit).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(limit, response.Result.Length);
        }

        [TestMethod]
        public void GetTradesByIdsAsyncOffsetTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var offset = 10000;

            var response = client.GetTradesByTimestampAsync(symbol, offset: offset).Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Length);
        }

        [TestMethod]
        public void GetOrderBookAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetOrderBookAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Asks.Length);
            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Bids.Length);
        }

        [TestMethod]
        public void GetOrderBookAsyncWithZeroLimitTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetOrderBookAsync(symbol, 0).Result;

            ResponseBasicCheck(response);

            Assert.AreNotEqual(0, response.Result.Asks.Length);
            Assert.AreNotEqual(0, response.Result.Bids.Length);
        }

        [TestMethod]
        public void GetOrderBookAsyncWithNullLimitTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetOrderBookAsync(symbol, null).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Asks.Length);
            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Bids.Length);
        }

        [TestMethod]
        public void GetCandlesAsyncTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";

            var response = client.GetCandlesAsync(symbol).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Length);
        }

        [TestMethod]
        public void GetCandlesAsyncMinute1Test()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var period = HitPeriod.Minute1;

            var response = client.GetCandlesAsync(symbol, period).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(DEFAULT_CONTENT_LENGTH, response.Result.Length);
            Assert.AreEqual(response.Result[0].Timestamp - response.Result[1].Timestamp, TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void GetCandlesAsyncLimitTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var limit = 555;

            var response = client.GetCandlesAsync(symbol, limit: limit).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(limit, response.Result.Length);
        }
        
        [TestMethod]
        public void GetCandlesAsyncFromTillTest()
        {
            var client = new HitRestApi();
            var symbol = "BTCUSD";
            var period = HitPeriod.Hour1;
            var from = new DateTime(2020, 4, 5);
            var till = new DateTime(2020, 4, 6);

            var response = client.GetCandlesAsync(symbol, from, till, period).Result;

            ResponseBasicCheck(response);

            Assert.AreEqual(24, response.Result.Length);
            Assert.AreEqual(response.Result[1].Timestamp - response.Result[0].Timestamp, TimeSpan.FromHours(1));
        }

        #region Misc
        private const int DEFAULT_CONTENT_LENGTH = 100;

        private static void ResponseBasicCheck<T>(HitResponse<T> hitResponse)
        {
            Assert.IsNull(hitResponse.Error);
            Assert.IsNotNull(hitResponse.Result);
        }
        #endregion Misc
    }
}