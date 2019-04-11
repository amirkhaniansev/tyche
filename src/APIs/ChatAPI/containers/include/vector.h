/**
 * GNU General Public License Version 3.0, 29 June 2007
 * vector
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


#ifndef __VECTOR_H__
#define __VECTOR_H__

#include <stdbool.h>
#include <stdlib.h>
#include "containers_errors.h"
 
 /**
 * exit codes start from 0x100
 */
#define SUCCESSFULLY_COMPLETED							0x0
#define VECTOR_ALLOCATION_ERROR							0x101
#define VECTOR_COMPARATOR_ERROR_IN_CONSTRUCTOR			0x102
#define VECTOR_ASSIGNER_ERROR_IN_CONSTRUCTOR			0x103
#define VECTOR_FINALIZER_ERROR_IN_CONSTRUCTOR			0x104
#define VECTOR_BASE_ALLOCATION_ERROR_IN_CONSTRUCTOR     0x105
#define VECTOR_DATASIZE_NEGATIVE						0x106
#define VECTOR_ASSIGNER_IS_NULL							0x107
#define VECTOR_ASSIGN_RIGHT_IS_NULL						0x108
#define VECTOR_BASE_ALLOCATION_ERROR_IN_ASSIGN			0x109
#define VECTOR_BASE_ALLOCATION_ERROR_IN_INSERT			0x110
#define VECTOR_HIGH_POSSITION_IN_INSERT					0x111
#define VECTOR_IS_EMPTY									0x112
#define VECTOR_ERASE_POSITION_OUT_OF_RANGE				0x113
#define VECTOR_IS_NULL									0x114
#define VECTOR_IS_CLEAR									0x115
#define INVALID_DATA									0x100
#define VECTOR_PTR_IS_NULL								0x116

/**
 * structure for vector interface
 */
typedef struct vector_generic_structure
{
	unsigned int _count;
	unsigned int _size;
	unsigned int _data_size;
	bool _is_primitive_type;
	void** _base;

	int   (*_comparator)(const void*, const void*);
	int   (*_assigner) (void*, void*);
	int   (*_finalizer)(void*);
	void* (*_copy_func)(void*);
} vector;

/** 
 * create_vector - creates vector
 *
 * @intial_size - initial size of vector, must be non-negative
 * @data_size - data size
 * @is_primitive_type - boolean value indicating whether 
 *			vector holds data of primitive type.
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
 * 
 * Errors
 * SUCCESSFULLY_COMPLETED(0x0) 							if function completed successfully(no error)
 * VECTOR_ALLOCATION_ERROR(0x101)	 					if allocation cannot be realized
 * VECTOR_DATASIZE_NEGATIVE(0x106)			 			if datasize argument negative
 * VECTOR_COMPARATOR_ERROR_IN_CONSTRUCTOR(0x102)		if comparator function is NULL
 * VECTOR_ASSIGNER_ERROR_IN_CONSTRUCTOR(0x103)			if assigner function is NULL 
 * VECTOR_FINALIZER_ERROR_IN_CONSTRUCTOR(0x104)			if finalizer function is NULL
 * VECTOR_BASE_ALLOCATION_ERROR_IN_CONSTRUCTOR(0x105)	if vector's _base allocation cannot be realized
 */
vector* vector_create(unsigned int initial_size,
	unsigned int data_size,
	bool is_primitive_type,
	int * error_code,
	int(*comparator)(const void*, const void*),
	int(*assigner)(void*, void*),
	int(*finalizer)(void*),
	void*(*copy_func)(void*)
);

/** 
 * front - gets the first element of vector
 * @vector - vector
 * May be called with non-null argument
 */
void* vector_front(vector* vector);

/**
 * back - gets the last element of vector
 * @vector - vector
 * May be called with non-null argument
 */
void* vector_back(vector* vector);

/**
 * at - gets the element with the specified index.
 * 
 * @vector - vector
 * @index - index of element
 * 
 * May be called with valid arguments.
 * Return value can be NULL if index is out of range or vector is null.
 */
void* vector_at(vector* vector, unsigned int index);

/**
 * capacity - gets the allocated capacity of vector
 * @vector - vector
 * May be called with non-null argument
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 */
unsigned int vector_capacity(vector* vector);

/**
 * insert - inserts new item to vector
 *
 * @vector - vector
 * @data - data
 * @position - position where new item must be inserted
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 * VECTOR_HIGH_POSSITION_IN_INSERT(0x111)			if given position argument is bigger than vector's count
 * VECTOR_BASE_ALLOCATION_ERROR_IN_INSERT(0x110)	if vector's _base allocation cannot be realized
*/
int vector_insert(vector* vector, unsigned int position, void* data);

/** 
 * erase - erases data from the specified position
 *
 * @vector - vector
 * @position - position
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 * VECTOR_ERASE_POSITION_OUT_OF_RANGE(0x113)		if given position argument is bigger than vector's count
 * INVALID_DATA(0x100)								if given data argument is NULL
 */
int vector_erase(vector* vector, unsigned int position);

/** 
 * push_back - inserts new element to the head of vector
 * 
 * @vector - vector
 * @data - data
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 */
int vector_push_back(vector* vector, void* data);

/**
 * pop_back - deletes the last element of vector
 * @vector - vector
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 * VECTOR_IS_EMPTY(0x112)							if vector's count equal to 0 
 * INVALID_DATA(0x100)								if given data argument is NULL
 */
int vector_pop_back(vector* vector);

/**
 * clear - clears vector
 * @vector - vector
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 */
int vector_clear(vector* vector);

/**
 * destroy_vector - destroys vector
 * @vector - vector
 */
int vector_destroy(vector* vector);

/** 
 * is_empty - checks if the given vector is empty
 * @vector - vector
 */
bool vector_is_empty(vector* vector);

#endif