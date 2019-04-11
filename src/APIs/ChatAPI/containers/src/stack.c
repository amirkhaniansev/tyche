/**
 * GNU General Public License Version 3.0, 29 June 2007
 * stack
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


#include "../include/stack.h"
#include "../include/vector.h"

#define DEFAULT_INITIAL_SIZE	4
#define DEFAULT_DATA_SIZE		4

static void* default_copy_func(void* object)
{
	return object;
}

static int default_assigner_func(void* left, void* right)
{
	left = right;
	return 0;
}

static int default_finalizer_func(void* data)
{
	return 0;
}

static int default_comparator_func(const void* left,const void* right)
{
	return left > right ? 1 : left < right ? -1 : 0;
}

/**
 * stack_create - creates stack for primitive type
 * Return value is non-null if everything is OK
 */
stack* stack_create_p()
{
	return stack_create(
		DEFAULT_INITIAL_SIZE,
		DEFAULT_DATA_SIZE,
		true,
		default_comparator_func,
		default_assigner_func,
		default_finalizer_func,
		default_copy_func);
}

/**
 * stack_create - creates stack
 *
 * @data_size - data size
 * @is_primitive_type - boolean value indicating whether
 *			stack holds data of primitive type.
 *		Primitive types are considered:
 *			int
 *			short int
 *			double
 *			float
 *			char
 *			unsigned char
 *			long
 *			unsigned int
			etc.
 * @comparator - pointer to function which will be used to compare data.
 * @assigner - pointer to function which will be used to assign data.
 * @finalizer - pointer to function which will be used to finalize object.
 * @copy_func - pointer to function which will be used to copy data.
 *
 * Must be called with valid arguments, otherwise the result will be NULL.
 */
stack* stack_create(unsigned int initial_size,
		unsigned int data_size,
		bool is_primitive_type,
		int(*comparator)(const void*, const void*),
		int(*assigner)(void*, void*),
		int(*finalizer)(void*),
		void*(copy_func)(void*))
{
	int error_code;
	vector* storage_vector = vector_create(
		initial_size,
		data_size, is_primitive_type,
		&error_code,
		comparator,
		assigner,
		finalizer,
		copy_func);

	if(error_code != SUCCESSFULLY_COMPLETED)
		return NULL;

	stack* object = malloc(sizeof(stack));

	object ->_storage = storage_vector;
	object->_count = 0;
	object->_data_size = data_size;
	object->_is_primitive_type = is_primitive_type;
	object->_comparator = comparator;
	object->_assigner = assigner;
	object->_finalizer = finalizer;
	object->_copy_func = copy_func;

	return object;
}

/**
 * stack_top - gets the top element of stack without popping it.
 * @stack - stack
 */
void* stack_top(stack* stack)
{
	if(stack == NULL)
		return NULL;
	
	return vector_back(stack->_storage);
}

/**
 * stack_push - stack element to stack
 *
 * @stack - stack
 * @data - data that will be pushed to stack
 */
int stack_push(stack* stack, void* data)
{
	if(stack == NULL)
		return STACK_IS_NULL;
	if(data == NULL)
		return STACK_DATA_IS_NULL;

	if(vector_push_back(stack->_storage, data) != SUCCESSFULLY_COMPLETED)
		return STACK_PUSH_ERROR;
	
	stack->_count++;
	return STACK_SUCCESS;
}

/**
 * stack_pop - pops element from the stack
 *
 * @stack - stack
 * @data - data that will be popped from stack,.
 */
int stack_pop(stack* stack)
{
	if(stack == NULL)
		return STACK_POP_ERROR;

	if(vector_pop_back(stack->_storage) != SUCCESSFULLY_COMPLETED)
		return STACK_POP_ERROR;
	
	stack->_count--;
	return STACK_SUCCESS;
}

/**
 * stack_is_empty - checks if the given stack is empty
 * @stack - stack
 */
bool stack_is_empty(stack* stack)
{
	if(stack == NULL)
		return false;
	
	return vector_is_empty(stack->_storage);
}

/**
 * destroy_stack - destroys stack
 * @stack - stack
 */
int stack_destroy(stack* stack)
{
	if(stack == NULL)
		return STACK_SUCCESS;
	
	vector_destroy(stack->_storage);
	free(stack);

	return 0;
}
