package okex

import (
	"net/http"
	"strings"
)

/*
 OKEX futures contract api
 @author Tony Tian
 @date 2018-03-17
 @version 1.0.0
*/

const (
	CrossPositionFlag = "force_liqu_price"
	FixedAccountFlag  = "hold"
	FuturesPathPrefix = "/api/futures/v3/"
)

/*
 =============================== Futures market api ===============================
*/
/*
 The exchange rate of legal tender pairs
*/
func (client *Client) GetFuturesExchangeRate() (ExchangeRate, error) {
	var exchangeRate ExchangeRate
	_, err := client.Request(GET, FUTURES_RATE, nil, &exchangeRate)
	return exchangeRate, err
}

/*
  Get all of futures contract list
*/
func (client *Client) GetFuturesInstruments() ([]FuturesInstrumentsResult, error) {
	var Instruments []FuturesInstrumentsResult
	_, err := client.Request(GET, FUTURES_INSTRUMENTS, nil, &Instruments)
	return Instruments, err
}

/*
 Get the futures contract currencies
*/
func (client *Client) GetFuturesInstrumentCurrencies() ([]FuturesInstrumentCurrenciesResult, error) {
	var currencies []FuturesInstrumentCurrenciesResult
	_, err := client.Request(GET, FUTURES_CURRENCIES, nil, &currencies)
	return currencies, err
}

/*
 Get the futures contract Instrument book
 depth value：1-200
 merge value：1(merge depth)
*/
func (client *Client) GetFuturesInstrumentBook(InstrumentId string, size int) (FuturesInstrumentBookResult, error) {
	var book FuturesInstrumentBookResult
	params := NewParams()
	params["size"] = Int2String(size)
	requestPath := BuildParams(GetInstrumentIdUri(FUTURES_INSTRUMENT_BOOK, InstrumentId), params)
	_, err := client.Request(GET, requestPath, nil, &book)
	return book, err
}

/*
 Get the futures contract Instrument all ticker
*/
func (client *Client) GetFuturesInstrumentAllTicker() ([]FuturesInstrumentTickerResult, error) {
	var tickers []FuturesInstrumentTickerResult
	_, err := client.Request(GET, FUTURES_TICKERS, nil, &tickers)
	return tickers, err
}

/*
 Get the futures contract Instrument ticker
*/
func (client *Client) GetFuturesInstrumentTicker(InstrumentId string) (FuturesInstrumentTickerResult, error) {
	var ticker FuturesInstrumentTickerResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_TICKER, InstrumentId), nil, &ticker)
	return ticker, err
}

/*
 Get the futures contract Instrument trades
*/
func (client *Client) GetFuturesInstrumentTrades(InstrumentId string) ([]FuturesInstrumentTradesResult, error) {
	var trades []FuturesInstrumentTradesResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_TRADES, InstrumentId), nil, &trades)
	return trades, err
}

/*
 Get the futures contract Instrument candles
 granularity: @see  file: futures_constants.go
*/
func (client *Client) GetFuturesInstrumentCandles(InstrumentId, start, end string, granularity, count int) ([][]float64, error) {
	var candles [][]float64
	params := NewParams()
	params["start"] = start
	params["end"] = end
	params["granularity"] = Int2String(granularity)
	params["count"] = Int2String(count)
	requestPath := BuildParams(GetInstrumentIdUri(FUTURES_INSTRUMENT_CANDLES, InstrumentId), params)
	_, err := client.Request(GET, requestPath, nil, &candles)
	return candles, err
}

/*
 Get the futures contract Instrument index
*/
func (client *Client) GetFuturesInstrumentIndex(InstrumentId string) (FuturesInstrumentIndexResult, error) {
	var index FuturesInstrumentIndexResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_INDEX, InstrumentId), nil, &index)
	return index, err
}

/*
 Get the futures contract Instrument estimated price
*/
func (client *Client) GetFuturesInstrumentEstimatedPrice(InstrumentId string) (FuturesInstrumentEstimatedPriceResult, error) {
	var estimatedPrice FuturesInstrumentEstimatedPriceResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_ESTIMATED_PRICE, InstrumentId), nil, &estimatedPrice)
	return estimatedPrice, err
}

/*
 Get the futures contract Instrument holds
*/
func (client *Client) GetFuturesInstrumentOpenInterest(InstrumentId string) (FuturesInstrumentOpenInterestResult, error) {
	var openInterest FuturesInstrumentOpenInterestResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_OPEN_INTEREST, InstrumentId), nil, &openInterest)
	return openInterest, err
}

/*
 Get the futures contract Instrument limit price
*/
func (client *Client) GetFuturesInstrumentPriceLimit(InstrumentId string) (FuturesInstrumentPriceLimitResult, error) {
	var priceLimit FuturesInstrumentPriceLimitResult
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_PRICE_LIMIT, InstrumentId), nil, &priceLimit)
	return priceLimit, err
}

