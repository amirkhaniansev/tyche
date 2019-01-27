#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <stddef.h>
#include <limits.h>
#include <unistd.h>

#include "dictionary.h"

typedef struct
{
	int x;
	int y;
} A;

static int hash(const void* x)
{
	A* a = (A*)x;
	return a->x ^ a->y;
}
static bool compare(const void* x, const void* y)
{
	A* a = (A*)x;
	A* b = (A*)y;
	return a->x == b->x && a->y == b->y;
}

int main(int argc, char** argv)
{
	dictionary_t* d = dictionary_create(
			10,
			sizeof(int),
			sizeof(int),
			false,
			false,
			&compare,
			&hash,
			&free,
			&free
	);

	for(int i = 0; i < 4000; i++) {
		A* ptr0 = malloc(sizeof(A));
		ptr0->x = i;
		ptr0->y = i+1;
		int* ptr1 = malloc(sizeof(int));
		*ptr1 = i;
		dictionary_add(d, ptr0, ptr1);

	}

	dictionary_destroy(d);

	return 0;
}
