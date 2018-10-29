//
// Created by zxp on 17/07/18.
//

#ifndef CPPSDK_UTILS_H
#define CPPSDK_UTILS_H

#include <ctime>
#include <string>
#include <map>
using namespace std;

char * GetTimestamp(char *timestamp, int len);
string BuildParams(string requestPath, map<string,string> m);
string JsonFormat(string jsonStr);

#endif //CPPSDK_UTILS_H
