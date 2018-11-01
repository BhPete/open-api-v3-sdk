using System;
using System.Collections.Generic;
using System.Text;

namespace OKExSDK.Models.Futures
{
    public class AccountFixed : Account
    {
        /// <summary>
        /// 逐仓账户余额
        /// </summary>
        public decimal fixed_balance { get; set; }
        /// <summary>
        /// 逐仓可用余额
        /// </summary>
        public decimal available_qty { get; set; }
        /// <summary>
        /// 冻结的保证金(成交以后仓位所需的)
        /// </summary>
        public decimal margin_frozen { get; set; }
        /// <summary>
        /// 挂单冻结保证金
        /// </summary>
        public decimal margin_for_unfilled { get; set; }
    }
}
