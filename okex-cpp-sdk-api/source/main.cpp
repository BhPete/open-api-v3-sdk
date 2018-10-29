#include <iostream>
#include "okapi.h"
#include <cpprest/http_client.h>

string instrument_id = "BCH-USD-181228";
string order_id = "1641326222656512";
string currency  = "bch";

using namespace web::http;
using namespace web::http::client;
using namespace web;

int main(int argc, char *args[])
{
    OKAPI okapi;
    /************************** set config **********************/
    struct Config config;
    config.SecretKey = "";
    config.ApiKey = "";
    config.Endpoint = "https://www.okex.com";
    config.I18n = "en_US";
    config.IsPrint = true;
    config.Passphrase = "";

    okapi.SetConfig(config);
    /************************** test examples **********************/
    okapi.GetServerTime();
    okapi.GetCurrencies();
    okapi.GetWalletCurrency(currency);
    okapi.GetWithdrawFee();

    /************************** futures test examples **********************/
    value obj;
    okapi.GetFuturesPositions();
    okapi.GetFuturesProductPosition(instrument_id);
    okapi.GetFuturesAccountsByCurrency(currency);
    okapi.GetFuturesLeverageByCurrency(currency);
    obj["instrument_id"] = value::string(instrument_id);
    obj["direction"] = value::string("long");
    obj["leverage"] = value::string("20");
    okapi.SetFuturesLeverageByCurrency(currency, obj);
    okapi.GetFuturesAccountsLedgerByCurrency(currency);
    
    obj["instrument_id"] = value::string(instrument_id);
    obj["type"] = value::number(2);
    obj["price"] = value::number(10000.1);
    obj["size"] = value::number(1);
    obj["margin_price"] = value::number(0);
    obj["leverage"] = value::number(10);
    okapi.FuturesOrder(obj);
    okapi.CancelFuturesProductOrder(instrument_id, order_id);

    okapi.GetFuturesOrders("2", instrument_id);
    okapi.GetFuturesOrderList(instrument_id, order_id);
    okapi.GetFuturesFills(instrument_id, order_id);
    okapi.GetFuturesProducts();
    okapi.GetFuturesProductBook(instrument_id, 50);
    okapi.GetFuturesTicker();
    okapi.GetFuturesProductTicker(instrument_id);
    okapi.GetFuturesProductTrades(instrument_id);
    okapi.GetFuturesProductCandles(instrument_id);
    okapi.GetFuturesIndex(instrument_id);
    okapi.GetFuturesRate();
    okapi.GetFuturesProductEstimatedPrice(instrument_id);
    okapi.GetFuturesProductOpenInterest(instrument_id);
    okapi.GetFuturesProductPriceLimit(instrument_id);
    okapi.GetFuturesProductLiquidation(instrument_id,  0);
    okapi.GetFuturesProductHolds(instrument_id);
    return 0;
}