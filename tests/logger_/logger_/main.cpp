/**
 * GNU General Public License Version 3.0, 29 June 2007
 * main
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

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