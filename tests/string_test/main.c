/**
 * GNU General Public License Version 3.0, 29 June 2007
 * main
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

#include <stdio.h>
#include <assert.h>
#include <string.h>
#include <stdlib.h>

#include "string_t.h"

void test_at()
{
	string_t* s = string_t_create_c("Hello");
	assert(string_t_at(s, 1) == 'e');
	string_t_destroy(s);
}

void test_clear()
{
	string_t* s = string_t_create_c("Hello");
	int cleared = string_t_clear(s);
	assert(cleared == 0);
	string_t_destroy(s);
}

void test_compare()
{
	string_t* s0 = string_t_create_c("Hello");
	string_t* s1 = string_t_create_c("Hello,Babe");
	assert(string_t_compare(s0,s1) == -1);
	string_t_destroy(s0);
	string_t_destroy(s1);
}

void test_compare_c_string()
{
	string_t* s = string_t_create_c("Hello");
	char* c_s = "Hello, Babe";
	assert(string_t_compare_c_string(s, c_s) == -1);
	string_t_destroy(s);
}

void test_concat()
{
	string_t* s0 = string_t_create_c("Hello");
	string_t* s1 = string_t_create_c("Hello,Babe");
	string_t* s2 = string_t_concat(s1,s0);
	printf("%s %d %d\n", s2->_buffer, s2->_capacity, s2->_length);
	string_t_destroy(s0);
	string_t_destroy(s1);
	string_t_destroy(s2);
}

void test_concat_c_string()
{
	string_t* s0 = string_t_create_c("Hello");
	char* s1 = "Hello,Babe";
	string_t* s2 = string_t_concat_c(s0,s1);
	printf("%s %d %d\n", s2->_buffer, s2->_capacity, s2->_length);
	string_t_destroy(s0);
	string_t_destroy(s2);
}

void test_contains()
{
	string_t* s = string_t_create_c("Hello");
	assert(string_t_contains(s, 'H') == true);
	string_t_destroy(s);
}

void test_to_lower()
{
	string_t* s = string_t_create_c("HELLO");
	string_t_to_lower(s);
	assert(string_t_compare_c_string(s, "hello") == 0);
	string_t_destroy(s);
}

void test_to_upper()
{
	string_t* s = string_t_create_c("hello");
	string_t_to_upper(s);
	assert(string_t_compare_c_string(s, "HELLO") == 0);
	string_t_destroy(s);
}

void test_is_substring()
{
	string_t* w = string_t_create_c("Hello, babe");
	string_t* p = string_t_create_c("babe");
	assert(string_t_is_substring(w,p) == true);
	string_t_destroy(w);
	string_t_destroy(p);
}

void test_big_string()
{
	string_t s;
	string_t_init(&s);
	for(int i = 0; i < 100; i++) {
		char* text = malloc(100001 * sizeof(char));
		for(int i = 0; i < 100000; i++)
			text[i] = 'A';
		text[100000] = '\0';
		string_t* append_text = string_t_create_c(text);
		string_t_append_chars(&s, append_text);
		string_t_destroy(append_text);
		free(text);
	}
	string_t_destroy_s(&s);
}

int main(int argc, char** argv)
{
	test_at();
	test_clear();
	test_compare();
	test_compare_c_string();
	test_concat();
	test_concat_c_string();
	test_contains();
	test_to_lower();
	test_to_upper();
	test_is_substring();
	int i = 0;
	while(i++ < 10)
	test_big_string();

	return 0;
}
