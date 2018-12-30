/*
 * GNU General Public License Version 3.0, 29 June 2007
 * Header file of list.
 * Copyright (C) 2018  Sevak Amirkhanian
 * Email: amirkhanyan.sevak@gmail.com
 * For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
 */

#ifndef __LIST_H__
#define __LIST_H__

#include <stdbool.h>

/* structures for linked list node */
typedef struct list_node_
{
	struct list_node_* _next;
	struct list_node_* _prev;
	void* _data;
} list_node;

/* structure for linked list interface */
typedef struct list_generic_structure
{
	unsigned int _count;
	unsigned int _data_size;
	bool _is_primitive_type;

	list_node* _top;
	list_node* _last;

	int   (*_comparator)(const void*, const void*);
	int   (*_assigner) (const void*, const void*);
	int   (*_finalizer)(const void*);
	void* (*_copy_func)(const void*);
} list;

typedef list_node* list_iterator;

/**
 * list_create - creates list
 *
 * @data_size - data size
 * @is_primitive_type - boolean value indicating whether
 *			list holds data of primitive type.
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
list* list_create(unsigned int data_size,
		bool is_primitive_type,
		int(*comparator)(const void*, const void*),
		int(*assigner)(const void*, const void*),
		int(*finalizer)(const void*),
		void*(copy_func)(const void*));

/**
 * list_front - gets the first element of list
 * @list - list
 * May be called with non-null argument.
 */
void* list_front(list* list);

/**
 * list_back - gets the last element of list
 * @list - list
 * May be called with non-null argument
 */
void* list_back(list* list);

/**
 * list_at - gets the element with the specified position
 *
 * @list - list
 * @position - position
 */
void* list_at(list* list, unsigned int position);

/**
 * list_assign - assigns right list to the left list
 *
 * @left - left list
 * @right - right list
 *
 * May be called with valid arguments.
 */
int list_assign(list* left, list* right);

/**
 * list_push_front - pushes new element to the front of list
 *
 * @list - list
 * @data - data that will be added to the front of list
 */
int list_push_front(list* list, void* data);

/**
 * list_pop_front - pops new element from the front of list
 *
 * @list - list
 * @data - data that will be removed from the front of list
 */
int list_pop_front(list* list, void* data);

/**
 * list_push_back - inserts new element to the head of list
 *
 * @list - list
 * @data - data
 */
int list_push_back(list* list, void* data);

/**
 * list_pop_back - deletes the last element of list
 * @vector - vector
 */
int list_pop_back(list* list);

/**
 * list_insert_it - inserts new item to the list after specified iterator
 *
 * @list - list
 * @position - iterator representing the node after which the
 * 				new element will be added.
 */
int list_insert_it(list* list, list_iterator position, void*data);

/**
 * list_insert_po - inserts new item to the list at the specified index
 *
 * @list - list
 * @position - index
 * @data - data
 */
int list_insert_po(list* list, unsigned int position, void* data);

/**
 * list_sort - sorts the list
 *
 * @list - list
 */
int list_sort(list* list);

/**
 * erase_it - deletes the item with the given position
 *
 * @list - list
 * @position - iterator representing the node which will be deleted.
 */
int list_erase_it(list* list, list_iterator position);

/**
 * list_erase_po - deletes the item with the given index
 *
 * @list - list
 * @position - index of element
 */
int list_erase_po(list* list,unsigned int position);

/**
 * list_clear - clears list
 * @list - list
 */
int list_clear(list* list);

/**
 * list_destroy - destroys list
 * @list - list
 */
int list_destroy(list* list);

/**
 * list_is_empty - checks if the given list is empty
 * @list - list
 */
bool list_is_empty(list* list);

/**
 * list_begin - gets the beginning iterator of list
 * @list - list
 */
list_iterator list_begin(list* list);

/**
 * list_end - gets the iterator indicating the end of the list
 * @list - list
 */
list_iterator list_end(list* list);

/**
 * list_move_next - moves iterator to the next element
 * @iterator - list iterator
 */
int list_move_next(list_iterator iterator);

/**
 * list_move_prev - moves iterator to the previous element
 * @iterator - list iterator
 */
int list_move_prev(list_iterator iterator);

#endif
