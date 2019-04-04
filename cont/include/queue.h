/**
 * GNU General Public License Version 3.0, 29 June 2007
 * queue
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


#ifndef __QUEUE_H__
#define __QUEUE_H__

#include <stdbool.h>

/**
 * Structure for queue interface.
 * Note that queue should use list for implementing storage,
 * because queue inserts occur only at the beginning.
 */
typedef struct queue_generic_structure
{
	unsigned int _count;
	unsigned int _data_size;
	bool _is_primitive_type;

	void* _storage;

	int   (*_comparator)(const void*, const void*);
	int   (*_assigner) (void*, void*);
	int   (*_finalizer)(void*);
	void* (*_copy_func)(void*);
} queue;

/**
 * queue_create - creates queue
 *
 * @data_size - data size
 * @is_primitive_type - boolean value indicating whether
 *			queue holds data of primitive type.
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
queue* queue_create(unsigned int data_size,
		bool is_primitive_type,
		int(*comparator)(const void*, const void*),
		int(*assigner)(void*, void*),
		int(*finalizer)(void*),
		void*(copy_func)(void*));

/**
 * queue_front - gets the first element of queue
 * @queue - queue
 * May be called with non-null argument.
 */
void* queue_front(queue* queue);

/**
 * queue_back - gets the last element of queue
 * @queue - queue
 * May be called with non-null argument
 */
void* queue_back(queue* queue);

/**
 * queue_push - pushes element to queue
 *
 * @queue - queue
 * @data - data that will be pushed to queue
 */
int queue_push(queue* queue, void* data);

/**
 * queue_pop - pops element from the queue
 *
 * @queue - queue
 * @data - data that will be popped.
 */
int queue_pop(queue* queue);

/**
 * queue_is_empty - checks if the given queue is empty
 * @queue - queue
 */
bool queue_is_empty(queue* queue);

/**
 * destroy_queue - destroys queue
 * @queue - queue
 */
int queue_destroy(queue* queue);

#endif
