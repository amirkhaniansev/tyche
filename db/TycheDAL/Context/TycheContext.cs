/**
 * GNU General Public License Version 3.0, 29 June 2007
 * TycheContext
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
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

using Microsoft.EntityFrameworkCore;
using TycheDAL.Models;

namespace TycheDAL.Context
{
    public class TycheContext : DbContext
    {
        private readonly string connectionString;

        public DbSet<User> Users { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<NotificationAssignment> NotificationAssignments { get; set; }
        
        public DbSet<ChatRoomMember> ChatroomMembers { get; set; }

        public TycheContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoomMember>()
                .HasKey(crm => new
                {
                    crm.ChatRoomId,
                    crm.UserId
                });

            modelBuilder.Entity<NotificationAssignment>()
                .HasKey(na => new
                {
                    na.NotificationId,
                    na.UserId
                });

            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Notification>()
                .Property(notification => notification.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Message>()
                .Property(message => message.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Verification>()
                .Property(verification => verification.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ChatRoom>()
                .Property(chatroom => chatroom.Id)
                .ValueGeneratedOnAdd();
        }
    }
}