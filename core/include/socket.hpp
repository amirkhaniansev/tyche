/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Header file for socket
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

#ifndef __SOCKET_HPP__
#define __SOCKET_HPP__

#include "types.hpp"

namespace core
{
	namespace socket
	{
		class Socket
		{
			private:
				AddressFamily addressFamily;
				SocketType socketType;
				ProtocolType protocolType;
				SocketAddress address;
				SocketDescriptor descriptor;
			protected:
				bool isConnected;
				bool isDisconnected;
				bool isListening;
			public:
				Socket(const SocketType&, const ProtocolType&);
				Socket(const AddressFamily&, const SocketType&, const ProtocolType&);
				Socket(const Socket&);
				~Socket();
				AddressFamily GetAddressFamily();
				ProtocolType  GetProtocolType();
				SocketType    GetSocketType();
				SocketAddress GetSocketAddress();
				Socket* Accept();
				void Bind(const SocketAddress&);
				void Listen(const int&);
				void Connect(const SocketAddress&);
				void Close();
				void Receive(const byte*, const int&);
				void Send(const byte*, const int&);
		};
	}
}

#endif
