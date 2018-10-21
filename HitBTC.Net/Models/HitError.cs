using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitError
    {
        /// <summary>
        /// Error code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Error description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        public override string ToString() => $"Code: {this.Code} | Msg: {this.Message} | Descr: {this.Description}";
    }
}