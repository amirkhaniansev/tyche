#include <stdio.h>
#include <assert.h>
#include <string.h>

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

	return 0;
}
