/*
 * GNU General Public License Version 3.0, 29 June 2007
 * Header file of vector.
 * Copyright (C) 2018  Sevak Amirkhanian
 * Email: amirkhanyan.sevak@gmail.com
 * Copyright (C) 2018  David Petrosyan
 * Email: david.petrosyan11100@gmail.com
 * For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
 */

#ifndef __VECTOR_H__
#define __VECTOR_H__

#include <stdbool.h>

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
	int   (*_assigner) (const void*, const void*);
	int   (*_finalizer)(const void*);
	void* (*_copy_func)(const void*);
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
 */
vector* vector_create(unsigned int initial_size,
	unsigned int data_size,
	bool is_primitive_type,
	int * error_code,
	int(*comparator)(const void*, const void*),
	int(*assigner)(const void*, const void*),
	int(*finalizer)(const void*),
	void*(*copy_func)(const void*)
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
 */
unsigned int vector_capacity(vector* vector);

/** 
 * assign - assignes right vector to the left vector
 * 
 * @left - left vector
 * @right - right vector
 * 
 * May be called with valid arguments.
 */
int vector_assign(vector* left, vector* right);

/**
 * insert - inserts new item to vector
 * 
 * @vector - vector
 * @data - data
 * @position - position where new item must be inserted
 */
int vector_insert(vector* vector, unsigned int position, void* data);

/** 
 * erase - erases data from the specified position
 *
 * @vector - vector
 * @position - position
 */
int vector_erase(vector* vector, unsigned int position);

/** 
 * push_back - inserts new element to the head of vector
 * 
 * @vector - vector
 * @data - data
 */
int vector_push_back(vector* vector, void* data);

/**
 * pop_back - deletes the last element of vector
 * @vector - vector
 */
int vector_pop_back(vector* vector);

/**
 * clear - clears vector
 * @vector - vector
 */
int vector_clear(vector* vector);

/**
 * destroy_vector - destroys vector
 * @vector - vector
 */
int vector_destroy_vector(vector* vector);

/** 
 * is_empty - checks if the given vector is empty
 * @vector - vector
 */
bool vector_is_empty(vector* vector);

#endif