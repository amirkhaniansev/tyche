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


#include "../include/vector.h"

static int power_of_two(int x) {
	int i = 2;
	for (; i < x; i *= 2){}
	return i;
}
static void safe_free(void * pointer) {
	free(pointer);
	pointer = NULL;
}

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
 *			etc.
 * @comparator - pointer to function which will be used to compare data.
 * @assigner - pointer to function which will be used to assign data.
 * @finalizer - pointer to function which will be used to finalize object.
 * @copy_func - pointer to function which will be used to copy data.
 * 
 * Must be called with valid arguments, otherwise the result will be NULL.
 * 
 * Errors
 * SUCCESSFULLY_COMPLETED						(0x0)	if function completed successfully(no error)
 * VECTOR_ALLOCATION_ERROR						(0x101)	if allocation cannot be realized
 * VECTOR_DATASIZE_NEGATIVE						(0x106)	if datasize argument negative
 * VECTOR_COMPARATOR_ERROR_IN_CONSTRUCTOR		(0x102)	if comparator function is NULL
 * VECTOR_ASSIGNER_ERROR_IN_CONSTRUCTOR			(0x103)	if assigner function is NULL 
 * VECTOR_FINALIZER_ERROR_IN_CONSTRUCTOR		(0x104)	if finalizer function is NULL
 * VECTOR_BASE_ALLOCATION_ERROR_IN_CONSTRUCTOR	(0x105)	if vector's _base allocation cannot be realized
 */
vector* vector_create(
	unsigned int initial_size,
	unsigned int data_size,
	bool is_primitive_type,
	int * error_code,
	int(*comparator)(const void*, const void*),
	int(*assigner)(void*, void*),
	int(*finalizer)(void*),
	void*(*copy_func)(void*))
{
	vector* _vector = NULL;
	*error_code = SUCCESSFULLY_COMPLETED;
	
	if (initial_size < 0)
		return NULL;
	initial_size = power_of_two(initial_size);

	_vector = malloc(sizeof(vector));
	
	_vector->_size = initial_size;

	
	if (_vector == NULL) {
		*error_code = VECTOR_ALLOCATION_ERROR;
		return NULL;
	}

	if (data_size < 0) {
		*error_code = VECTOR_DATASIZE_NEGATIVE;
		return NULL;
	}
	_vector->_is_primitive_type = is_primitive_type;
	_vector->_data_size = data_size;
	
	if (comparator == NULL) {
		*error_code = VECTOR_COMPARATOR_ERROR_IN_CONSTRUCTOR;
		return NULL;
	}
	_vector->_comparator = comparator;

	if (assigner == NULL) {
		*error_code = VECTOR_ASSIGNER_ERROR_IN_CONSTRUCTOR;
		return NULL;
	}
	_vector->_assigner = assigner;

	if (finalizer == NULL) {
		*error_code = VECTOR_FINALIZER_ERROR_IN_CONSTRUCTOR;
		return NULL;
	}
	_vector->_finalizer = finalizer;
	

	_vector->_base = malloc(_vector->_size * sizeof(void*));
	if (_vector->_base == NULL) {
		*error_code = VECTOR_BASE_ALLOCATION_ERROR_IN_CONSTRUCTOR;
		return NULL;
	}
	
	_vector->_count = 0;

	return _vector;
}

/**
* front - gets the first element of vector
* @vector - vector
* May be called with non-null argument
*/
void * vector_front(vector * vector)
{
	if (vector == NULL || vector->_base == NULL || vector->_count < 1)
		return NULL;
	else return vector->_base[0];
}

/**
* back - gets the last element of vector
* @vector - vector
* May be called with non-null argument
*/
void * vector_back(vector * vector)
{
	if (vector == NULL || vector->_base == NULL || vector->_count < 1)
		return NULL;
	else return vector->_base[vector->_count - 1];
}

/**
* at - gets the element with the specified index.
*
* @vector - vector
* @index - index of element
*
* May be called with valid arguments.
* Return value can be NULL if index is out of range or vector is null.
*/
void * vector_at(vector * vector, unsigned int index)
{
	if (vector == NULL || vector->_base == NULL || vector->_count < 1 || index > vector->_count - 1) 
		return NULL;
	return vector->_base[index];
}

/**
 * capacity - gets the allocated capacity of vector
 * @vector - vector
 * May be called with non-null argument
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 */
unsigned int vector_capacity(vector * vector)
{
	if (vector == NULL)
		return VECTOR_IS_NULL;
	else if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;
	else 
		return vector->_size;
}

/** 
 * assign - assignes right vector to the left vector
 * 
 * @left - left vector
 * @right - right vector
 * 
 * May be called with valid arguments.
 *
 * Errors
 * VECTOR_ASSIGN_RIGHT_IS_NULL(0x108) 				if vector that have to assign is NULL
 * VECTOR_BASE_ALLOCATION_ERROR_IN_ASSIGN(0x109)	if vector's _base allocation cannot be realized
 */
