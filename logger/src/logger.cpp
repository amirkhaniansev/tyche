/*
* GNU General Public License Version 3.0, 29 June 2007
* Header file of logger.
* Copyright (C) 2018  David Petrosyan
* Email: david.petrosyan11100@gmail.com
* For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
*/
#include "../include/logger.hpp"


void addSecond(std::string &time)
{
	int h = (time[0]-'0') * 10 + (time[1]-'0');
	int m = (time[3]-'0') * 10 + (time[4]-'0');
	int s = (time[6]-'0') * 10 + (time[7]-'0');
	if (s == 59)
	{
		if (m == 59)
		{

			if (h == 23)
				h = 0;
			else
				h++;

			m = 0;
			s = 0;
		}
		else
		{
			m++;
			s = 0;
		}
	}
	else
	{
		s++;
	}
	time[0] = h / 10 + '0';
	time[1] = h % 10 + '0';
	time[3] = m / 10 + '0';
	time[4] = m % 10 + '0';
	time[6] = s / 10 + '0';
	time[7] = s % 10 + '0';
}
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
void Logger::passiveLogThreadFunction() {
	while(true){
		std::this_thread::sleep_for(std::chrono::minutes(this->interval));	
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
Logger::Logger(std::string modulName,int interval) : timerThread(&Logger::passiveLogThreadFunction, this)
{
	if (interval < 60)
		interval = 60;
	
	this->interval = interval;

	this->modulName = modulName;	
	
	this->logCache = new std::unordered_map<std::string, LogInfo>();

	this->filePath = Date() + ".txt";

	std::ifstream f(this->filePath);
	if (!f.good())
		std::ofstream file(this->filePath, std::ofstream::app);

}
void Logger::Log(const LogInfo &l)
{	
	static std::string last_log_time = Time();
	std::string current_time = Time();

	if (current_time == "23:59:59")
	{
		writeInFile();
		current_time = "00:00:00";
	}
	else {
		while (last_log_time >= current_time) 
			addSecond(current_time);
	}
	last_log_time = current_time;
	
	std::pair<std::string, LogInfo> logpair(current_time, l);
	
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
		file<< itr->first<<"\t"
			<<"ErrorType- "<< itr->second.errorType<<"\t"
			<<"ExceptionMessage- "<< itr->second.exceptionMessage<<"\t\t"
			<<"Message- "<< itr->second.message<<"\t"
			<<"Time- "<< itr->second.time<< std::endl;		
		itr++;
	}
	this->logCache->clear();
	file.close();
	
}
Logger::~Logger()
{
	delete logCache;
}