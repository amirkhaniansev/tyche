#include "logger.hpp"
int main() {
	Logger ll("Modul", 10);
	LogInfo l1(DEFAULT, "Test Exception1", "Test loginfo1", "15:15:20"),
			l2(FATAL, "Test Exception2", "Test loginfo2", "15:18:20"),
			l3(SUCCESS, "Test Exception3", "Test loginfo3", "5144984");
	ll.Log(l1);
	ll.Log(l2);
	ll.Log(l3);
	std::this_thread::sleep_for(std::chrono::seconds(20));
	LogInfo 
		l11(DEFAULT, "Test Exception11", "Test loginfo1", "15:15:20"),
		l12(FATAL, "Test Exception12", "Test loginfo2", "15:18:20"),
		l13(SUCCESS, "Test Exception13", "Test loginfo3", "5144984");
	ll.Log(l11);
	ll.Log(l12);
	ll.Log(l13);
	system("PAUSE");
	return 0;
}