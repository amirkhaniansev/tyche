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

#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <stddef.h>
#include <limits.h>
#include <unistd.h>

#include "dictionary.h"

typedef struct
{
	int x;
	int y;
} A;

static int hash(const void* x)
{
	A* a = (A*)x;
	return a->x ^ a->y;
}
static bool compare(const void* x, const void* y)
{
	A* a = (A*)x;
	A* b = (A*)y;
	return a->x == b->x && a->y == b->y;
}

int main(int argc, char** argv)
{
	dictionary_t* d = dictionary_create(
			10,
			sizeof(int),
			sizeof(int),
			false,
			false,
			&compare,
			&hash,
			&free,
			&free
	);

	for(int i = 0; i < 4000; i++) {
		A* ptr0 = malloc(sizeof(A));
		ptr0->x = i;
		ptr0->y = i+1;
		int* ptr1 = malloc(sizeof(int));
		*ptr1 = i;
		dictionary_add(d, ptr0, ptr1);

	}

	dictionary_destroy(d);

	return 0;
}
