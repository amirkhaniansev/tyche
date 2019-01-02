#include <stdio.h>
#include "string_t.h"

int main(int argc, char** argv)
{
	string_t* s = string_t_create_c("Hambal");
	printf("Hambal");
	return 0;
}
