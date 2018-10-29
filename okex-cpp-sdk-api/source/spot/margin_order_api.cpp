#include "../okapi.h"

static string MarginOrderPrefix = "/api/margin/v3/";

string OKAPI::AddOrder(value &jsonObj) {
    string params = jsonObj.serialize();
    return Request(POST, MarginOrderPrefix+"orders", params);
}

string OKAPI::AddBatchOrder(value &jsonObj) {
    string params = jsonObj.serialize();
    return Request(POST, MarginOrderPrefix+"batch_orders", params);
}
/**
 * Cancle a order
 *
 * @param instrument_id
 * @param order_id
 */
string OKAPI::CancleOrdersByProductIdAndOrderId(string order_id, string instrument_id) {
    string method(DELETE);
    map<string,string> m;
    m.insert(make_pair("instrument_id", instrument_id));
    string request_path = BuildParams(MarginOrderPrefix+"cancel_orders/"+order_id, m);
    return Request(method, request_path);
}


/**
 * Batch cancle order
 *
 * @param instrument_id
 */
string OKAPI::CancleOrdersByProductId(string instrument_id) {
    string method(DELETE);
    map<string,string> m;
    m.insert(make_pair("instrument_id", instrument_id));
    string request_path = BuildParams(MarginOrderPrefix+"cancel_batch_orders", m);
    return Request(method, request_path);
}

/**
 * get a order
 *
 * @param instrument_id
 * @param order_id
 * @return
 */
string OKAPI::GetOrderByProductIdAndOrderId(string order_id, string instrument_id) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("instrument_id", instrument_id));
    string request_path = BuildParams(MarginOrderPrefix+"orders/"+order_id, m);
    return Request(method, request_path);
}

/**
 * get order list
 *
 * @param instrument_id
 * @param status    pending、done、archive
 * @param from
 * @param to
 * @param limit
 * @return
 */
string OKAPI::GetOrders(string instrument_id, string status, string from, string to, string limit) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("instrument_id", instrument_id));
    m.insert(make_pair("status", status));
    m.insert(make_pair("from", from));
    m.insert(make_pair("to", to));
    m.insert(make_pair("limit", limit));
    string request_path = BuildParams(MarginOrderPrefix+"orders", m);
    return Request(method, request_path);
}


string OKAPI::GetFills(string order_id, string instrument_id,  string from, string to, string limit) {
    string method(GET);
    map<string,string> m;
    m.insert(make_pair("instrument_id", instrument_id));
    m.insert(make_pair("from", from));
    m.insert(make_pair("to", to));
    m.insert(make_pair("limit", limit));
    string request_path = BuildParams(MarginOrderPrefix+"fills", m);
    return Request(method, request_path);
}

