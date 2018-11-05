using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OKExSDK;
using OKExSDK.Models;
using OKExSDK.Models.Account;
using OKExSDK.Models.Futures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SampleCS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneralApi generalApi;
        private AccountApi accountApi;
        private FuturesApi futureApi;
        private string apiKey = "";
        private string secret = "";
        private string passPhrase = "";
        public MainWindow()
        {
            InitializeComponent();
            this.generalApi = new GeneralApi(this.apiKey, this.secret, this.passPhrase);
            this.futureApi = new FuturesApi(this.apiKey, this.secret, this.passPhrase);
            this.accountApi = new AccountApi(this.apiKey, this.secret, this.passPhrase);


            this.DataContext = new MainViewModel();
        }

        private async void btnSyncServerTimeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await this.generalApi.syncTimeAsync();
                MessageBox.Show(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetPositions(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getPositionsAsync();
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var result = (bool)resResult["result"];
                    if (result)
                    {
                        var margin_mode = (string)resResult["margin_mode"];
                        // 全仓
                        if (margin_mode == "crossed")
                        {
                            var obj = resResult.ToObject<PositionResult<PositionCrossed>>();
                            MessageBox.Show(JsonConvert.SerializeObject(resResult));
                        }
                        else if (margin_mode == "fixed")
                        {
                            // 逐仓
                            var obj = resResult.ToObject<PositionResult<PositionFixed>>();
                            MessageBox.Show(JsonConvert.SerializeObject(resResult));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetPositionByInstrumentId(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getPositionByIdAsync("EOS-USD-181228");
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var result = (bool)resResult["result"];
                    if (result)
                    {
                        var margin_mode = (string)resResult["margin_mode"];
                        // 全仓
                        if (margin_mode == "crossed")
                        {
                            var obj = resResult.ToObject<PositionResult<PositionCrossed>>();
                            MessageBox.Show(JsonConvert.SerializeObject(resResult));
                        }
                        else if (margin_mode == "fixed")
                        {
                            // 逐仓
                            var obj = resResult.ToObject<PositionResult<PositionFixed>>();
                            MessageBox.Show(JsonConvert.SerializeObject(resResult));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetFuturesAccounts(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getAccountsAsync();
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    //{"result":true,"info":{"btc":{"contracts":[{"available_qty":"0.004","fixed_balance":"0","instrument_id":"BTC-USD-181228","margin_for_unfilled":"0.0000001","margin_frozen":"0","realized_pnl":"0","unrealized_pnl":"0"}],"equity":"0.004","margin_mode":"fixed","total_avail_balance":"0.004"},"btg":{"equity":"0.427","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"0.427","unrealized_pnl":"0"},"etc":{"equity":"0.404","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"0.404","unrealized_pnl":"0"},"bch":{"equity":"0.0000004","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"0.0000004","unrealized_pnl":"0"},"xrp":{"equity":"8.743","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"8.743","unrealized_pnl":"0"},"eth":{"equity":"0.012","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"0.012","unrealized_pnl":"0"},"eos":{"equity":"1.224","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"1.224","unrealized_pnl":"0"},"ltc":{"equity":"0.024","margin":"0","margin_mode":"crossed","margin_ratio":"10000","realized_pnl":"0","total_avail_balance":"0.024","unrealized_pnl":"0"}}}
                    var result = (bool)resResult["result"];
                    if (result)
                    {
                        var info = resResult["info"];
                        foreach (var item in info.Children())
                        {
                            if (item.Children().Count() > 0)
                            {
                                var account = item.Children().First();
                                var margin_mode = (string)account["margin_mode"];
                                if (margin_mode == "crossed")
                                {
                                    //全仓
                                    var accountInfo = account.ToObject<AccountCrossed>();
                                    MessageBox.Show(((JProperty)item).Name + "：" + JsonConvert.SerializeObject(accountInfo));
                                }
                                else
                                {
                                    //逐仓
                                    var accountInfo = account.ToObject<AccountFixed>();
                                    MessageBox.Show(((JProperty)item).Name + "：" + JsonConvert.SerializeObject(accountInfo));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetOneCurencyAccounts(object sender, RoutedEventArgs e)
        {
            try
            {
                var currency = "eos";
                var resResult = await this.futureApi.getAccountByCurrencyAsync(currency);
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    //{"equity":"1.224","margin":"0","realized_pnl":"0","unrealized_pnl":"0","margin_ratio":"10000","margin_mode":"crossed","total_avail_balance":"1.224"}
                    var margin_mode = (string)resResult["margin_mode"];
                    if (margin_mode == "crossed")
                    {
                        //全仓
                        var accountInfo = resResult.ToObject<AccountCrossed>();
                        MessageBox.Show(currency + "：" + JsonConvert.SerializeObject(accountInfo));
                    }
                    else
                    {
                        //逐仓
                        var accountInfo = resResult.ToObject<AccountFixed>();
                        MessageBox.Show(currency + "：" + JsonConvert.SerializeObject(accountInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetLeverage(object sender, RoutedEventArgs e)
        {
            try
            {
                //{"leverage":20,"margin_mode":"crossed","currency":"EOS"}
                var resResult = await this.futureApi.getLeverageAsync("eos");
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var margin_mode = (string)resResult["margin_mode"];
                    //全仓
                    if (margin_mode == "crossed")
                    {
                        var leverage = resResult.ToObject<LeverageCrossed>();
                        MessageBox.Show(JsonConvert.SerializeObject(leverage));
                    }
                    else
                    {
                        //逐仓
                        var leverage = resResult.ToObject<LeverageFixed>();
                        MessageBox.Show(JsonConvert.SerializeObject(leverage));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnSetCrossedLeverage(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.setCrossedLeverageAsync("eos", 20);
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var result = resResult.ToObject<SetCrossedLeverageResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnSetFixedLeverage(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.setFixedLeverageAsync(this.currency.Text, int.Parse(this.leverage.Text), this.instrument_id.Text, this.direction.Text);
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var result = resResult.ToObject<SetCrossedLeverageResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetLedger(object sender, RoutedEventArgs e)
        {
            try
            {
                //[{"ledger_id":"1730792498235392","timestamp":"2018-11-02T08:03:17.0Z","amount":"-0.019","balance":"0","currency":"EOS","type":"match","details":{"order_id":0,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1730166161965057","timestamp":"2018-11-02T05:24:00.0Z","amount":"-0.007","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1730166159284224,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1730166161965056","timestamp":"2018-11-02T05:24:00.0Z","amount":"-0.005","balance":"-13","currency":"EOS","type":"match","details":{"order_id":1730166159284224,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1730165947662337","timestamp":"2018-11-02T05:23:57.0Z","amount":"-0.007","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1730165941482496,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1730165947662336","timestamp":"2018-11-02T05:23:57.0Z","amount":"0","balance":"13","currency":"EOS","type":"match","details":{"order_id":1730165941482496,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1691155227772928","timestamp":"2018-10-26T08:03:00.0Z","amount":"0.00052289","balance":"0","currency":"EOS","type":"match","details":{"order_id":0,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1689818088308738","timestamp":"2018-10-26T02:22:57.0Z","amount":"-0.00055597","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1689818080547840,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1689818088308737","timestamp":"2018-10-26T02:22:57.0Z","amount":"0.004","balance":"-1","currency":"EOS","type":"match","details":{"order_id":1689818080547840,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684987004485635","timestamp":"2018-10-25T05:54:21.0Z","amount":"-0.0005571","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1684986992538624,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684987004485634","timestamp":"2018-10-25T05:54:21.0Z","amount":"0","balance":"1","currency":"EOS","type":"match","details":{"order_id":1684986992538624,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684986777075713","timestamp":"2018-10-25T05:54:17.0Z","amount":"-0.0005571","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1684986768278528,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684986777075712","timestamp":"2018-10-25T05:54:17.0Z","amount":"-0.001","balance":"-1","currency":"EOS","type":"match","details":{"order_id":1684986768278528,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684129732001795","timestamp":"2018-10-25T02:16:20.0Z","amount":"-0.00055741","balance":"0","currency":"EOS","type":"fee","details":{"order_id":1684129718574080,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1684129732001794","timestamp":"2018-10-25T02:16:20.0Z","amount":"0","balance":"1","currency":"EOS","type":"match","details":{"order_id":1684129718574080,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1651521273235456","timestamp":"2018-10-19T08:03:34.0Z","amount":"-0.2","balance":"0","currency":"EOS","type":"match","details":{"order_id":0,"instrument_id":"EOS-USD-181228"}},{"ledger_id":"1651521271793668","timestamp":"2018-10-19T08:03:34.0Z","amount":"0","balance":"0","currency":"EOS","type":"match","details":{"order_id":0,"instrument_id":"EOS-USD-181019"}}]
                var resResult = await this.futureApi.getLedgersByCurrencyAsync("eos", 1, null, 10);
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var ledgers = resResult.ToObject<List<Ledger>>();
                    MessageBox.Show(JsonConvert.SerializeObject(ledgers));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Dictionary<string, string> orderTypes { get; set; } = new Dictionary<string, string>();

        private async void btnMakeOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = ((MainViewModel)this.DataContext).OrderSingle;
                var resResult = await this.futureApi.makeOrderAsync(order.instrument_id,
                    order.type,
                    order.price,
                    order.size,
                    order.leverage,
                    order.client_oid,
                    order.match_price == "True" ? "1" : "0");
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var orderResult = resResult.ToObject<OrderResultSingle>();
                    MessageBox.Show(JsonConvert.SerializeObject(orderResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnMakeOrderBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = ((MainViewModel)this.DataContext).OrderBatch;
                var orderDetails = ((MainViewModel)this.DataContext).OrderDetails;
                orderDetails.ForEach(od =>
                {
                    od.match_price = od.match_price == "True" ? "1" : "0";
                });
                order.orders_data = JsonConvert.SerializeObject(orderDetails);
                var resResult = await this.futureApi.makeOrdersBatchAsync(order);
                JToken codeJToken;
                if (resResult.TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var orderResult = resResult.ToObject<OrderBatchResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(orderResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnCancelOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.cancelOrderAsync(this.instrument_id_cancel.Text, this.order_id.Text);

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var cancelResult = resResult.ToObject<CancelOrderResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(cancelResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnCancelOrderBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                var orderIds = new List<long>();
                orderIds.Add(long.Parse(this.order_id.Text));
                var resResult = await this.futureApi.cancelOrderBatchAsync(this.instrument_id_cancel.Text, orderIds);

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var cancelResult = resResult.ToObject<CancelOrderBatchResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(cancelResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetOrders(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getOrdersAsync("BTC-USD-181109", "2", 1, null, 10);

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var ledgers = resResult.ToObject<OrderListResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(ledgers));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetOrderById(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getOrderByIdAsync("BTC-USD-181109", "asdasd");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var ledgers = resResult.ToObject<Order>();
                    MessageBox.Show(JsonConvert.SerializeObject(ledgers));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetFills(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getFillsAsync("BTC-USD-181109", "asdasd", 1, null, 10);
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var fills = resResult.ToObject<List<Fill>>();
                    MessageBox.Show(JsonConvert.SerializeObject(fills));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetInstruments(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getInstrumentsAsync();
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var instruments = resResult.ToObject<List<Instrument>>();
                    MessageBox.Show(JsonConvert.SerializeObject(instruments));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetBook(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getBookAsync("BTC-USD-181109", 20);
                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var book = resResult.ToObject<Book>();
                    MessageBox.Show(JsonConvert.SerializeObject(book));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetTickers(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getTickersAsync();
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var ticker = resResult.ToObject<List<Ticker>>();
                    MessageBox.Show(JsonConvert.SerializeObject(ticker));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetTickerById(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getTickerByInstrumentId("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var ticker = resResult.ToObject<Ticker>();
                    MessageBox.Show(JsonConvert.SerializeObject(ticker));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetTrades(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getTradesAsync("BTC-USD-181109", 1, null, 10);
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var trades = resResult.ToObject<List<Trade>>();
                    MessageBox.Show(JsonConvert.SerializeObject(trades));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetKData(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getCandlesDataAsync("BTC-USD-181109", DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, 60);
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var candles = resResult.ToObject<List<List<decimal>>>();
                    MessageBox.Show(JsonConvert.SerializeObject(candles));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetIndex(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getIndexAsync("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var index = resResult.ToObject<Index>();
                    MessageBox.Show(JsonConvert.SerializeObject(index));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetRate(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getRateAsync();

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var rate = resResult.ToObject<Rate>();
                    MessageBox.Show(JsonConvert.SerializeObject(rate));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetEstimatedPrice(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getEstimatedPriceAsync("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var estimatedPrice = resResult.ToObject<EstimatedPrice>();
                    MessageBox.Show(JsonConvert.SerializeObject(estimatedPrice));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetOpenInteest(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getOpenInterestAsync("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var openInterest = resResult.ToObject<OpenInterest>();
                    MessageBox.Show(JsonConvert.SerializeObject(openInterest));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnPriceLimit(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getPriceLimitAsync("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var priceLimit = resResult.ToObject<PriceLimit>();
                    MessageBox.Show(JsonConvert.SerializeObject(priceLimit));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnLiquidation(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getLiquidationAsync("BTC-USD-181109", "0", 1, null, 10);
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var liquidations = resResult.ToObject<List<Liquidation>>();
                    MessageBox.Show(JsonConvert.SerializeObject(liquidations));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnHolds(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.futureApi.getHoldsAsync("BTC-USD-181109");

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var holds = resResult.ToObject<Hold>();
                    MessageBox.Show(JsonConvert.SerializeObject(holds));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetCurrencies(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getCurrenciesAsync();
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var currencies = resResult.ToObject<List<Currency>>();
                    MessageBox.Show(JsonConvert.SerializeObject(currencies));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetWallet(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getWalletInfoAsync();
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var walletInfo = resResult.ToObject<List<Wallet>>();
                    MessageBox.Show(JsonConvert.SerializeObject(walletInfo));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetWalletByCurrency(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getWalletInfoByCurrencyAsync("eos");
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var walletInfo = resResult.ToObject<List<Wallet>>();
                    MessageBox.Show(JsonConvert.SerializeObject(walletInfo));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnMakeTransfer(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.makeTransferAsync(((MainViewModel)this.DataContext).Transfer);

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var transferResult = resResult.ToObject<TransferResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(transferResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnMakeWithDrawal(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.makeWithDrawalAsync(((MainViewModel)this.DataContext).WithDrawal);

                JToken codeJToken;
                if (((JObject)resResult).TryGetValue("code", out codeJToken))
                {
                    var errorInfo = resResult.ToObject<ErrorResult>();
                    MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                }
                else
                {
                    var withdrawalResult = resResult.ToObject<WithDrawalResult>();
                    MessageBox.Show(JsonConvert.SerializeObject(withdrawalResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetWithdrawalFee(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getWithDrawalFeeAsync("eos");
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var fee = resResult.ToObject<List<WithdrawalFee>>();
                    MessageBox.Show(JsonConvert.SerializeObject(fee));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetWithdrawalHistory(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getWithDrawalHistoryAsync();
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var history = resResult.ToObject<List<WithDrawalHistory>>();
                    MessageBox.Show(JsonConvert.SerializeObject(history));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetWithdrawalHistoryByCurrency(object sender, RoutedEventArgs e)
        {
            try
            {
                var resResult = await this.accountApi.getWithDrawalHistoryByCurrencyAsync("eos");
                if (resResult.Type == JTokenType.Object)
                {
                    JToken codeJToken;
                    if (((JObject)resResult).TryGetValue("code", out codeJToken))
                    {
                        var errorInfo = resResult.ToObject<ErrorResult>();
                        MessageBox.Show("错误代码：" + errorInfo.code + ",错误消息：" + errorInfo.message);
                    }
                }
                else
                {
                    var history = resResult.ToObject<List<WithDrawalHistory>>();
                    MessageBox.Show(JsonConvert.SerializeObject(history));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
