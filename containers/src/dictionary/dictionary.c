/**
 * GNU General Public License Version 3.0, 29 June 2007
 * dictionary
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


#include "../../include/dictionary.h"
#include "dictionary_internal.h"

struct dictionary {
	int* _buckets;
	int _count;
	int _size;
	int _version;
	int _free_list;
	int _free_count;
	size_t _key_size;
	size_t _value_size;
	bool _km_alloc;
	bool _vm_alloc;
	entry_t* _entries;
	key_comparator _key_cmp;
	key_hasher _key_h;
	key_finalizer _key_f;
	value_finalizer _value_f;
};

int find_entry(dictionary_t* dictionary, void* key);

int insert(dictionary_t* dictionary, void* key, void* value, bool add);

void resize(dictionary_t* dictionary);

void resize_s(dictionary_t* dictionary, int new_size, bool force_new_hash_codes);

void free_entries(dictionary_t* dictionary);

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
	value_finalizer value_f)
{
	if (data_size <= 0 || 
		key_size <= 0 || 
		capacity < 0 ||
		key_cmp == NULL ||
		key_h == NULL ||
		(!key_manually_allocated && key_f == NULL) ||
		(!value_manually_allocated && value_f == NULL))
		return NULL;

	dictionary_t* dictionary = malloc(sizeof(dictionary_t));
	if (dictionary == NULL)
		return NULL;
	
	int size = get_prime(capacity);
	
	dictionary->_buckets = malloc(size * sizeof(int));
	if (dictionary->_buckets == NULL) {
		free(dictionary);
		return NULL;
	}

	dictionary->_entries = malloc(size * sizeof(entry_t));
	if (dictionary->_entries == NULL) {
		free(dictionary->_buckets);
		free(dictionary);
		return NULL;
	}

	dictionary->_size = size;
	for (int i = 0; i < dictionary->_size; i++) {
		dictionary->_buckets[i] = -1;
		dictionary->_entries[i] = (entry_t){
			._hash_code = 0,
			._next = 0,
			._key = NULL,
			._value = NULL
		};
	}

	dictionary->_count = 0;
	dictionary->_free_count = 0;
	dictionary->_free_list = -1;
	dictionary->_version = 0;
	dictionary->_key_cmp = key_cmp;
	dictionary->_key_f = key_f;
	dictionary->_value_f = value_f;
	dictionary->_key_h = key_h;
	dictionary->_key_size = key_size;
	dictionary->_value_size = data_size;
	dictionary->_vm_alloc = value_manually_allocated;
	dictionary->_km_alloc = key_manually_allocated;

	return dictionary;
}

/**
 * dictionary_count - gets the count of dictionary
 * @dictionary - dictionary
 * Returns the count of dictionary if dictionary is not NULL, -1 otherwise.
 */
int dictionary_count(dictionary_t* dictionary)
{
	if (dictionary == NULL)
		return -1;

	return dictionary->_count - dictionary->_free_count;
}

/**
 * dictionary_get - gets the value with the given key
 * @dictionary - dictionary
 * @key - key
 * @Returns the value if the specified key exists, otherwise NULL.
 */
void* dictionary_get(dictionary_t* dictionary, void* key)
{
	int i = find_entry(dictionary, key);
	if (i > 0)
		return dictionary->_entries[i]._value;

	return NULL;
}

/** dictinary_set - sets the value with the given key
 * @dictionary - dictionary
 * @key - key
 * @data- data
 */
void dictionary_set(dictionary_t* dictionary, void* key, void* data)
{
	insert(dictionary, key, data, false);
}

/**
 * dictionary_keys - gets the collection of dictionary keys
 * @dictionary - dictionary
 */
keys_t dictionary_keys(dictionary_t* dictionary)
{
	keys_t keys;

	if(dictionary == NULL) {
		keys._keys = NULL;
		return keys;
	}

	keys._keys = malloc(dictionary_count(dictionary) * sizeof(void*));
	if(keys._keys == NULL)
		return keys;

	for(int i = 0; i < dictionary->_count; i++)
		if(dictionary->_entries[i]._hash_code >= 0)
			keys._keys[i] = dictionary->_entries[i]._key;

	return keys;
}

