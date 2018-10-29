//
// Created by zxp on 17/07/18.
//

#ifndef _UTIL_CPP_
#define _UTIL_CPP_

#include "utils.h"
#include <cpprest/http_client.h>

using namespace web;

char * GetTimestamp(char *timestamp, int len) {
    time_t t;
    time(&t);
    struct tm* ptm = gmtime(&t);
    strftime(timestamp,len,"%FT%T.123Z", ptm);
    return timestamp;
}

string BuildParams(string requestPath, map<string,string> m) {
    string str = requestPath;
    bool first = true;
    for(auto i=m.begin();i!=m.end();i++) {
        if (first) {
            str += "?";
            first = false;
        } else {
            str += "&";
        }
        str += i->first;
        str += "=";
        str += i->second;
    }
    return str;
}

string getLevelStr(int level) {
    string levelStr;
    for(int levelI = 0;levelI<level ; levelI++){
        levelStr.append("\t");
    }
    return levelStr;
}


string JsonFormat(string jsonStr) {
    int level = 0;
    string jsonForMatStr;
    for(int i=0;i<jsonStr.length();i++){
        char c = jsonStr.at(i);
        if(level>0&&'\n'==jsonForMatStr.at(jsonForMatStr.length()-1)){
            jsonForMatStr += getLevelStr(level);
        }
        switch (c) {
            case '{':
            case '[':
                jsonForMatStr += c;
                jsonForMatStr += '\n';
                level++;
                break;
            case ',':
                jsonForMatStr += c;
                jsonForMatStr += '\n';
                break;
            case '}':
            case ']':
                jsonForMatStr += '\n';
                level--;
                jsonForMatStr += getLevelStr(level);
                jsonForMatStr += c;
                break;
            default:
                jsonForMatStr += c;
                break;
        }
    }

    return jsonForMatStr;

}


// Demonstrates how to iterate over a JSON object.
void IterateJSONValue()
{
    // Create a JSON object.
    json::value obj;
    obj["key1"] = json::value::boolean(false);
    obj["key2"] = json::value::number(44);
    obj["key3"] = json::value::number(43.6);
    obj["key4"] = json::value::string(U("str"));


    // Loop over each element in the object.
    for (auto iter = obj.as_object().cbegin(); iter != obj.as_object().cend(); ++iter)
    {
        // Make sure to get the value as const reference otherwise you will end up copying
        // the whole JSON value recursively which can be expensive if it is a nested object.

        //const json::value &str = iter->first;
        //const json::value &v = iter->second;

        const auto &str = iter->first;
        const auto &v = iter->second;

        // Perform actions here to process each string and value in the JSON object...
        std::cout << "String: " << str.c_str() << ", Value: " << v.serialize() << endl;
    }

    /* Output:
    String: key1, Value: false
    String: key2, Value: 44
    String: key3, Value: 43.6
    String: key4, Value: str
    */
}


#endif