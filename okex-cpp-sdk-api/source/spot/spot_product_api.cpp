#include "../okapi.h"

static string SpotProductPrefix = "/api/spot/v3/";

/**
 * 全部币对信息
 *
 * @return
 */
string OKAPI::GetProducts() {
    return Request(GET, SpotProductPrefix+"products");
}


string OKAPI::GetProductsByProductId(string instrument_id, string size, string depth) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("size", size));
    m.insert(make_pair("depth", depth));
    string request_path = BuildParams(SpotProductPrefix+"products/"+instrument_id+"/book", m);
    return Request(method, request_path);
}

string OKAPI::GetTickers() {
    return Request(GET, SpotProductPrefix+"products/ticker");
}

string OKAPI::GetTickerByProductId(string instrument_id) {
    return Request(GET, SpotProductPrefix+"products/"+instrument_id+"/ticker");
}

string OKAPI::GetTrades(string instrument_id, string from, string to, string limit) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("from", from));
    m.insert(make_pair("to", to));
    m.insert(make_pair("limit", limit));
    string request_path = BuildParams(SpotOrderPrefix+"products/"+instrument_id+"/trades", m);
    return Request(method, request_path);
}

string OKAPI::GetCandles(string instrument_id, string granularity, string start, string end) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("granularity", granularity));
    m.insert(make_pair("start", start));
    m.insert(make_pair("end", end));
    string request_path = BuildParams(SpotOrderPrefix+"products/"+instrument_id+"/candles", m);
    return Request(method, request_path);
}
