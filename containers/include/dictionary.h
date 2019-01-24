/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Public API of dictionary.
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

#ifndef __DICTIONARY__
#define __DICTIONARY__

#include <stddef.h>
#include <stdbool.h>

#include "../internal/dictionary_types.h"

/* error codes */
#define DICTIONARY_SUCCESS			0x0
#define DICTIONARY_IS_NULL			0x500
#define DICTIONARY_KEY_IS_NULL		0x501
#define DICTIONARY_VALUE_IS_NULL	0x502
#define DICTIONARY_ALREADY_EXISTS	0x503

/**
 * dictionary_create - creates key-value dictionary
 * 
 * @capacity - capacity
 * @data_size - size of value
 * @key_size - size of key
 * @key_manually_allocated - boolean value indicating whether the key is manually allocated
 * @value_manually_allocated - boolean value indicating whether the value is manually allocated
 * @key_cmp - comparator for key
 * @key_h - hasher for key
 * @key_f - finalizer for key
 * @value_f - finalizer for value
 * 
 * Returns dictionary object if everything is OK, otherwise NULL.
 */
dictionary_t* dictionary_create(
	int capacity,
	size_t data_size,
	size_t key_size,
	bool key_manually_allocated,
	bool value_manually_allocated,
	key_comparator key_cmp,
	key_hasher key_h,
	key_finalizer key_f,
	value_finalizer value_f);

/** 
 * dictionary_count - gets the count of dictionary
 * @dictionary - dictionary
 * Returns the count of dictionary
 */
int dictionary_count(dictionary_t* dictionary);

/**
 * dictionary_get - gets the value with the given key
 * @dictionary - dictionary
 * @key - key
 * @Returns the value if the specified key exists, otherwise NULL.
 */
void* dictionary_get(dictionary_t* dictionary, void* key);

/** dictinary_set - sets the value with the given key
 * @dictionary - dictionary
 * @key - key
 * @data- data
 */
void dictionary_set(dictionary_t* dictionary, void* key, void* data);

/**
 * dictionary_keys - gets the collection of dictionary keys
 * @dictionary - dictionary
 */
keys_t* dictionary_keys(dictionary_t* dictionary);

/**
 * dictionary - gets the collection of dictionary values
 * dictionary - dictionary
 */
values_t* dictionary_values(dictionary_t* dictionary);

/** 
 * dictionary_add - adds the given key and value to the dictionary if the
 *				key does not exist
 * @dictionary - dictionary
 * @key - key
 * @value - value
 * Returns error code.
 */
int dictionary_add(dictionary_t* dictionary, void* key, void* data);

/** 
 * dictionary_contains - checks whether the dictionary contains the key
 * @dictionary - dictionary
 * @key - key
 */
bool dictionary_contains(dictionary_t* dictionary, void* key);

/**
 * dictionary_clear - clears the dictionary
 * @dictionary - dictionary
 */
void dictionary_clear(dictionary_t* dictionary);

/** 
 * dictionary_remove - removes the value with the given key from dictionary if it exists
 * @dictionary - dictionary
 * @key - key
 * Returns true if remove operation is successful, otherwise NULL.
 */
bool dictionary_remove(dictionary_t* dictionary, void* key);

/**
 * dictionary_destroy - destroys dictionary
 * @dictionary to be destroyed.
 * Returns error code.
 */
int dictionary_destroy(dictionary_t* dictionary);

#endif  
