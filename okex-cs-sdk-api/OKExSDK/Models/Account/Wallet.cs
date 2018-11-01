using System;
using System.Collections.Generic;
using System.Text;

namespace OKExSDK.Models.Account
{
    public class Wallet
    {
        public string Currency { get; set; }// 币种，如btc
        public decimal Balance { get; set; }// 余额
        public decimal Hold { get; set; }// 冻结(不可用)
        public decimal Available { get; set; }// 可用于提现或资金划转的数量
    }
}
