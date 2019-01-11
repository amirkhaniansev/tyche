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
#include <stdarg.h>
#include <math.h>

#include "../include/string_t.h"

static inline int sgn(int num)
{
	return (num > 0) - (num < 0);
}

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

	unsigned int i = start_index, j = 0;

	while(i < len)
		string->_buffer[i++] = c_string[j++];
	
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

static inline bool string_t_is_initialized(const string_t* string)
{
	if(string->_buffer != NULL &&
	   string->_length > 0 &&
	   string->_capacity >= 2 &&
	   (string->_capacity & (string->_capacity - 1)) != 0)
	   return true;
	
	return false;
}

static inline bool string_t_starts_with(const string_t* string, 
		const char* symbols,
		unsigned int len)
{
	if(string == NULL || string->_buffer == NULL)
		return false;

	if(len > string->_length)
		return false;

	unsigned int i = 0, j = 0;

	while(i < len)
		if(symbols[i++] != string->_buffer[j++])
			return false;
	
	return true;
}

static inline bool string_t_ends_with(const string_t* string, 
		const char* symbols,
		unsigned int len)
{
	if(string == NULL || string->_buffer == NULL)
		return false;

	if(len > string->_length)
		return false;

	unsigned int i =  len - 1, j = string->_length - 1;

	while(i)
		if(symbols[i--] != string->_buffer[j--])
			return false;	
	
	return true;
}

static inline int string_t_resize(string_t* string)
{
	char* new_buffer = malloc(2 * string->_capacity * sizeof(char));

	if(new_buffer == NULL)
		return STRING_BASE_IS_NULL;

	for(unsigned int i = 0; i < string->_length; i++)
		new_buffer[i] = string->_buffer[i];

	free(string->_buffer);
	string->_buffer = new_buffer;
	string->_capacity *= 2;
	return 0;
}

static inline void string_t_destroy_internal(string_t* string, bool is_on_stack)
{
	if(string == NULL)
		return;

	if(string->_buffer != NULL)
		free(string->_buffer);
	
	if(!is_on_stack)
		free(string);
}

/**
 * string_t_init - initializes already created string. 
 * 			This function is for scenarios when you want to create string
 * 			object on stack
 * @string - string
 * 
 * Returns 	0 								if initialization is successfull
 * 			STRING_IS_NULL 					if the given string is null
 * 			STRING_IS_ALREADY_INITIALIZED 	if the given string is already initialized
 * 			STRING_ALLOCATION_ERROR			if allocation error happened	
 */