int vector_assign(vector * left, vector * right)
{
	if (right == NULL){
		return VECTOR_ASSIGN_RIGHT_IS_NULL;
	}
	else if (right->_base == NULL) {
		left->_base = NULL;
		left->_count = left->_size = 0;
		return 0;
	}
	
	int error = vector_destroy(left);
	if (error != 0)return error;

	left->_count = right->_count;
	left->_size = right->_size;
	left->_data_size = right->_data_size;
	left->_is_primitive_type = right->_is_primitive_type;
	left->_comparator = right->_comparator;
	left->_assigner = right->_assigner;
	left->_finalizer = right->_finalizer;

	left->_base = malloc(left->_size * sizeof(void*));
	if (left->_base == NULL)
		return VECTOR_BASE_ALLOCATION_ERROR_IN_ASSIGN;
	
	for (unsigned int i = 0; i < left->_count; i++)
		left->_assigner(vector_at(left, i), vector_at(right, i));
	
	return 0;
}

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
 * INVALID_DATA(0x100)								if given data argument is NULL
*/
int vector_insert(vector * vector, unsigned int position, void * data)
{
	if (vector == NULL)
		return VECTOR_IS_NULL;
	if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;
	if (position > vector->_count)
		return VECTOR_HIGH_POSSITION_IN_INSERT;
	if (data == NULL)
		return INVALID_DATA;
	
	/**
	* If vector is full, we will allocate two times more memory and then will rewrite vector's data.
	*/
	if (vector->_count == vector->_size) {
		if (vector->_size < 2)vector->_size = 2;
		
		void **base_temp = malloc(2 * vector->_size * sizeof(void*));
		if (base_temp == NULL)
			return VECTOR_BASE_ALLOCATION_ERROR_IN_INSERT;
		for (unsigned int i = 0; i < vector->_size; i++)
			base_temp[i] = vector->_base[i];
		
		vector->_size *= 2;
		
		safe_free(vector->_base);
		vector->_base = base_temp;
		base_temp = NULL;			
	}

	void* pre_temp, *next_temp;
	pre_temp = vector->_base[position];
	for (unsigned int i = position; i < vector->_count; i++) {
		next_temp = vector->_base[i + 1];
		vector->_base[i + 1] = pre_temp;
		pre_temp = next_temp;
	}
	vector->_count++;
	vector->_base[position] = data;

	return 0;
}

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
 */
int vector_erase(vector * vector, unsigned int position)
{
	if (vector == NULL)
		return VECTOR_IS_NULL;
	else if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;
	else if (vector->_count < 1)
		return VECTOR_IS_EMPTY;
	else if (vector->_count <= position)
		return VECTOR_ERASE_POSITION_OUT_OF_RANGE;
	
	if(!vector->_is_primitive_type)
		vector->_finalizer(vector->_base[position]);
	safe_free(vector->_base[position]);
	
	for (unsigned int i = position; i < vector->_count - 1; i++)
		vector->_base[i] = vector->_base[i + 1];
	
	vector->_count--;
	return 0;
}

/** 
 * push_back - inserts new element to the head of vector
 * 
 * @vector - vector
 * @data - data
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 * INVALID_DATA(0x100)								if given data argument is NULL
 */
int vector_push_back(vector * vector, void * data)
{
	if (vector == NULL)
		return VECTOR_IS_NULL;
	if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;
	if (data == NULL)
		return INVALID_DATA;
	
	return vector_insert(vector, vector->_count, data);
}

/**
 * pop_back - deletes the last element of vector
 * @vector - vector
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 * VECTOR_IS_EMPTY(0x112)							if vector's count equal to 0 
 */
int vector_pop_back(vector * vector)
{
	if (vector == NULL) 
		return VECTOR_IS_NULL;
	else if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;
	else if (vector->_count < 1)
		return VECTOR_IS_EMPTY;

	vector->_count--;
	return 0;
}

/**
 * clear - clears vector
 * @vector - vector
 *
 * Errors
 * VECTOR_IS_NULL(0x114) 							if vector is NULL
 * VECTOR_IS_CLEAR(0x115)	 						if vector's base is NULL
 */
int vector_clear(vector * vector)
{
	if (vector == NULL) 
		return VECTOR_IS_NULL;
	else if (vector->_base == NULL)
		return VECTOR_IS_CLEAR;

	if (!vector->_is_primitive_type)
		for (unsigned int i = 0; i < vector->_size; i++)
			vector->_finalizer(vector->_base[i]);

	safe_free(vector->_base);
	vector->_count = 0;
	vector->_size = 0;

	return 0;
}

/**
* destroy_vector - destroys vector
* @vector - vector
*/
int vector_destroy(vector * vector)
{
	if (vector == NULL)
		return 0;

	vector_clear(vector);
	
	safe_free(vector);
	return 0;
}

/**
* is_empty - checks if the given vector is empty
* @vector - vector
*/
bool vector_is_empty(vector * vector)
{
	if (vector == NULL || vector->_base == NULL || vector->_count < 1)
		return true;
	else  return false;
}


