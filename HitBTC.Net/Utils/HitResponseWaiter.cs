using System;
using System.Threading;
using System.Threading.Tasks;
using HitBTC.Net.Communication;
using Newtonsoft.Json.Linq;

namespace HitBTC.Net.Utils
{
    internal class HitResponseWaiter<T> : IHitResponseWaiter
    {
        private HitResponse<T> response;

        private readonly CancellationTokenSource cancellationTokenSource;

        public HitResponseWaiter(TimeSpan timeout) => this.cancellationTokenSource = new CancellationTokenSource(timeout);

        public async Task WaitAsync(CancellationToken externalToken)
        {
            try
            {
                while (!this.cancellationTokenSource.IsCancellationRequested && !externalToken.IsCancellationRequested)
                    await Task.Delay(100);
            }
            catch(TaskCanceledException tce)
            { }
        }

        public void Abort() => this.cancellationTokenSource.Cancel();
        
        public void TryParseResponse(JObject jObject)
        {
            try
            {
                this.response = jObject.ToObject<HitResponse<T>>();
            }
            catch (Exception e)
            {
            }
            finally
            {
                this.Abort();
            }
        }

        public HitResponse<TResult> GetResponse<TResult>() => this.response as HitResponse<TResult>;
    }

    internal interface IHitResponseWaiter
    {
        Task WaitAsync(CancellationToken externalToken);
        
        void TryParseResponse(JObject jObject);

        HitResponse<TResult> GetResponse<TResult>();
    }
}