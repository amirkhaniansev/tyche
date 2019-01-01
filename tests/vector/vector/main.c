#include "vector.h"
#include <stdio.h>
int _comparator(const int * p, const int * q) {
	p = q;
	return 0;
}
int _assigner(const int * p, const int * q) {
	p = q;
	return 0;
}
int _finalizer(const void* p) {
	p = NULL;
	return 0;
}
void* _copy_func(const void *p) {
	return NULL;
}

int main() {
	int error;
	vector *v = vector_create(10, sizeof(int), true, &error, &_comparator, &_assigner, &_finalizer, &_copy_func);
	if(error!=SUCCESSFULLY_COMPLETED)
		printf("Error construct= %d\n", error);
	
	for (int i = 1; i < v->_size; i++) {
		error = vector_push_back(v, i);
		if (error != 0)printf("Error push back= %d\n", error);
	}
	vector_clear(v);
	for (unsigned int i = 0; i < v->_size; i++) {
		int x = vector_at(v,i);
		printf("at= %d\n", x);
	}
	getchar();
	vector_destroy(v);
	return 0;
}