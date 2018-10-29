//
// Created by zxp on 13/07/18.
//

#ifndef CPPSDK_OKAPI_H
#define CPPSDK_OKAPI_H
#include <iostream>
#include <string>
#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include "utils.h"
#include "constants.h"

using namespace std;
using namespace web::http;
using namespace web::http::client;
using namespace web;
using namespace web::json;

struct Config{
    // Rest api endpoint url. eg: http://www.okex.com/
    string Endpoint;
    // The user's api key provided by OKEx.
    string ApiKey;
    // The user's secret key provided by OKEx. The secret key used to sign your request data.
    string SecretKey;
    // The Passphrase will be provided by you to further secure your API access.
    string Passphrase;
    // Http request timeout.
    int TimeoutSecond;
    // Whether to print API information
    bool IsPrint;
    // Internationalization @see file: constants.go
    string  I18n;
};

struct ServerTime {
    string Iso;
    string Epoch;
};

class OKAPI {
public:
    OKAPI() {};
    OKAPI(struct Config &cf) {SetConfig(cf);};
    ~OKAPI() {};
    void SetConfig(struct Config &cf);
    string Request(const string &method, const string &requestPath, const string &params="");
    string GetSign(string timestamp, string method, string requestPath, string body);

    void TestRequest();
    /************************** REST API ***************************/
    string GetServerTime();
    string GetExchangeRate();

    /************************** Futures API ***************************/
    string GetFuturesPositions();
    string GetFuturesProductPosition(string instrument_id);
    string GetFuturesAccountsByCurrency(string currency);
    string GetFuturesLeverageByCurrency(string currency);
    string SetFuturesLeverageByCurrency(string currency, value &obj);
    string GetFuturesAccountsLedgerByCurrency(string currency);

    string FuturesOrder(value &obj);
    string FuturesOrders(value &obj);
    string CancelFuturesProductOrder(string instrument_id, string order_id);
    string CancelFuturesProductOrders(string instrument_id);

    string GetFuturesOrders(string status, string instrument_id, string from="", string to="", string limit="");
    string GetFuturesOrderList(string instrument_id, string order_id);
    string GetFuturesFills(string instrument_id, string order_id, string from="", string to="", string limit="");
    string GetFuturesProducts();
    string GetFuturesProductBook(string &instrument_id, int book);
    string GetFuturesTicker();
    string GetFuturesProductTicker(string &instrument_id);
    string GetFuturesProductTrades(string &instrument_id);
    string GetFuturesProductCandles(string instrument_id, string start="", string end="", int granularity=604800);
    string GetFuturesIndex(string instrument_id);
    string GetFuturesRate();
    string GetFuturesProductEstimatedPrice(string instrument_id);
    string GetFuturesProductOpenInterest(string instrument_id);
    string GetFuturesProductPriceLimit(string instrument_id);
    string GetFuturesProductLiquidation(string instrument_id, int status);
    string GetFuturesProductHolds(string instrument_id);

    // 以下暂未在文档中描述
    string GetFuturesOrder(string order_id);
    string GetFuturesProductCurrencies();
    string GetFuturesProductIndex(string instrument_id);
    string GetFuturesAccounts();
    string GetFuturesAccountsHoldsByProductId(string instrument_id);
    string FuturesClosePositionParams(value &obj);
    string GetFuturesUsersSelfTrailingVolume();

    /************************** Account API ***************************/
    string GetCurrencies();
    string GetWallet();
    string GetWalletCurrency(string currency);
    string DoTransfer(value &jsonObj);
    string WithDrawals(value &jsonObj);
    string GetWithdrawFee();
    string GetWithdrawFee(string currency);
    string GetWithdrawHistory();
    string GetWithdrawHistoryByCurrency(string currency);
    string GetLedger(string type, string currency, string from, string to, string limit);
    string GetDepositAddress(string currency);
    string GetDepositHistory();
    string GetDepositHistoryByCurrency(string currency);

     /************************** Margin Account API ***************************/
    string GetAccounts();
    string GetAccountsByProductId(string instrument_id);
    string GetMarginLedger(string instrument_id, string beginDate, string endDate, string isHistory, string currencyId, string type, string from, string to, string limit);
    string GetMarginInfo();
    string GetBorrowAccounts(string status, string type, string from, string to, string limit);
    string GetMarginInfoByProductId(string instrument_id);
    string GetBorrowAccountsByProductId(string instrument_id, string status, string from, string to, string limit);
    string Borrow(value &jsonObj);
    string Repayment(value &jsonObj);

    /************************** Margin Order API ***************************/
    string AddOrder(value &jsonObj);
    string AddBatchOrder(value &jsonObj);
    string CancleOrdersByProductIdAndOrderId(string order_id, string instrument_id);
    string CancleOrdersByProductId(string instrument_id);
    string GetOrders(string instrument_id, string status, string from, string to, string limit);
    string GetOrderByProductIdAndOrderId(string order_id, string instrument_id);
    string GetFills(string order_id, string instrument_id,  string from, string to, string limit);

    /************************** Spot Account API ***************************/
    string GetSpotAccounts();
    string GetSpotAccountByCurrency(string currency);
    string GetSpotTime();
    string GetSpotLedgersByCurrency(string currency, string from, string to, string limit);

    /************************** Spot Order API ***************************/
    string AddSpotOrder(value &jsonObj);
    string AddSpotBatchOrder(value &jsonObj);
    string CancleSpotOrdersByProductIdAndOrderId(string order_id, value &jsonObj);
    string CancleSpotBatchOrders(string order_ids, value &jsonObj);
    string GetSpotOrders(string instrument_id, string status, string from, string to, string limit);
    string GetSpotOrdersPending(string from, string to, string limit);
    string GetSpotOrderByProductIdAndOrderId(string order_id, string instrument_id);
    string GetSpotFills(string order_id, string instrument_id, string from, string to, string limit);

    /************************** Spot Product API ***************************/
    string GetProducts();
    string GetProductsByProductId(string instrument_id, string size, string depth);
    string GetTickers();
    string GetTickerByProductId(string instrument_id);
    string GetTrades(string instrument_id, string from, string to, string limit);
    string GetCandles(string instrument_id, string granularity, string start, string end);

    /************************** Ett API ***************************/
    string GetEttAccounts();
    string GetEttAccountByCurrency(string currency);
    string GetEttLedgersByCurrency(string currency, string from, string to, string limit);
    string AddEttOrder(value &jsonObj);
    string CancleEttOrderByOrderId(string order_id);
    string GetEttOrders(string ett, string type, string status, string from, string to, string limit);
    string GetEttOrderByOrderId(string order_id);
    string GetEttConstituents(string ett);
    string GetEttDefinePrice(string ett);

private:
    struct Config m_config;
};


#endif //CPPSDK_OKAPI_H
