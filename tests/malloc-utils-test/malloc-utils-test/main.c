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

#include <stdlib.h>
#include <stdio.h>

#include "include/malloc_utilities.h"

int big_structure()
{
	typedef struct big_structure
	{
		int x11;
		int x12;
		int x13;
		int x21;
		int x22;
		int x23;
		int x31;
		int x32;
		int x33;
	} matrix;

	__def_gc__();
	__def_var__(matrix, m);

	__gc_return__(0);
}

void big_char()
{
	__def_gc__();
	__def_arr__(char, arr, 100000000);
	__gc_return__();
}

void big_unsigned_long_long()
{
	__def_gc__();
	__def_arr__(unsigned long long, ulong, 1000000);

	for (int i = 0; i < 10000; i++)
		ulong[i] = 1UL;

	__gc_return__();
}

int main(int argc, char* argv[])
{
	big_char();
	big_structure();
	big_unsigned_long_long();

	return 0;
}