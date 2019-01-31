/*
* GNU General Public License Version 3.0, 29 June 2007
* Header file of logger.
* Copyright (C) 2018  David Petrosyan
* Email: david.petrosyan11100@gmail.com
* For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
*/
#include "../include/logger.h"

std::string Date() {
	time_t now = time(0);
	tm g;
	localtime_s(&g,&now);

	std::string s;

	char d1 = g.tm_mday / 10 + 0x30;
	char d2 = g.tm_mday % 10 + 0x30;
	s += d1;
	s += d2;
	s += '.';

	g.tm_mon++;
	char m1 = g.tm_mon / 10 + 0x30;
	char m2 = g.tm_mon % 10 + 0x30;
	s += m1;
	s += m2;
	s += '.';

	g.tm_year += 1900;
	char y4 = g.tm_year % 10 + 0x30;
	char y3 = g.tm_year / 10 % 10 + 0x30;
	char y2 = g.tm_year / 100 % 10 + 0x30;
	char y1 = g.tm_year / 1000 % 10 + 0x30;
	s += y1;
	s += y2;
	s += y3;
	s += y4;

	return s;
}
std::string Time() {
	time_t now = time(0);
	tm g;
	localtime_s(&g, &now);

	std::string s;

	char h1 = g.tm_hour / 10 + 0x30;
	char h2 = g.tm_hour % 10 + 0x30;
	s += h1;
	s += h2;
	s += ":";

	char m1 = g.tm_min / 10 + 0x30;
	char m2 = g.tm_min % 10 + 0x30;
	s += m1;
	s += m2;
	s += ":";

	char s1 = g.tm_sec / 10 + 0x30;
	char s2 = g.tm_sec % 10 + 0x30;
	s += s1;
	s += s2;

	return s;
}
void Logger::timerThreadFunction() {
	while(true){
		std::cout << "Thread is started\n";
		std::this_thread::sleep_for(std::chrono::minutes(this->interval));
		std::cout << "Thread wake up\n";
		try {
			this->cacheMutex.lock();
			
			writeInFile();

			this->cacheMutex.unlock();
		}
		catch(std::exception &e){
			this->cacheMutex.unlock();
		}
	}
}
Logger::Logger(std::string modulName,int interval) : timerThread(&Logger::timerThreadFunction, this)
{
	if (interval < 60)
		interval = 60;
	
	this->interval = interval;

	this->modulName = modulName;	
	
	this->logCache = new std::unordered_map<std::string, LogInfo>();

	this->filePath = Date() + ".txt";

	std::ifstream f(this->filePath);
	if (!f.good())
		std::ofstream file(this->filePath,std::ofstream::app);

}
void Logger::Log(const LogInfo &l)
{
	std::pair<std::string, LogInfo> logpair(Time(), l);
	this->logCache->insert(logpair);
	if (this->logCache->size() > 10000) {
		try {
			this->cacheMutex.lock();

			writeInFile();

			this->cacheMutex.unlock();
		}
		catch (std::exception &e) {
			this->cacheMutex.unlock();
		}
	}
}
void Logger::writeInFile()
{
	std::unordered_map<std::string, LogInfo>::iterator itr = this->logCache->begin();
	std::ofstream file;
	file.open(this->filePath, std::ios_base::app);
	while(itr != this->logCache->end())
	{		
		file <<"\n"<< itr->first << "\n"
			<< "ErrorType"<<"\t\t"<< itr->second.errorType << "\n"
			<< "ExceptionMessage"<<"\t"<< itr->second.exceptionMessage << "\n"
			<< "Message"<<"\t\t\t"<< itr->second.message << "\n"
			<< "Time"<<"\t\t\t"<< itr->second.time << "\n" << std::endl;		
		itr = this->logCache->erase(itr);
	}
	file.close();
	
}
Logger::~Logger()
{
	delete logCache;
}