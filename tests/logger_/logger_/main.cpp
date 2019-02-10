#include "logger.hpp"
int main() {

	Logger ll("Modul", 10);
	LogInfo l1(DEFAULT, "Test Exception1", "Test loginfo1", "15:15:20"),
			l2(FATAL, "Test Exception2", "Test loginfo2", "15:18:20"),
			l3(SUCCESS, "Test Exception3", "Test loginfo3", "5144984");
	ll.Log(l1);
	ll.Log(l2);
	ll.Log(l3);


	system("PAUSE");
	return 0;
}