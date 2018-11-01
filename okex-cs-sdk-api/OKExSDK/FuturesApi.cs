using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OKExSDK.Models.Futures;
using OKExSDK.Models.General;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OKExSDK
{
    public class FuturesApi : SdkApi
    {
        private string FUTURES_SEGMENT = "api/futures/v3";

        /// <summary>
        /// FuturesApi构造函数
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="secret">Secret</param>
        /// <param name="passPhrase">Passphrase</param>
        public FuturesApi(string apiKey, string secret, string passPhrase) : base(apiKey, secret, passPhrase) { }

        /// <summary>
        /// 获取合约账户所有的持仓信息。
        /// </summary>
        /// <returns>账户所有持仓信息</returns>
        public async Task<JObject> getPositions()
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/position";

            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();

                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 获取某个合约的持仓信息
        /// </summary>
        /// <param name="instrument_id">合约ID</param>
        /// <returns>该合约全部持仓</returns>
        public async Task<JObject> getPositionById(string instrument_id)
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/{instrument_id}/position";
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 所有币种合约账户信息
        /// </summary>
        /// <returns>合约账户信息</returns>
        public async Task<JObject> getAccounts()
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/accounts";
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 获取单个币种的合约账户信息
        /// </summary>
        /// <param name="currency">币种，如：btc</param>
        /// <returns>该币种的合约账户信息</returns>
        public async Task<JObject> getAccountByCurrency(string currency)
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/accounts/{currency}";
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 获取合约账户币种杠杆倍数
        /// </summary>
        /// <param name="currency">币种，如：btc</param>
        /// <returns></returns>
        public async Task<JObject> getLeverage(string currency)
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}{currency}/leverage";
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 全仓设定合约币种杠杆倍数
        /// </summary>
        /// <param name="currency">币种，如：btc</param>
        /// <param name="leverage">要设定的杠杆倍数，10或20</param>
        /// <returns></returns>
        public async Task<JObject> setCrossedLeverage(string currency, int leverage)
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/{currency}/leverage";
            var body = new { leverage = leverage };
            var bodyStr = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, bodyStr)))
            {
                var res = await client.PostAsync(url, new StringContent(bodyStr, Encoding.UTF8, "application/json"));
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }

        /// <summary>
        /// 逐仓设定合约币种杠杆倍数
        /// </summary>
        /// <param name="currency">币种，如：btc</param>
        /// <param name="leverage">要设定的杠杆倍数，10或20</param>
        /// <param name="instrument_id">合约ID，如BTC-USD-180213</param>
        /// <param name="direction">开仓方向，long(做多)或者short(做空)</param>
        /// <returns></returns>
        public async Task<JObject> setFixedLeverage(string currency, int leverage, string instrument_id, string direction)
        {
            var url = $"{this.BASEURL}{this.FUTURES_SEGMENT}/{currency}/leverage";
            var body = new { instrument_id = instrument_id, direction = direction, leverage = leverage };
            var bodyStr = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, bodyStr)))
            {
                var res = await client.PostAsync(url, new StringContent(bodyStr, Encoding.UTF8, "application/json"));
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }
    }
}
