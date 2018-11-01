using System;
using System.Collections.Generic;
using System.Text;

namespace OKExSDK.Models.Futures
{
    public class AccountCrossed : Account
    {
        /// <summary>
        /// 已用保证金
        /// </summary>
        public decimal margin { get; set; }
        /// <summary>
        /// 保证金率
        /// </summary>
        public double margin_ratio { get; set; }
    }
}
