#ifndef __STACK_H__
#define __STACK_H__

#include <stddef.h>
#include <stdbool.h>
#include <stdlib.h>

#define STACK_DEFAULT_SIZE		0x4
#define STACK_SUCCESS			0x0
#define STACK_IS_NULL			0x1
#define STACK_BASE_IS_NULL		0x2
#define STACK_PUSH_ITEM_IS_NULL	0x3
#define STACK_IS_EMPTY			0x5

#define DEFINE_GENERIC_STACK_TYPE(TYPE)								\
	typedef struct __stack_##TYPE{								\
		size_t _size;									\
		size_t _count;									\
		size_t _type_size;								\
		TYPE* _base;									\
	} stack_##TYPE;

#define DEFINE_GENERIC_STACK_ITEM_FINALIZER(TYPE)						\
	typedef void(*finalizer_##TYPE)(TYPE* ptr);

#define STACK_NULL_CHECK(STACK_PTR)								\
	if((STACK_PTR) == NULL)									\
		return STACK_IS_NULL

#define STACK_BASE_NULL_CHECK(STACK_PTR)							\
	if((STACK_PTR) -> _base == NULL)							\
		return STACK_BASE_IS_NULL

#define DEFINE_GENERIC_STACK_CONSTRUCTOR(TYPE)							\
	stack_##TYPE* stack_create_##TYPE()							\
	{											\
		stack_##TYPE* stack = malloc(sizeof(stack_##TYPE));				\
		if(stack == NULL)								\
			return NULL;								\
												\
		stack->_size = STACK_DEFAULT_SIZE;						\
		stack->_type_size = sizeof(TYPE);					\
		stack->_count = 0x0;								\
												\
		stack->_base = malloc(stack->_size * stack->_type_size);			\
		if(stack->_base == NULL) {							\
			free(stack);								\
			return NULL;								\
		}										\
												\
		return stack;									\
	}

#define DEFINE_GENERIC_STACK_CLEAR(TYPE)							\
	int stack_clear_##TYPE(stack_##TYPE* stack, finalizer_##TYPE finalizer)			\
	{											\
		STACK_NULL_CHECK(stack);							\
		STACK_BASE_NULL_CHECK(stack);							\
												\
		if(stack -> _count == 0)							\
		    return STACK_SUCCESS;							\
												\
		if(finalizer != NULL)								\
			for(size_t i = 0; i < stack->_count; i++)				\
				finalizer(stack->_base + stack->_count);			\
												\
		stack->_count ^= stack->_count;							\
		return STACK_SUCCESS;								\
	}

#define DEFINE_GENERIC_STACK_DESTRUCTOR(TYPE)							\
	int stack_destroy_##TYPE(stack_##TYPE* stack, finalizer_##TYPE finalizer)		\
	{											\
		int clear_result = stack_clear_##TYPE(stack, finalizer);			\
		if(clear_result != STACK_SUCCESS)						\
			return clear_result;							\
												\
		free(stack->_base);								\
		free(stack);									\
		return STACK_SUCCESS;								\
	}

#define DEFINE_GENERIC_STACK_PUSH(TYPE)								\
	int stack_push_##TYPE(stack_##TYPE* stack, TYPE* item_ptr)				\
	{											\
		STACK_NULL_CHECK(stack);							\
		STACK_BASE_NULL_CHECK(stack);							\
		if(item_ptr == NULL)								\
			return STACK_PUSH_ITEM_IS_NULL;						\
												\
		if(stack->_size == stack->_count) {						\
			stack->_size <<= 1;							\
												\
			TYPE* new_base = malloc(stack->_type_size * stack->_size);		\
			if(new_base == NULL)							\
				return STACK_BASE_IS_NULL;					\
												\
			for(size_t i = 0; i < stack->_count; i++)				\
				new_base[i] = stack->_base[i];					\
												\
			free(stack->_base);							\
												\
			stack->_base = new_base;						\
		}										\
												\
		stack->_base[stack->_count++] = *item_ptr;					\
												\
		return STACK_SUCCESS;								\
	}

#define DEFINE_GENERIC_STACK_POP(TYPE)								\
	int stack_pop_##TYPE(stack_##TYPE* stack)						\
	{											\
		STACK_NULL_CHECK(stack);							\
		STACK_BASE_NULL_CHECK(stack);							\
		if(stack->_count == 0)								\
			return STACK_IS_EMPTY;							\
												\
		stack->_count--;								\
		return STACK_SUCCESS;	    							\
	}

#define DEFINE_GENERIC_STACK_TOP(TYPE)								\
	TYPE* stack_top_##TYPE(stack_##TYPE* stack)						\
	{											\
		if(stack == NULL || stack->_base == NULL || stack->_count == 0)			\
			return NULL;								\
		return stack->_base[stack->_count - 1];						\
	}

#define __DEFINE_GENERIC_STACK__(TYPE)								\
	DEFINE_GENERIC_STACK_TYPE(TYPE);							\
	DEFINE_GENERIC_STACK_CONSTRUCTOR(TYPE)							\
	DEFINE_GENERIC_STACK_ITEM_FINALIZER(TYPE);						\
	DEFINE_GENERIC_STACK_CLEAR(TYPE);							\
	DEFINE_GENERIC_STACK_DESTRUCTOR(TYPE);							\
	DEFINE_GENERIC_STACK_PUSH(TYPE);							\
	DEFINE_GENERIC_STACK_POP(TYPE);								\
	DEFINE_GENERIC_STACK_TOP(TYPE);	

__DEFINE_GENERIC_STACK__(int);
__DEFINE_GENERIC_STACK__(double);
__DEFINE_GENERIC_STACK__(float);

#endif