/**
 * dictionary - gets the collection of dictionary values
 * dictionary - dictionary
 */
values_t dictionary_values(dictionary_t* dictionary)
{
	values_t values;

	if(dictionary == NULL) {
		values._values = NULL;
		return values;
	}

	values._values = malloc(dictionary_count(dictionary) * sizeof(void*));
	if(values._values == NULL)
		return values;

	for(int i = 0; i < dictionary->_count; i++)
		if(dictionary->_entries[i]._hash_code >= 0)
			values._values[i] = dictionary->_entries[i]._value;

	return values;
}

/**
 * dictionary_contains - checks whether the dictionary contains the key
 * @dictionary - dictionary
 * @key - key
 */
bool dictionary_contains(dictionary_t* dictionary, void* key)
{
	return find_entry(dictionary, key) >= 0;
}

/**
 * dictionary_add - adds the given key and value to the dictionary if the
 *				key does not exist
 * @dictionary - dictionary
 * @key - key
 * @value - value
 * Returns error code.
 */
int dictionary_add(dictionary_t* dictionary, void* key, void* data)
{
	return insert(dictionary, key, data, true);
}

/**
 * dictionary_remove - removes the value with the given key from dictionary if it exists
 * @dictionary - dictionary
 * @key - key
 * Returns true if remove operation is successful, otherwise NULL.
 */
bool dictionary_remove(dictionary_t* dictionary, void* key)
{
	if(dictionary == NULL || dictionary->_buckets == NULL || key == NULL)
		return false;

	int hash_code = (dictionary->_key_h(key)) & 0x7FFFFFFF;
	int bucket = hash_code % dictionary->_size;
	int last = -1;
	int* bs = dictionary->_buckets;
	entry_t* es = dictionary->_entries;

	for(int i = bs[bucket]; i >= 0; last = i, i = es[i]._next) {
		if(es[i]._hash_code == hash_code && dictionary->_key_cmp(es[i]._key, key)) {
			if(last < 0)
				bs[bucket] = es[i]._next;
			else es[last]._next = es[i]._next;

			es[i]._hash_code = -1;
			es[i]._next = dictionary->_free_list;
			free_entry_key(dictionary, es[i]);
			free_entry_value(dictionary, es[i]);
			dictionary->_free_list = i;
			dictionary->_free_count++;
			dictionary->_version++;

			return true;
		}
	}

	return false;
}

/**
 * dictionary_clear - clears the dictionary
 * @dictionary - dictionary
 */
void dictionary_clear(dictionary_t* dictionary)
{
	if(dictionary == NULL || dictionary->_count == 0)
		return;

	for(int i = 0; i < dictionary->_size; i++)
		dictionary->_buckets[i] = -1;

	for(int i = 0; i < dictionary->_count; i++) {
		if(dictionary->_entries[i]._hash_code >= 0) {
			free_entry_key(dictionary, dictionary->_entries[i]);
			free_entry_value(dictionary, dictionary->_entries[i]);
		}
		dictionary->_entries[i]._hash_code = 0;
		dictionary->_entries[i]._next = 0;
	}

	dictionary->_free_list = -1;
	dictionary->_count = 0;
	dictionary->_free_count = 0;
	dictionary->_version++;
}

/**
 * dictionary_destroy - destroys dictionary
 * @dictionary to be destroyed.
 * Returns error code.
 */
int dictionary_destroy(dictionary_t* dictionary)
{
	if(dictionary == NULL)
		return DICTIONARY_IS_NULL;

	dictionary_clear(dictionary);
	free(dictionary->_buckets);
	free(dictionary->_entries);
	free(dictionary);

	return DICTIONARY_SUCCESS;
}

