## OKCoin OKEX V3 Open Api使用说明

### 1.基于.NET Standard，支持上传至NuGet

###  2.依赖Json.NET序列化/反序列化

### 3.简单使用

```c#
private async void btnGetPositions(object sender, RoutedEventArgs e)
{
    //创建合约API对象，传入Api Key，Secret Key, PassPhrase
    var futureApi = new FuturesApi(this.apiKey, this.secret, this.passPhrase);
    try
    {
        //调用获取合约账户所有的持仓信息方法
        var resResult = await futureApi.getPositions();
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
```



