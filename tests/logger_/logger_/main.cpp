#include "logger.h"
int main() {

	Logger ll("Modul", 10);
	LogInfo l(DEFAULT, "Test Exception1", "Test loginfo1", "15:15:20");
	LogInfo k(FATAL, "Test Exception2", "Test loginfo2", "15:18:20");
	ll.Log(l);
	ll.Log(k);

	system("PAUSE");
	return 0;
}