int insert(dictionary_t* dictionary, void* key, void* value, bool add)
{
	if (dictionary == NULL)
		return DICTIONARY_IS_NULL;
	if (key == NULL)
		return DICTIONARY_KEY_IS_NULL;
	if (value == NULL)
		return DICTIONARY_VALUE_IS_NULL;

	if(dictionary->_buckets == NULL) {
		int size = get_prime(0);
		dictionary->_size = size;

		dictionary->_buckets = malloc(size * sizeof(int));	
		if(dictionary->_buckets == NULL)
			return DICTIONARY_ALLOCATION_ERROR;
		for(int i = 0; i < size; i++)
			dictionary->_buckets[i] = -1;
		
		free_entries(dictionary);
		dictionary->_entries = malloc(size * sizeof(entry_t));
		if(dictionary->_entries == NULL) {
			free(dictionary->_buckets);
			return DICTIONARY_ALLOCATION_ERROR;
		}

		dictionary->_free_list = -1;
	}

	int hash_code = dictionary->_key_h(key) & 0x7FFFFFF;
	int target_bucket = hash_code % dictionary->_size;
	int* bs = dictionary->_buckets;
	entry_t* es = dictionary->_entries;

	for (int i = bs[target_bucket]; i >= 0; i = es[i]._next) {
		if (es[i]._hash_code == hash_code && dictionary->_key_cmp(es[i]._key, key)) {
			if (add == true)
				return DICTIONARY_ALREADY_EXISTS;
			
			free_entry_value(dictionary, es[i]);
			
			es[i]._value = value;
			dictionary->_version++;	
			return DICTIONARY_SUCCESS;
		}
	}

	int index = 0;
	if(dictionary->_free_count > 0) {
		index = dictionary->_free_list;
		dictionary->_free_list = es[index]._next;
		dictionary->_free_count--;
	}
	else {
		if(dictionary->_count == dictionary->_size) {
			resize(dictionary);
			target_bucket = hash_code % dictionary->_size;
		}
		index = dictionary->_count;
		dictionary->_count++;
	}

	es[index] = (entry_t) {
		._hash_code = hash_code,
		._next = dictionary->_buckets[target_bucket],
		._key = key,
		._value = value
	};
	dictionary->_buckets[target_bucket] = index;
	dictionary->_version++;

	return DICTIONARY_SUCCESS;
}

int find_entry(dictionary_t* dictionary, void* key)
{
	if(dictionary == NULL || dictionary->_buckets == NULL || key == NULL)
		return -1;

	int hash_code = dictionary->_key_h(key) & 0x7FFFFFFF;
	int* bs = dictionary->_buckets;
	entry_t* es = dictionary->_entries;

	for(int i = bs[hash_code % dictionary->_size]; i >=0; i = es[i]._next)
		if(es[i]._hash_code == hash_code && dictionary->_key_cmp(es[i]._key, key))
			return i;

	return -1;
}

void resize_s(dictionary_t* dictionary, int new_size, bool force_new_hash_codes)
{
	int* new_buckets = malloc(new_size * sizeof(int));
	for(int i = 0; i < new_size; i++)
		new_buckets[i] = -1;

	entry_t* new_entries = malloc(new_size * sizeof(entry_t));
	for(int i = 0; i < dictionary->_count; i++)
		new_entries[i] = dictionary->_entries[i];

	int hash = 0;
	if(force_new_hash_codes) {
		for(int i = 0; i < dictionary->_count; i++) {
			if(new_entries[i]._hash_code != -1) {
				hash = dictionary->_key_h(new_entries[i]._key);
				new_entries[i]._hash_code = hash & 0x7FFFFFFF;
			}
		}
	}

	int bucket = 0;
	for(int i = 0; i < dictionary->_count; i++) {
		if(new_entries[i]._hash_code >= 0) {
			bucket = new_entries[i]._hash_code % new_size;
			new_entries[i]._next = new_buckets[bucket];
			new_buckets[bucket] = 1;
		}
	}

	free(dictionary->_buckets);
	free(dictionary->_entries);

	dictionary->_size = new_size;
	dictionary->_buckets = new_buckets;
	dictionary->_entries = new_entries;
}

void resize(dictionary_t* dictionary)
{
	resize_s(dictionary, expand_prime(dictionary->_count), false);
}

void free_entries(dictionary_t* dictionary)
{
	entry_t* es = dictionary->_entries;
	for(int i = 0; i < dictionary->_size; i++) {
		if(es[i]._hash_code >= 0) {
			free_entry_key(dictionary, es[i]);
			free_entry_value(dictionary, es[i]);
		}
	}

	free(es);
}
