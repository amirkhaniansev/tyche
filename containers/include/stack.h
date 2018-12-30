/*
 * GNU General Public License Version 3.0, 29 June 2007
 * Header file of stack.
 * Copyright (C) 2018  Sevak Amirkhanian
 * Email: amirkhanyan.sevak@gmail.com
 * For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
 */

#ifndef __STACK_H__
#define __STACK_H__

#include <stdbool.h>

/**
 * Structure for stack interface.
 * Stack should use vector for internal storage.
 */
typedef struct stack_generic_structure
{
	unsigned int _count;
	unsigned int _data_size;
	bool _is_primitive_type;

	void* _storage;

	int   (*_comparator)(const void*, const void*);
	int   (*_assigner) (const void*, const void*);
	int   (*_finalizer)(const void*);
	void* (*_copy_func)(const void*);
} stack;

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
		int(*assigner)(const void*, const void*),
		int(*finalizer)(const void*),
		void*(copy_func)(const void*));

/**
 * stack_top - gets the top element of stack without popping it.
 * @stack - stack
 */
void* stack_top(stack* stack);

/**
 * stack_push - stack element to stack
 *
 * @stack - stack
 * @data - data that will be pushed to stack
 */
int stack_push(stack* stack, void* data);

/**
 * stack_pop - pops element from the stack
 *
 * @stack - stack
 * @data - data that will be popped from stack,.
 */
int stack_pop(queue* queue, void* data);

/**
 * stack_is_empty - checks if the given stack is empty
 * @stack - stack
 */
bool stack_is_empty(stack* stack);

/**
 * destroy_stack - destroys stack
 * @stack - stack
 */
int stack_destroy(stack* stack);

#endif
