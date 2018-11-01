using Newtonsoft.Json;
using OKExSDK.Models.Account;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OKExSDK
{
    public class AccountApi : SdkApi
    {
        private string ACCOUNT_SEGMENT = "api/account/v3";
        public AccountApi(string apiKey, string secret, string passPhrase) : base(apiKey, secret, passPhrase) { }

        /// <summary>
        /// 获取币种列表
        /// </summary>
        /// <returns>币种列表</returns>
        public async Task<List<CoinCurrency>> getCurrenciesAsync()
        {
            var url = $"{this.BASEURL}{this.ACCOUNT_SEGMENT}/currencies";

            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);

                var contentStr = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CoinCurrency>>(contentStr);
            }
        }

        /// <summary>
        /// 钱包账户信息
        /// </summary>
        /// <returns>钱包列表</returns>
        public async Task<List<Wallet>> getWalletInfoAsync()
        {
            var url = $"{this.BASEURL}{this.ACCOUNT_SEGMENT}/wallet";
            using (var client = new HttpClient(new HttpInterceptor(this._apiKey, this._secret, this._passPhrase, null)))
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Wallet>>(contentStr);
            }
        }


    }
}
