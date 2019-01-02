/*
 * GNU General Public License Version 3.0, 29 June 2007
 * Source file file of string type.
 * Copyright (C) 2018  Sevak Amirkhanian
 * Email: amirkhanyan.sevak@gmail.com
 * For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
 */

#include <stddef.h>
#include <string.h>
#include <stdlib.h>

#include "../include/string_t.h"

static inline unsigned int get_power_of_2(const unsigned int num)
{
	unsigned int  i;
	for (i = 2; i <= num; i *= 2);
	return i;
}

static inline void populate_string(const string_t* string,
		const char* c_string,
		unsigned int len,
		unsigned int start_index)
{
	unsigned int i = start_index;

	for(; i < len; i++)
		string->_buffer[i] = c_string[i];
	
	string->_buffer[i] = '\0';
}

static inline string_t* string_t_allocate(unsigned int len, unsigned int capacity)
{
	string_t* string = malloc(sizeof(string_t));

	if(string == NULL)
		return NULL;

	string->_length = len;
	string->_capacity = capacity;
	string->_buffer = malloc(string->_capacity * sizeof(char));

	if(string->_buffer == NULL){
		free(string);
		return NULL;
	}

	return string;
}

static inline string_t* string_t_create(const char* c_string,
		unsigned int len,
		unsigned int capacity)
{
	if(c_string == NULL || len <= 0 || capacity <= 0)
		return NULL;

	string_t* string = string_t_allocate(len, capacity);

	if(string == NULL)
		return NULL;

	populate_string(string, c_string, len, 0);

	return string;
}

/**
 * string_t_create - creates new string from the given c_string
 * @c_string - C-style null-terminated string
 *
 * Returns NULL if the string creation is impossible.
 */
string_t* string_t_create_c(const char* c_string)
{
	if(c_string == NULL)
		return NULL;

	unsigned int len = strlen(c_string);
	unsigned int cap = get_power_of_2(len);

	return string_t_create(c_string, len, cap);
}

/**
 * string_t_create_arr - creates new string from the given char array and
 * 						 length of the array
 * @array - char array
 * @length - length of array
 *
 * Returns NULL if the arguments are not valid.
 */
string_t* string_t_create_arr(const char array[], unsigned int length)
{
	return string_t_create(array, length, get_power_of_2(length));
}

/**
 * string_t_create_copy - creates new string from the given string
 * @string - string
 */
string_t* string_t_create_copy(const string_t* string)
{
	if(string == NULL)
		return NULL;

	return string_t_create(string->_buffer, string->_length, string->_capacity);	
}

/**
 * string_t_at - gets the char of the given index
 *
 * @string - string
 * @index - index of the char
 *
 * Returns symbol of the given index.
 */
char string_t_at(const string_t* string,const unsigned int index)
{
	if(string == NULL || index <= 0 || index >= string->_length)
		return 0;

	return string->_buffer[index];
}

/**
 * string_t_compare - compares 2 strings
 * @left - left string
 * @right - right string
 *
 * Returns		 1 			if left > right
 * 				-1			if left < right
 * 				 0 			if left == right
 * 				 error 		otherwise
 */
int string_t_compare(const string_t* left,const string_t* right)
{
	if(left == right)
		return 0;

	if(left == NULL)
		return STRING_CMP_LEFT_IS_NULL;

	if(right == NULL)
		return STRING_CMP_RIGHT_IS_NULL;
	
	return strcmp(left->_buffer, right->_buffer);
}

/**
 * string_t_compare_c_string - compares string and C-style string
 * @string - string
 * @c_string - null-terminated C-string
 *
 * Returns		 1 			if left > right
 * 				-1			if left < right
 * 				 0 			if left == right
 * 				 error 		otherwise
 */
int string_t_compare_c_string(const string_t* string, const char* c_string)
{
	if(string == NULL)
		return STRING_CMP_LEFT_IS_NULL;
	
	if(string->_buffer == c_string)
		return 0;
	
	if(c_string == NULL)
		return STRING_CMP_RIGHT_IS_NULL;

	return strcmp(string->_buffer, c_string);	
}

/**
 * string_t_clear - makes string an empty string
 * @string - string
 * 
 * Returns 0 if clear is successful.
 */
int string_t_clear(string_t* string)
{
	if(string == NULL)
		return STRING_IS_NULL;

	string->_capacity = 2;
	string->_length = 0;
	
	free(string->_buffer);

	string->_buffer = malloc(string->_capacity * sizeof(char));

	if(string->_buffer == NULL)
		return STRING_ALLOCATION_ERROR;

	string->_buffer[0] = '\0';

	return 0;
}

/**
 * string_t_contains - checks if string contains the given symbol
 * 
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if the string contains the given symbol and
 * 		false otherwise.
 */
bool string_t_contains(const string_t* string, const char symbol)
{
	if(string == NULL)
		return false;

	for(int i = 0; i < string->_length; i++)
		if(string->_buffer[i] == symbol)
			return true;

	return false;
}

/**
 * string_t_concat - concatenates 2 strings
 * @left - left string
 * @right - right string
 * 
 * Returns the concatenation of 2 given strings if everything is OK,
 * 		otherwise the result will be NULL. 
 */
string_t* string_t_concat(string_t* left, string_t* right)
{
	if(left == NULL)
		return right;
	
	if(right == NULL)
		return left;

	unsigned int result_len = left->_length + right->_length;

	string_t* result = string_t_allocate(result_len, get_power_of_2(result_len));

	populate_string(result, left->_buffer, left->_length, 0);
	populate_string(result, right->_buffer, result->_length, left->_length);

	return result;
}
