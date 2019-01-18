/*
* GNU General Public License Version 3.0, 29 June 2007
* Header file of list.
* Copyright (C) 2018  David Petrosyan
* Email: david.petrosyan11100@gmail.com
* For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
*/

#include "list.h"

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
*
* Errors
* LIST_DATA_SIZE_NEGATIVE	(0x116)	 					if datasize argument negative
* LIST_COMPARATOR_IS_NULL	(0x117)			 			if comparator function is NULL
* LIST_ASSIGNER_IS_NULL	(0x118)						if assigner function is NULL
* LIST_FINALIZER_IS_NULL	(0x119)						if finalizer function is NULL
* LIST_ALLOCATION_ERROR	(0x120)						if allocation cannot be realized
*/
list * list_create(
	unsigned int error_code,
	unsigned int data_size,
	bool is_primitive_type,
	int(*comparator)(const void *, const void *),
	int(*assigner)(void *, void *),
	int(*finalizer)(void *),
	void *(*copy_func)(void *))
{
	if (data_size < 0){
		error_code = LIST_DATA_SIZE_NEGATIVE;
		return NULL;
	}
	if (comparator == NULL) {
		error_code = LIST_COMPARATOR_IS_NULL;
		return NULL;
	}
	if (assigner == NULL) {
		error_code = LIST_ASSIGNER_IS_NULL;
		return NULL;
	}		
	if (finalizer == NULL) {
		error_code = LIST_FINALIZER_IS_NULL;
		return NULL;
	}

	list* List = malloc(sizeof(list));
	if (List == NULL) {
		error_code = LIST_ALLOCATION_ERROR;
		return NULL;
	}

	List->_count = 0;
	List->_data_size = data_size;
	List->_is_primitive_type = is_primitive_type;
	List->_top = List->_last = NULL;

	return List;
}
/**
* list_front - gets the first element of list
* @list - list
* May be called with non-null argument.
*/
void * list_front(list * list)
{
	if (list == NULL || list_is_empty(list))
		return NULL;

	return list->_top->_data;
}
/**
* list_back - gets the last element of list
* @list - list
* May be called with non-null argument
*/
void * list_back(list * list)
{
	if (list == NULL || list_is_empty(list))
		return NULL;

	return list->_last->_data;
}
/**
* list_at - gets the element with the specified position
*
* @list - list
* @position - position
*/
void * list_at(list * list, unsigned int position)
{
	if (list == NULL || list_is_empty(list) || list->_count <= position)
		return NULL;

	list_iterator finder;
	if (list->_count / 2 > position) {
		finder = list->_top;
		for (unsigned int i = 0; i < position; i++) {
			finder = finder->_next;
		}
	}
	else {
		finder = list->_last;
		for (unsigned int i = list->_count - 1; i > position; i--) {
			finder = finder->_prev;
		}
	}

	return finder;
}

/**
* list_assign - assigns right list to the list and return that list
*
* @right - right list
*
* May be called with valid argument.
*/
list* list_assign(list * right)
{
	if (list_is_empty(right))
		return NULL;
	
	list *List = malloc(sizeof(list));
	if (List == NULL)
		return NULL;

	List->_data_size = right->_data_size;
	List->_is_primitive_type = right->_is_primitive_type;
	List->_comparator = right->_comparator;
	List->_assigner = right->_assigner;
	List->_finalizer = right->_finalizer;
	List->_count = 0;

	list_iterator right_temp_node = right->_top;

	List->_top = NULL;
	List->_last = NULL;

	int error;
	while (right_temp_node != NULL) {
		error = list_push_back(List, right_temp_node->_data);
		if (error != 0)
			return NULL;
		right_temp_node = right_temp_node->_next;
	}

	return List;
}

/**
* list_push_front - pushes new element to the front of list
*
* @list - list
* @data - data that will be added to the front of list
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
* LIST_NODE_ALLOCATION_ERROR	(0x123)					if list's node allocation cannot be realized
* INVALID_DATA					(0x125)					if data is NULL
*/
int list_push_front(list * list, void * data)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (data == NULL)
		return INVALID_DATA;

	list_iterator new_node = malloc(sizeof(list_node));
	if (new_node == NULL)
		return LIST_NODE_ALLOCATION_ERROR;

	new_node->_data = data;
	new_node->_next = list->_top;
	new_node->_prev = NULL;

	if (list->_top != NULL)
		list->_top->_prev = new_node;
	
	list->_top = new_node;
	list->_count++;

	if (list->_count == 1)
		list->_last = list->_top;

	return 0;
}

