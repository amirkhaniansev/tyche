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
#include <assert.h>
#include <limits.h>

#include "../../src/APIs/ChatAPI/include/generic/stack.h"

__DEFINE_GENERIC_STACK__(int);

int main(int argc, char** argv)
{
	stack_int* stack = stack_create_int();
	assert(stack != NULL);

	int limit = INT_MAX / 100;
	for(int i = 0; i < limit; i++)
		assert(stack_push_int(stack, &i) == STACK_SUCCESS);

	for(int i = 0; i < limit; i++)
	{
		int value = *stack_top_int(stack);
		printf("%d\n", value);
		assert(stack_pop_int(stack) == STACK_SUCCESS);
	}

	assert(stack_clear_int(stack, NULL) == STACK_SUCCESS);
	assert(stack_destroy_int(stack, NULL) == STACK_SUCCESS);

	return 0;
}
