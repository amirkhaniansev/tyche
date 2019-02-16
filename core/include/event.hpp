/**
 * GNU General Public License Version 3.0, 29 June 2007
 * event
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


#ifndef __EVENT_HPP__
#define __EVENT_HPP__

#include <list>
#include <functional>

namespace core
{
	template<typename TEventArgument, typename TSender = void*>
	class Event
	{
		typedef std::function<void(const TSender&, const TEventArgument&)> Handler;
		typedef std::list<Handler> HandlersList;
		typedef unsigned long long ulong;
		private:
			HandlersList *handlers;
			ulong riseCount;
		public:
			Event();
			Event(const Event&);
			~Event();
			void AddHandler(const Handler&);
			void RemoveHandler(const Handler&);
			void RiseEvent(const TSender&, const TEventArgument&);
	};

	template<typename TEventArgument, typename TSender>
	Event<TEventArgument, TSender>::Event()
		: riseCount(0), handlers(new HandlersList()) {}

	template<typename TEventArgument, typename TSender>
	Event<TEventArgument, TSender>::Event(const Event& event)
		: riseCount(event.riseCount), handlers(event.handlers) {}

	template<typename TEventArgument, typename TSender>
	Event<TEventArgument, TSender>::~Event()
	{
		delete this->handlers;
	}

	template<typename TEventArgument, typename TSender>
	void Event<TEventArgument, TSender>::AddHandler(const Handler& handler)
	{
		this->handlers->push_front(handler);
	}

	template<typename TEventArgument, typename TSender>
	void Event<TEventArgument, TSender>::RemoveHandler(const Handler& handler)
	{
		this->handlers->remove(handler);
	}

	template<typename TEventArgument, typename TSender>
	void Event<TEventArgument, TSender>::RiseEvent(const TSender& sender, const TEventArgument& arg)
	{
		for (auto const &handler : *this->handlers)
			handler(sender, arg);
		this->riseCount++;
	}
};

#endif