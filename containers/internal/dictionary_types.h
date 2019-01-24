/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Types for dictionary.
 * Copyright (C) <2018>
 *               Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *                        <DavidPetr>       <david.petrosyan11100@gmail.com>
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

#ifndef __DICTIONARY_TYPES__
#define __DICTIONARY_TYPES__

/* dictionary types */
typedef struct dictionary dictionary_t;
typedef struct keys keys_t;
typedef struct values values_t;

/* function pointer types */
typedef bool(*key_comparator)(const void*, const void*);
typedef int(*key_hasher)(const void*);
typedef void(*key_finalizer)(void*);
typedef void(*value_finalizer)(void*);
typedef char*(*key_to_string_func)(const void*);

#endif