/*
 Get the futures contract liquidation
*/
func (client *Client) GetFuturesInstrumentLiquidation(InstrumentId string, status, from, to, limit int) (FuturesInstrumentLiquidationListResult, error) {
	var liquidation []FuturesInstrumentLiquidationResult
	params := NewParams()
	params["status"] = Int2String(status)
	params["from"] = Int2String(from)
	params["to"] = Int2String(to)
	params["limit"] = Int2String(limit)
	requestPath := BuildParams(GetInstrumentIdUri(FUTURES_INSTRUMENT_LIQUIDATION, InstrumentId), params)
	response, err := client.Request(GET, requestPath, nil, &liquidation)
	var list FuturesInstrumentLiquidationListResult
	page := parsePage(response)
	list.Page = page
	list.LiquidationList = liquidation
	return list, err
}

/*
 =============================== Futures trade api ===============================
*/

/*
 Get all of futures contract position list.
 return struct: FuturesPositions
*/
func (client *Client) GetFuturesPositions() (FuturesPosition, error) {
	response, err := client.Request(GET, FUTURES_POSITION, nil, nil)
	return parsePositions(response, err)
}

/*
 Get all of futures contract position list.
 return struct: FuturesPositions
*/
func (client *Client) GetFuturesInstrumentPosition(InstrumentId string) (FuturesPosition, error) {
	response, err := client.Request(GET, GetInstrumentIdUri(FUTURES_INSTRUMENT_POSITION, InstrumentId), nil, nil)
	return parsePositions(response, err)
}

/*
 Get all of futures contract account list
 return struct: FuturesAccounts
*/
func (client *Client) GetFuturesAccounts() (FuturesAccount, error) {
	response, err := client.Request(GET, FUTURES_ACCOUNTS, nil, nil)
	return parseAccounts(response, err)
}

/*
 Get the futures contract currency account @see file : futures_constants.go
 return struct: FuturesCurrencyAccounts
*/
func (client *Client) GetFuturesAccountsByCurrency(currency string) (FuturesCurrencyAccounts, error) {
	response, err := client.Request(GET, GetCurrencyUri(FUTURES_ACCOUNT_CURRENCY_INFO, currency), nil, nil)
	return parseCurrencyAccounts(response, err)
}

/*
 Get the futures contract currency ledger
*/
func (client *Client) GetFuturesAccountsLedgerByCurrency(currency string) (FuturesCurrencyLedger, error) {
	var ledger FuturesCurrencyLedger
	_, err := client.Request(GET, GetCurrencyUri(FUTURES_ACCOUNT_CURRENCY_LEDGER, currency), nil, &ledger)
	return ledger, err
}

/*
 Get the futures contract Instrument holds
*/
func (client *Client) GetFuturesAccountsHoldsByInstrumentId(InstrumentId string) (FuturesAccountsHolds, error) {
	var holds FuturesAccountsHolds
	_, err := client.Request(GET, GetInstrumentIdUri(FUTURES_ACCOUNT_INSTRUMENT_HOLDS, InstrumentId), nil, &holds)
	return holds, err
}

/*
 Create a new order
*/
func (client *Client) FuturesOrder(newOrderParams FuturesNewOrderParams) (FuturesNewOrderResult, error) {
	var newOrderResult FuturesNewOrderResult
	_, err := client.Request(POST, FuturesPathPrefix+"order", newOrderParams, &newOrderResult)
	return newOrderResult, err
}

/*
 Batch create new order.(Max of 5 orders are allowed per request)
*/
func (client *Client) FuturesOrders(batchNewOrder FuturesBatchNewOrderParams) (FuturesBatchNewOrderResult, error) {
	var batchNewOrderResult FuturesBatchNewOrderResult
	_, err := client.Request(POST, FuturesPathPrefix+"orders", batchNewOrder, &batchNewOrderResult)
	return batchNewOrderResult, err
}

/*
 Cancel the order
*/
func (client *Client) CancelFuturesInstrumentOrder(InstrumentId string, orderId int64) (FuturesCancelInstrumentOrderResult, error) {
	var cancelInstrumentOrderResult FuturesCancelInstrumentOrderResult
	_, err := client.Request(DELETE, FuturesPathPrefix+"orders/"+InstrumentId+"/"+Int64ToString(orderId), nil,
		&cancelInstrumentOrderResult)
	return cancelInstrumentOrderResult, err
}

/*
 Batch Cancel the orders
*/
func (client *Client) CancelFuturesInstrumentOrders(InstrumentId string) (FuturesCancelInstrumentOrdersResult, error) {
	var cancelInstrumentOrdersResult FuturesCancelInstrumentOrdersResult
	_, err := client.Request(DELETE, FuturesPathPrefix+"orders/"+InstrumentId, nil, &cancelInstrumentOrdersResult)
	return cancelInstrumentOrdersResult, err
}

/*
 close position
*/
func (client *Client) FuturesClosePosition(closePositionParams FuturesClosePositionParams) (FuturesClosePositionResult, error) {
	var closePositionResult FuturesClosePositionResult
	_, err := client.Request(DELETE, FuturesPathPrefix+"close_position", closePositionParams, nil)
	return closePositionResult, err
}