/**
* list_pop_front - pops new element from the front of list
*
* @list - list
* @data - data that will be removed from the front of list
*
* Errors
* LIST_IS_NULL	(0x121)									if list is NULL
* LIST_IS_EMPTY(0x122)									if list is empty
*/
int list_pop_front(list * list)
{
	if (list == NULL) {
		return LIST_IS_NULL;
	}
	if (list_is_empty(list)) {
		return LIST_IS_EMPTY;
	}
	if (list->_count == 1) {
		return list_clear(list);
	}
	if (list->_count == 2) {
		free(list->_top);
		list->_top = list->_last;
		list->_top->_next = list->_top->_prev = NULL;
		list->_count--;

		return 0;
	}

	list_iterator replace_temp;
	replace_temp = list->_top->_next;
	replace_temp->_prev = NULL;

	if (!list->_is_primitive_type)
		list->_finalizer(list->_top);
	free(list->_top);

	list->_top = replace_temp;
	list->_count--;
	return 0;
}

/**
* list_push_back - inserts new element to the head of list
*
* @list - list
* @data - data
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
* LIST_NODE_ALLOCATION_ERROR	(0x123)					if list's node allocation cannot be realized
* INVALID_DATA					(0x125)					if data is NULL
*/
int list_push_back(list * list, void * data)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (data == NULL)
		return INVALID_DATA;
	if (list_is_empty(list))
		return list_push_front(list, data);

	list_iterator new_node = malloc(sizeof(list_node));
	if (new_node == NULL)
		return LIST_NODE_ALLOCATION_ERROR;

	new_node->_data = data;
	new_node->_prev = list->_last;
	new_node->_next = NULL;
	list->_last->_next = new_node;
	list->_last = new_node;
	list->_count++;

	return 0;
}

/**
* list_pop_back - deletes the last element of list
* @vector - vector
*
* Errors
* LIST_IS_NULL	(0x121)									if list is NULL
* LIST_IS_EMPTY(0x122)									if list is empty
*/
int list_pop_back(list * list)
{
	if (list == NULL) {
		return LIST_IS_NULL;
	}
	if (list_is_empty(list)) {
		return LIST_IS_EMPTY;
	}
	if (list->_count == 1) {
		return list_clear(list);
	}
	if (list->_count == 2) {
		free(list->_last);
		list->_last = list->_top;
		list->_top->_next = NULL;
		list->_count--;
		return 0;
	}

	list_iterator new_last_node = list->_last->_prev;
	new_last_node->_next = NULL;

	if (!list->_is_primitive_type)
		list->_finalizer(list->_last);
	free(list->_last);

	list->_last = new_last_node;
	list->_count--;

	return 0;
}

/**
* list_insert_it - inserts new item to the list after specified iterator
*
* @list - list
* @position - iterator representing the node after which the
* 				new element will be added.
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
* LIST_NODE_ALLOCATION_ERROR	(0x123)					if list's node allocation cannot be realized
* INVALID_DATA					(0x125)					if data is NULL
* POSITION_NODE_IS_NULL		(0x127)					if given position iterator is NULL
*/
int list_insert_it(list * list, list_iterator position, void * data)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (data == NULL)
		return INVALID_DATA;
	if (position == NULL)
		return POSITION_NODE_IS_NULL;

	list_iterator find_node = list->_top;

	while (find_node != position)
		find_node = find_node->_next;

	list_iterator new_node = malloc(sizeof(list_node));
	if (new_node == NULL)
		return LIST_NODE_ALLOCATION_ERROR;

	new_node->_data = data;
	new_node->_next = find_node->_next;
	new_node->_prev = find_node;
	find_node->_next->_prev = new_node;
	find_node->_next = new_node;
	list->_count++;

	return 0;
}

/**
* list_insert_po - inserts new item to the list at the specified index
*
* @list - list
* @position - index
* @data - data
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
* LIST_NODE_ALLOCATION_ERROR	(0x123)					if list's node allocation cannot be realized
* INVALID_DATA					(0x125)					if data is NULL
* POSITION_OUT_OF_RANGE		(0x126)					if given position is greater than _count
*/
int list_insert_po(list * list, unsigned int position, void * data)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (data == NULL)
		return INVALID_DATA;
	if (position > list->_count)
		return POSITION_OUT_OF_RANGE;
	if (position == 0)
		return list_push_front(list, data);
	if (position == list->_count)
		return list_push_back(list, data);

	list_iterator existing_node = list_at(list, position), new_node = malloc(sizeof(list_node));
	if (new_node == NULL)
		return LIST_NODE_ALLOCATION_ERROR;

	new_node->_data = data;
	new_node->_next = existing_node;
	new_node->_prev = existing_node->_prev;	
	existing_node->_prev->_next = new_node;
	existing_node->_prev = new_node;

	list->_count++;

	return 0;
}

