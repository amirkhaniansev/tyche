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

#include "vector.h"
#include <stdio.h>
int _comparator(const int * p, const int * q) {
	p = q;
	return 0;
}
int _assigner(const int * p, const int * q) {
	p = q;
	return 0;
}
int _finalizer(const void* p) {
	p = NULL;
	return 0;
}
void* _copy_func(const void *p) {
	return NULL;
}

int main() {
	int error;
	vector *v = vector_create(1, sizeof(int), true, &error, &_comparator, &_assigner, &_finalizer, &_copy_func);
	if(error!=SUCCESSFULLY_COMPLETED)
		printf("Error construct= %d\n", error);
	
	for (int i = 0; i < 1024; i++) {
		error = vector_push_back(v, i);
		if (error != 0)printf("Error push back= %d\n", error);
	}
	for (unsigned int i = 0; i < v->_size; i++) {
		int x = vector_at(v,i);
		printf("at= %d\n", x);
	}
	getchar();
	vector_destroy(v);
	return 0;
}