/*
 Get all of futures contract order list
*/
func (client *Client) GetFuturesOrders(getOrdersParams FuturesOrdersParams) (FuturesGetOrdersResult, error) {
	var getOrdersResult FuturesGetOrdersResult
	_, err := client.Request(GET, FuturesPathPrefix+"orders", getOrdersParams, &getOrdersResult)
	return getOrdersResult, err
}

/*
 Get all of futures contract a order by order id
*/
func (client *Client) GetFuturesOrder(orderId int64) (FuturesGetOrderResult, error) {
	var getOrderResult FuturesGetOrderResult
	_, err := client.Request(GET, FuturesPathPrefix+"orders/"+Int64ToString(orderId), nil, &getOrderResult)
	return getOrderResult, err
}

/*
 Get all of futures contract transactions.
*/
func (client *Client) GetFuturesFills(fillsParams FuturesFillsParams) (FuturesFillsResult, error) {
	var getFillsResult FuturesFillsResult
	_, err := client.Request(GET, FuturesPathPrefix+"fills", fillsParams, &getFillsResult)
	return getFillsResult, err
}

/*
 Get futures contract account volume
*/
func (client *Client) GetFuturesUsersSelfTrailingVolume() (FuturesUsersSelfTrailingVolumesResult, error) {
	var usersSelfTrailingVolumesResult FuturesUsersSelfTrailingVolumesResult
	_, err := client.Request(GET, FuturesPathPrefix+"users/self/trailing_volume", nil, &usersSelfTrailingVolumesResult)
	return usersSelfTrailingVolumesResult, err
}

func parsePositions(response *http.Response, err error) (FuturesPosition, error) {
	var position FuturesPosition
	if err != nil {
		return position, err
	}
	jsonString := GetResponseDataJsonString(response)
	if strings.Contains(jsonString, "\"margin_mode\":\"fixed\"") {
		var fixedPosition FuturesFixedPosition
		err = JsonString2Struct(jsonString, &fixedPosition)
		if err != nil {
			return position, err
		} else {
			position.Result = fixedPosition.Result
			position.MarginMode = fixedPosition.MarginMode
			position.FixedPosition = fixedPosition.FixedPosition
		}
	}
	if strings.Contains(jsonString, "\"margin_mode\":\"crossed\"") {
		var crossPosition FuturesCrossPosition
		err = JsonString2Struct(jsonString, &crossPosition)
		if err != nil {
			return position, err
		} else {
			position.Result = crossPosition.Result
			position.MarginMode = crossPosition.MarginMode
			position.CrossPosition = crossPosition.CrossPosition
		}
	}
	return position, nil
}

func parseAccounts(response *http.Response, err error) (FuturesAccount, error) {
	var account FuturesAccount
	if err != nil {
		return account, err
	}
	jsonString := GetResponseDataJsonString(response)
	if strings.Contains(jsonString, "\"contracts\"") {
		var fixedAccount FuturesFixedAccountInfo
		err = JsonString2Struct(jsonString, &fixedAccount)
		if err != nil {
			return account, err
		} else {
			account.Result = fixedAccount.Result
			account.FixedAccount = fixedAccount.Info
			account.MarginMode = "fixed"
		}
	} else {
		var crossAccount FuturesCrossAccountInfo
		err = JsonString2Struct(jsonString, &crossAccount)
		if err != nil {
			return account, err
		} else {
			account.Result = crossAccount.Result
			account.MarginMode = "crossed"
			account.CrossAccount = crossAccount.Info
		}
	}
	return account, nil
}

func parseCurrencyAccounts(response *http.Response, err error) (FuturesCurrencyAccounts, error) {
	var futuresCurrencyAccounts FuturesCurrencyAccounts
	if err != nil {
		return futuresCurrencyAccounts, err
	}
	jsonString := GetResponseDataJsonString(response)
	if strings.Contains(jsonString, FixedAccountFlag) {
		var fixedAccounts FuturesFixedAccount
		err = JsonString2Struct(jsonString, &fixedAccounts)
		if err != nil {
			return futuresCurrencyAccounts, err
		} else {
			futuresCurrencyAccounts.MarginMode = FIXED
			futuresCurrencyAccounts.fixedAccount = fixedAccounts
		}
	} else {
		var crossAccounts FuturesCrossAccount
		err = JsonString2Struct(jsonString, &crossAccounts)
		if err != nil {
			return futuresCurrencyAccounts, err
		} else {
			futuresCurrencyAccounts.MarginMode = CROSS
			futuresCurrencyAccounts.crossAccount = crossAccounts
		}
	}
	return futuresCurrencyAccounts, nil
}

func parsePage(response *http.Response) PageResult {
	var page PageResult
	jsonString := GetResponsePageJsonString(response)
	JsonString2Struct(jsonString, &page)
	return page
}
