/*
* GNU General Public License Version 3.0, 29 June 2007
* Header file of logger.
* Copyright (C) 2018  David Petrosyan
* Email: david.petrosyan11100@gmail.com
* For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
*/


#ifndef LOGGER_H
#define LOGGER_H

#include <iostream>
#include <string>					
#include <unordered_map>
#include <mutex>
#include <thread>
#include <fstream>
#include <ctime>
#include <chrono>        

enum ErrorType {
	SUCCESSS = 0x0,
	FATAL = 0x1,
	DEFAULT = 0x2,
	FAIL = 0x3
};

struct LogInfo {
	ErrorType errorType;
	std::string message;
	std::string exceptionMessage;
	std::string time;
	LogInfo(ErrorType e, std::string m, std::string ex, std::string t) :errorType(e), message(m), exceptionMessage(ex), time(t) {};
};

class Logger
{
public:	
	Logger(std::string, int);
	~Logger();
	void Log(const LogInfo&);
	void writeInFile();
	void timerThreadFunction();
private:	
	
	std::unordered_map<std::string, LogInfo> *logCache;
	std::mutex cacheMutex;
	std::thread timerThread;
	std::string filePath;
	std::string modulName;
	int interval;
};

#endif