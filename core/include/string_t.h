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

#define STRING_IS_NULL 					0x200
#define STRING_OUT_OF_RANGE 			0x201
#define STRING_BASE_IS_NULL 			0x202
#define STRING_CMP_LEFT_IS_NULL 		0x203
#define STRING_CMP_RIGHT_IS_NULL		0x204
#define STRING_ALLOCATION_ERROR 		0x205

/** structure for ASCII strings **/
typedef struct string_structure
{
	unsigned int _length;
	unsigned int _capacity;
	char* _buffer;
}string_t;

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
string_t* string_t_concat(string_t* left, string_t* right);

#endif
