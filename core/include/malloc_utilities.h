/**
 * GNU General Public License Version 3.0, 29 June 2007
 * C malloc utilities for automatic management of memory for some simple scenarios.
 * Copyright (C) <2018>
 *               Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *                        <DavidPetr>       <david.petrosyan11100@gmail.com>
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

#ifndef __MALLOC_UTILITIES_H__
#define __MALLOC_UTILITIES_H__

#include <stdlib.h>
#include "../../containers/include/stack.h"

typedef void* malloc_ptr;

#define __def_gc__()\
	stack* storage = stack_create_p()\

#define __def_var__(variable_type, variable)\
	variable_type* variable = malloc(sizeof(variable_type));\
	stack_push(storage, variable)

#define __def_arr__(variable_type, variable, length)\
	variable_type* variable = malloc(length * sizeof(variable_type));\
	stack_push(storage, variable)

#define __gc_return__(return_value)\
	do {\
		malloc_ptr ptr = NULL;\
		while(!stack_is_empty(storage)) {\
			ptr = stack_top(storage);\
			free(ptr);\
			stack_pop(storage);\
		}\
	} while(0)

#endif