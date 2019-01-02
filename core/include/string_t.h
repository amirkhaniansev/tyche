/*
 * GNU General Public License Version 3.0, 29 June 2007
 * Header file of string_t.
 * Copyright (C) 2018  Sevak Amirkhanian
 * Email: amirkhanyan.sevak@gmail.com
 * For full notice please see https://github.com/amirkhaniansev/tyche/tree/master/LICENSE.
 */

#ifndef __STRING_T_H__
#define __STRING_T_H__

#include <stdbool.h>

#define STRING_IS_NULL 						0x200
#define STRING_OUT_OF_RANGE 				0x201
#define STRING_BASE_IS_NULL 				0x202
#define STRING_CMP_LEFT_IS_NULL 			0x203
#define STRING_CMP_RIGHT_IS_NULL			0x204
#define STRING_ALLOCATION_ERROR 			0x205
#define STRING_IS_ALREADY_INITIALIZED		0x206
#define STRING_SYMBOL_NOT_FOUND				0x207
#define STRING_INDEX_OF_CHAR_COUNT_ERROR	0x208

/** structure for ASCII strings **/
typedef struct string_structure
{
	unsigned int _length;
	unsigned int _capacity;
	char* _buffer;
}string_t;

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
int string_t_init(string_t* string);

/**
 * string_t_create - creates new string from the given c_string
 * @c_string - C-style null-terminated string
 *
 * Returns NULL if the string creation is impossible.
 */
string_t* string_t_create_c(const char* c_string);

/**
 * string_t_create_arr - creates new string from the given char array and
 * 						 length of the array
 * @array - char array
 * @length - length of array
 *
 * Returns NULL if the arguments are not valid.
 */
string_t* string_t_create_arr(const char array[], unsigned int length);

/**
 * string_t_create_copy - creates new string from the given string
 * @string - string
 */
string_t* string_t_create_copy(const string_t* string);

/**
 * string_t_at - gets the char of the given index
 *
 * @string - string
 * @index - index of the char
 *
 * Returns symbol of the given index.
 */
char string_t_at(const string_t* string, unsigned int index);

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
int string_t_compare(const string_t* left,const string_t* right);

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
int string_t_compare_c_string(const string_t* string, const char* c_string);

/**
 * string_t_clear - makes string an empty string
 * @string - string
 */
int string_t_clear(string_t* string);

/**
 * string_t_contains - checks if string contains the given symbol
 * 
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if the string contains the given symbol and
 * 		false otherwise.
 */
bool string_t_contains(const string_t* string, const char symbol);

/**
 * string_t_concat - concatenates 2 strings
 * @left - left string
 * @right - right string
 * 
 * Returns the concatenation of 2 given strings if everything is OK,
 * 		otherwise the result will be NULL. 
 */
string_t* string_t_concat(string_t* left,string_t* right);

/**
 * string_t_concat_c - concatenates string and C-string
 * @string - string
 * @c_string - C-style null-terminated string
 * 
 * Returns the concatenation of 2 given strings if everything is OK,
 * 		otherwise the result will be NULL. 
 */
string_t* string_t_concat_c(string_t* string,char* c_string);

/**
 * string_t_to_c - converts the given string to C-style null-terminated string
 * @string - string
 * Returns C-string if everything is OK otherwise the result is NULL.
 */
char* string_t_to_c(const string_t* string);

/**
 * string_t_starts_with_char - checks if the string starts with the given symbol
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if string starts with the given symbol, false otherwise.
 */
bool string_t_starts_with_char(const string_t* string, const char symbol);

/**
 * string_t_starts_with_chars - checks if the string starts with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string starts with the given symbols, false otherwise
 */
bool string_t_starts_with_chars(const string_t* string, const char* symbols);

/**
 * string_t_starts_with_chars - checks if the string starts with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string starts with the given symbols, false otherwise
 */
bool string_t_starts_with_string(const string_t* string, const string_t* symbols);

/**
 * string_t_ends_with_char - checks if the string ends with the given symbol
 * @string - string
 * @symbol - symbol
 * 
 * Returns true if string ends with the given symbol, false otherwise.
 */
bool string_t_ends_with_char(const string_t* string, const char symbol);

/**
 * string_t_ends_with_chars - checks if the string ends with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string ends with the given symbols, false otherwise
 */
bool string_t_ends_with_chars(const string_t* string, const char* symbols);

/**
 * string_t_ends_with_chars - checks if the string ends with the given symbols
 * @string - string
 * @symbols - symbols
 * 
 * Returns true if string ends with the given symbols, false otherwise
 */
bool string_t_ends_with_string(const string_t* string, const string_t* symbols);

/**
 * string_t_equal - checks if 2 strings are equal
 * @left - left string
 * @right - right string
 * 
 * Returns true if 2 strings are equal and false otherwise.
 */
bool string_t_equal(const string_t* left, const string_t* right);

/**
 * string_t_equal - checks if 2 strings are equal
 * @left - left string
 * @right - right C-style null-terminated string
 * 
 * Returns true if 2 strings are equal and false otherwise.
 */
bool string_t_equal_c(const string_t* left, const string_t* right);

/**
 * string_t_index_of - returns the index of first occurrence of the given 
 * 			symbol in the string
 * @string - string
 * @symbol - symbol
 * 
 * Returns 	STRING_IS_NULL			if string is null
 * 			STRING_SYMBOL_NOT_FOUND	if symbol is not found
 */
unsigned int string_t_index_of(const string_t* string, const char symbol);

/**
 * string_t_index_of_any - returns the index of the first occurrence of the given
 * 		symbols in the string. Note that ellipsis arguments must be chars.
 * @string - string
 * @amount - amount of chars
 * @... - char arguments
 * 
 * Returns 	STRING_IS_NULL			if string is null
 * 			STRING_SYMBOL_NOT_FOUND	if none of symbols is found
 */
unsigned int string_t_index_of_any(const string_t* string,unsigned int amount,...);

#endif