int string_t_init(string_t* string)
{
	if(string == NULL)
		return STRING_IS_NULL;
	
	if(string_t_is_initialized(string))
		return STRING_IS_ALREADY_INITIALIZED;
	
	if(string->_buffer != NULL)
		free(string->_buffer);

	string->_length = 0;
	string->_capacity = 2;
	string->_buffer = malloc(string->_capacity * sizeof(char));

	if(string->_buffer == NULL)
		return STRING_ALLOCATION_ERROR;
	
	string->_buffer[0] = '\0';

	return 0;
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
	
	return sgn(strcmp(left->_buffer, right->_buffer));
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

	return sgn(strcmp(string->_buffer, c_string));	
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

/**
 * string_t_concat_c - concatenates string and C-string
 * @string - string
 * @c_string - C-style null-terminated string
 * 
 * Returns the concatenation of 2 given strings if everything is OK,
 * 		otherwise the result will be NULL. 
 */
string_t* string_t_concat_c(string_t* string,char* c_string)
{
	if(string == NULL)
		return string_t_create_c(c_string);

	if(c_string == NULL)
		return string;

	unsigned int result_len = string->_length + strlen(c_string);

	string_t* result = string_t_allocate(result_len, get_power_of_2(result_len));

	populate_string(result, string->_buffer, string->_length, 0);
	populate_string(result, c_string, result->_length, string->_length);

	return result; 	
}

/**
 * string_t_to_c - converts the given string to C-style null-terminated string
 * @string - string
 * Returns C-string if everything is OK otherwise the result is NULL.
 */
char* string_t_to_c(const string_t* string)
{
	if(string == NULL)
		return NULL;

	char* result = malloc((string->_length + 1) * sizeof(char));

	if(result == NULL)
		return NULL;

	for(unsigned int i = 0; i < string->_length; i++)
		result[i] = string->_buffer[i];
	
	result[string->_length] = '\0';

	return result;
}

/**
 * string_t_starts_with_char - checks if the string starts with the given symbol
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if string starts with the given symbol, false otherwise.
 */
bool string_t_starts_with_char(const string_t* string, const char symbol)
{
	if(string == NULL || string->_buffer == NULL)
		return false;

	return string->_buffer[0] == symbol;
}

/**
 * string_t_starts_with_chars - checks if the string starts with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string starts with the given symbols, false otherwise
 */
bool string_t_starts_with_chars(const string_t* string, const char* symbols)
{
	if(symbols == NULL)
		return false;
	
	return string_t_starts_with(string, symbols, strlen(symbols));
}

/**
 * string_t_starts_with_chars - checks if the string starts with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string starts with the given symbols, false otherwise
 */
bool string_t_starts_with_string(const string_t* string, const string_t* symbols)
{
	if(symbols == NULL || symbols->_buffer == NULL)
		return false;
	
	return string_t_starts_with(string, symbols->_buffer, symbols->_length);
}

/**
 * string_t_ends_with_char - checks if the string ends with the given symbol
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if string ends with the given symbol, false otherwise.
 */
bool string_t_ends_with_char(const string_t* string, const char symbol)
{
	if(string == NULL || string->_buffer == NULL)
		return false;
	
	return string->_buffer[string->_length-1] == symbol;
}

/**
 * string_t_ends_with_chars - checks if the string ends with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string ends with the given symbols, false otherwise
 */
bool string_t_ends_with_chars(const string_t* string, const char* symbols)
{
	if(symbols == NULL)
		return false;
	
	return string_t_ends_with(string, symbols, strlen(symbols));
}

/**
 * string_t_ends_with_chars - checks if the string ends with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string ends with the given symbols, false otherwise
 */
bool string_t_ends_with_string(const string_t* string, const string_t* symbols)
{
	if(symbols == NULL || symbols->_buffer == NULL)
		return false;

	return string_t_ends_with(string, symbols->_buffer, symbols->_length);
}

/**
 * string_t_equal - checks if 2 strings are equal
 * @left - left string
 * @right - right string
 * 
 * Returns true if 2 strings are equal and false otherwise.
 */
bool string_t_equal(const string_t* left, const string_t* right)
{
	return string_t_compare(left, right) == 0;
}

/**
 * string_t_equal - checks if 2 strings are equal
 * @left - left string
 * @right - right C-style null-terminated string
 * 
 * Returns true if 2 strings are equal and false otherwise.
 */
bool string_t_equal_c(const string_t* left, const string_t* right)
{
	if(right == NULL)
		return false;

	return string_t_compare_c_string(left, right->_buffer) == 0;
}

/**
 * string_t_index_of - returns the index of first occurrence of the given 
 * 			symbol in the string
 * @string - string
 * @symbol - symbol
 * 
 * Returns 	STRING_IS_NULL			if string is null
 * 			STRING_SYMBOL_NOT_FOUND	if symbol is not found
 */
int string_t_index_of(const string_t* string, const char symbol)
{
	if(string == NULL)
		return STRING_IS_NULL;

	if(string->_buffer == NULL)
		return STRING_BASE_IS_NULL;
	
	for(unsigned int i = 0; i < string->_length; i++)
		if(string->_buffer[i] == symbol)
			return i;

	return STRING_SYMBOL_NOT_FOUND;
}

/**
 * string_t_index_of_any - returns the index of the first occurrence of the given
 * 		symbols in the string. Note that ellipsis arguments must be chars.
 * @string - string
 * @amount - amount of chars
 * @... - char arguments
 * 
 * Returns 	STRING_IS_NULL						if string is null
 * 			STRING_SYMBOL_NOT_FOUND				if none of symbols is found
 * 			STRING_BASE_IS_NULL					if buffer is null
 *			STRING_INDEX_OF_CHAR_COUNT_ERROR 	if amount of char arguments is not positive
 */
int string_t_index_of_any(const string_t* string,unsigned int amount,...)
{
	if(string == NULL)
		return STRING_IS_NULL;

	if(string->_buffer == NULL)
		return STRING_BASE_IS_NULL;

	if(amount <= 0)
		return STRING_INDEX_OF_CHAR_COUNT_ERROR;

	va_list symbols;
	va_start(symbols, amount);

	unsigned int i = 0;
	bool found = false;
	char symbol;

	while(amount-- && !found) {
		symbol = va_arg(symbols, int);
		for(i = 0; i < string->_length; i++) {
			if(symbol == string->_buffer[i]) {
				found = true;
				break;
			}
		}
	}

	va_end(symbols);
	
	if(found)
		return i;

	return STRING_SYMBOL_NOT_FOUND;			
}

/**
 * string_t_append_char - appends character to given string
 * @string - string
 * @character - character
 */
int string_t_append_char(string_t* string, char character)
{
	if(string == NULL || string->_buffer == NULL)
		return STRING_IS_NULL;
	
	if(string->_length == string->_capacity)
		string_t_resize(string);
	
	string->_buffer[string->_length] = character;
	string->_length++;
	return 0;
}

/**
 * string_t_append_chars - appends the append string to initial string
 * @string - string
 * @append_string - append string
 */
int string_t_append_chars(string_t* string, string_t* append_string)
{
	if(string == NULL || append_string == NULL)
		return STRING_IS_NULL;

	if(string->_buffer == NULL || append_string->_buffer == NULL)
		return STRING_BASE_IS_NULL;

	for(unsigned int i = 0; i < append_string->_length; i++)
		if(string_t_append_char(string, append_string->_buffer[i]) != 0)
			return STRING_APPEND_ERROR;
	
	return 0;
}

/**
 * string_t_to_lower - makes characters of string lowercase
 * @string - string
 */
int string_t_to_lower(string_t* string)
{
	if(string == NULL)
		return STRING_IS_NULL;
	if(string->_buffer == NULL)
		return STRING_BASE_IS_NULL;

	for(unsigned int i = 0; i < string->_length; i++)
		if(string->_buffer[i] > 64 && string->_buffer[i] < 91)
			string->_buffer[i] |= 32;
	
	return 0;
}

/**
 * string_to_upper - makes characters of string uppercase
 * @string - string
 */
int string_t_to_upper(string_t* string)
{
	if(string == NULL)
		return STRING_IS_NULL;
	if(string->_buffer == NULL)
		return STRING_BASE_IS_NULL;

	for(unsigned int i = 0; i < string->_length; i++)
		if(string->_buffer[i] > 96 && string->_buffer[i] < 123)
			string->_buffer[i] &= ~32;

	return 0; 
}

/**
 * string_t_is_substring - checks if the possible substring is actually a substring of
 * 		the given string.
 * 		Impelementation uses Knuth-Morris-Pratt algorithm.
 * 		Complexity O(n + m) 
 * 			where 	n = length of string
 * 					m = length of possible substring
 * @string - string
 * @substring - possible substring
 * Returns true if the possible substring is actually a substring of
 * 		the given string
 */
bool string_t_is_substring(string_t* string, string_t* substring)
{
	if(string == NULL || substring == NULL || 
	   string->_buffer == NULL || substring->_buffer == NULL)
	   return false;

	int* table = malloc(substring->_length * sizeof(int));	
	if(table == NULL)
		return false;

	int pos = 1;
	int current_ci = 0;
	char* word = string->_buffer;
	char* pattern = substring->_buffer;

	table[0] = -1;

	while(pos < string->_length) {
		if(word[pos] == word[current_ci]) {
			table[pos] = table[current_ci];
		}
		else {
			table[pos] = current_ci;
			current_ci = table[current_ci];
			while(current_ci >= 0 && word[pos] != word[current_ci]) {
				current_ci = table[current_ci];
			}  
		}
		pos++;
		current_ci++;
	}

	table[pos] = current_ci; 

	int i = 0, j = 0;

	while(j < string->_length) {
		if(word[j] == pattern[i]) {
			i++;
			j++;
			if(i == substring->_length) {
				free(table);
				return true;
			}
		}
		else {
			i = table[i];
			if(i < 0) {
				i++;
				j++;
			}
		}
	}

	free(table);

	return false;
}

/**
 * string_t_destroy - destroys string_t object
 * @string - string
 * 
 * Returns error code.
 */
void string_t_destroy(string_t* string)
{
	string_t_destroy_internal(string, false);
}

/** 
 * string_t_destroy_static - destroys string_t object allocated on stack
 * @string - string
 * 
 * Returns error code.
 */
void string_t_destroy_s(string_t* string)
{
	string_t_destroy_internal(string, true);
}
