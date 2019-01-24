/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Internal helpers for dictionary.
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

#ifndef __DICTIONARY_INTERNAL__
#define __DICTIONARY_INTERNAL__

#include <stdlib.h>
#include <limits.h>
#include <math.h>

#include "../../internal/dictionary_types.h"

#define free_entry_key(dictionary, entry)\
	do {\
		if(dictionary->_km_alloc)\
			free(entry._key);\
		else dictionary->_key_f(entry._key);\
		entry._key = NULL;\
	} while(0)

#define free_entry_value(dictionary, entry) \
	do {\
		if(dictionary->_vm_alloc)\
			free(entry._value);\
		else dictionary->_value_f(entry._value);\
		entry._value = NULL;\
	} while(0)

struct keys
{
	void** _keys;
};

struct values
{
	void** _values;
};

typedef struct entry_ {
	int _hash_code;
	int _next;
	void* _key;
	void* _value;
} entry_t;


static int has_prime = 101;
static int primes_count = 72;
static int max_prime_array_length = 0x7FEFFFFD;
static int primes[] = {
	3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 
	353, 431, 521, 631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371, 
	4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023, 25229, 
	30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
	187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 
	968897, 1162687, 1395263,1674319, 2009191, 2411033, 2893249, 3471899, 
	4166287, 4999559, 5999471, 7199369 
};

bool is_prime(int number)
{
	if ((number & 1) == 0)
		return number == 2;

	int sqr = (int)sqrt(number);
	for (int i = 3; i < sqr; i += 2)
		if (number % i == 0)
			return false;

	return true;
}

int get_prime(int minimum)
{
	for (int i = 0; i < primes_count; i++)
		if (primes[i] >= minimum)
			return primes[i];

	for (int i = (minimum | 1); i < INT_MAX; i++)
		if (is_prime(i) && ((i - 1) % has_prime != 0))
			return i;

	return minimum;
}

int expand_prime(int old_size)
{
	int new_size = 2 * old_size;
	if ((unsigned int)new_size > max_prime_array_length && max_prime_array_length > old_size)
		return max_prime_array_length;
	
	return get_prime(new_size);
}

#endif
