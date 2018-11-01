using Newtonsoft.Json;
using OKExSDK;
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

        private async void btnGetCurrencies(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await this.accountApi.getCurrenciesAsync();
                MessageBox.Show(JsonConvert.SerializeObject(result));
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
                var result = await this.accountApi.getWalletInfoAsync();
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
                var resResult = await this.futureApi.getPositions();
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
                else
                {
                    MessageBox.Show("错误代码：" + (string)resResult["code"] + ",错误消息：" + (string)resResult["message"]);
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
                var resResult = await this.futureApi.getPositions();
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
                else
                {
                    MessageBox.Show("错误代码：" + (string)resResult["code"] + ",错误消息：" + (string)resResult["message"]);
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
                var result = await this.futureApi.getAccounts();
                MessageBox.Show(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