/**
* list_sort - sorts the list
*
* @list - list
*
* Errors
* LIST_IS_NULL	(0x121)									if list is NULL
* LIST_IS_EMPTY(0x122)									if list is empty
*/
int list_sort(list * List)
{
	if (List == NULL)
		return LIST_IS_NULL;
	if (list_is_empty(List))
		return LIST_IS_EMPTY;

	/*
	must be
	*/
	return 0;
}

/**
* erase_it - deletes the item with the given position
*
* @list - list
* @position - iterator representing the node which will be deleted.
*
* Errors
* LIST_IS_NULL				(0x121)						if list is NULL
* LIST_IS_EMPTY			(0x122)						if list is empty
* POSITION_NODE_IS_NULL	(0x127)						if position iterator is NULL
*/
int list_erase_it(list * list, list_iterator position)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (list_is_empty(list))
		return LIST_IS_EMPTY;
	if (position == NULL)
		return POSITION_NODE_IS_NULL;
	if (list->_count == 1)
		return list_clear(list);
	if (position == list->_top)
		return list_pop_front(list);
	if (position == list->_last)
		return list_pop_back(list);

	list_iterator find_node = list->_top;

	while (find_node != position)
		find_node = find_node->_next;

	find_node->_next->_prev = find_node->_prev;
	find_node->_prev->_next = find_node->_next;

	if (!list->_is_primitive_type)
		list->_finalizer(find_node->_data);
	free(find_node);

	list->_count--;

	return 0;
}

/**
* list_erase_po - deletes the item with the given index
*
* @list - list
* @position - index of element
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
* LIST_IS_EMPTY				(0x122)					if list is empty
* POSITION_OUT_OF_RANGE		(0x126)					if given position is greater than _count
*/
int list_erase_po(list * list, unsigned int position)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (list_is_empty(list))
		return LIST_IS_EMPTY;
	if (position > list->_count)
		return POSITION_OUT_OF_RANGE;
	if (list->_count == 1)
		return list_clear(list);
	if (position == 0)
		return list_pop_front(list);
	if (position == list->_count)
		return list_pop_back(list);

	list_iterator erase_node = list_at(list, position);

	erase_node->_next->_prev = erase_node->_prev;
	erase_node->_prev->_next = erase_node->_next;

	if (!list->_is_primitive_type)
		list->_finalizer(erase_node->_data);
	free(erase_node);

	list->_count--;

	return 0;
}

/**
* list_clear - clears list
* @list - list
*
* Errors
* LIST_IS_NULL					(0x121)					if list is NULL
*/
int list_clear(list * list)
{
	if (list == NULL)
		return LIST_IS_NULL;
	if (list_is_empty(list))
		return 0;

	list_iterator delete_node;

	while (list->_top != NULL) {
		delete_node = list->_top;
		list->_top = list->_top->_next;
		if (!list->_is_primitive_type)
			list->_finalizer(delete_node->_data);
		free(delete_node);
	}

	list->_top = list->_last = NULL;
	list->_count = 0;

	return 0;
}

/**
* list_is_empty - checks if the given list is empty
* @list - list
*/
int list_destroy(list * list)
{
	if (list == NULL)
		return 0;

	list_clear(list);

	free(list);
	list = NULL;

	return 0;
}

/**
* list_is_empty - checks if the given list is empty
* @list - list
*/
bool list_is_empty(list * list)
{
	if (list->_top == NULL || list->_count < 1)
		return true;
	else
		return false;
}

/**
* list_begin - gets the beginning iterator of list
* @list - list
*/
list_iterator list_begin(list * list)
{
	if (list == NULL)
		return NULL;
	return list->_top;
}

/**
* list_end - gets the iterator indicating the end of the list
* @list - list
*/
list_iterator list_end(list * list)
{
	if (list == NULL)
		return NULL;
	return list->_last;
}

/**
* list_move_next - moves iterator to the next element
* @iterator - list iterator
*/
int list_move_next(list_iterator iterator)
{
	if (iterator = NULL)
		return ITERATOR_IS_NULL;

	iterator = iterator->_next;

	return 0;
}

/**
* list_move_prev - moves iterator to the previous element
* @iterator - list iterator
*/
int list_move_prev(list_iterator iterator)
{
	if (iterator = NULL)
		return ITERATOR_IS_NULL;

	iterator = iterator->_prev;

	return 0;
}
