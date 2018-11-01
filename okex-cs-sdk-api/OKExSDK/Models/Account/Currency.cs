using System;
using System.Collections.Generic;
using System.Text;

namespace OKExSDK.Models.Account
{
    public class CoinCurrency
    {
        public string Currency { get; set; }// 币种名称，如btc
        public string Name { get; set; }// 币种中文名称，不显示则无对应名称
        public int Can_deposit { get; set; }// 是否可充值，0表示不可充值，1表示可以充值
        public int Can_withdraw { get; set; }// 是否可提币，0表示不可提币，1表示可以提币
        public decimal Min_withdrawal { get; set; }// 币种最小提币量
    }
}
