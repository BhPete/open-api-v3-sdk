using OKExSDK.Models.Account;
using OKExSDK.Models.Futures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCS
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.ordertypes = new Dictionary<string, string>();
            this.ordertypes.Add("开多", "1");
            this.ordertypes.Add("开空", "2");
            this.ordertypes.Add("平多", "3");
            this.ordertypes.Add("平空", "4");
            this.transferTypes.Add("子账户", "0");
            this.transferTypes.Add("币币", "1");
            this.transferTypes.Add("合约", "3");
            this.transferTypes.Add("C2C", "4");
            this.transferTypes.Add("币币杠杆", "5");
            this.transferTypes.Add("钱包", "6");
            this.transferTypes.Add("ETT", "7");

            this.destinationTypes.Add("OKCoin国际", "2");
            this.destinationTypes.Add("OKEx", "3");
            this.destinationTypes.Add("数字货币地址", "4");

        }

        public Dictionary<string, string> transferTypes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> destinationTypes { get; set; } = new Dictionary<string, string>();
        public Transfer Transfer { get; set; } = new Transfer();
        public WithDrawal WithDrawal { get; set; } = new WithDrawal();
        public KeyInfo KeyInfo { get; set; }
        private OrderSingle orderSingle = new OrderSingle();

        public OrderSingle OrderSingle
        {
            get { return orderSingle; }
            set
            {
                if (OrderSingle != value)
                {
                    OrderSingle = value;
                    RaisePropertyChanged("OrderSingle");
                }
            }
        }
        private List<OrderBatchDetail> orderdetails = new List<OrderBatchDetail>() {
            new OrderBatchDetail()
        };

        public List<OrderBatchDetail> OrderDetails
        {
            get { return orderdetails; }
            set { orderdetails = value; }
        }



        private OrderBatch orderBatch = new OrderBatch();

        public OrderBatch OrderBatch
        {
            get { return orderBatch; }
            set { orderBatch = value; }
        }

        private Dictionary<string, string> ordertypes;

        public Dictionary<string, string> OrderTypes
        {
            get { return ordertypes; }
            set { ordertypes = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
