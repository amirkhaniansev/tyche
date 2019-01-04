#include <stdio.h>
#include <assert.h>
#include <limits.h>

#include "stack.h"

int main(int argc, char** argv)
{
	stack* stack = stack_create_p();
	int i = 0, element = 0, limit = INT_MAX / 1000;

	while(i <= limit)
		stack_push(stack, i++);

	for(i = 0; i <= limit; i++) {
		element = stack_top(stack);
		stack_pop(stack);
		printf("stack element %d : %d\n", i, element);
	}

	assert(stack_is_empty(stack) == true);
	assert(stack_destroy(stack) == 0);

	return 0;
}
