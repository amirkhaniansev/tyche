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

#include "list.h"
#include <stdio.h>

int comparator(const void*a, const void*b) {

	return 0;
}
int assigner(void*a, void*b) {
	return 0;
}
int finalizer(void*a) {
	return 0;
}
void* copy_func(void*a) {
	return a;
}

int main() {
	list *l;
	list_iterator data, temp;
	int error = 0;
	l = list_create(error, sizeof(int), true, &comparator, &assigner, &finalizer, &copy_func);
	if (error != 0)wprintf(L"Error create %d \n", error);

	int a[16];
	for (int i = 0; i < 16; i++)a[i] = i;

	for (int i = 0; i < 16; i++) {
		error = list_insert_po(l, 0, &a[i]);
		if (error != 0)
		{
			wprintf(L"Error %d\n", error);
			getchar();
		}
	}
	int b = 20;

	error = list_insert_po(l, 10, &b);
	if (error != 0)
	{
		wprintf(L"Error %d\n", error);
		getchar();
	}
	/*error = list_erase_po(l, 3);
	if (error != 0)
	{
		wprintf(L"Error %d\n", error);
		getchar();
	}*/
	temp = list_at(l, 5);
	error = list_erase_it(l, temp);
	if (error != 0)
	{
		wprintf(L"Error %d\n", error);
		getchar();
	}
	
	int h = 30;
	temp = list_at(l, 12);
	error = list_insert_it(l, temp, &h);
	if (error != 0)
	{
		wprintf(L"Error %d\n", error);
		getchar();
	}
	
	list *ll = NULL;
	ll = list_assign(l);
	if(ll==NULL) {
		wprintf(L"Error NULL \n");
		getchar();
		return 0;
	}
	if (error != 0)
	{
		wprintf(L"Error %d\n", error);
		getchar();
	}
	list_destroy(l);
	for (unsigned int i = 0; i < ll->_count; i++) {
		data = list_at(ll, i);
		int *x = data->_data;
		wprintf(L"at %d\n", *x);
	}

	getchar();
	list_clear(l);
	for (unsigned int i = 0; i < l->_count; i++) {
		data = list_at(l, i);		
		if (data != NULL) {
			int *x = data->_data;
			wprintf(L"at %d\n", *x);
		}
	}
	list_destroy(l);
	getchar();
	return 0;
}