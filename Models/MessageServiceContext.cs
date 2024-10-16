﻿using Microsoft.EntityFrameworkCore;

namespace BackEnd_2024_Project.Models
{
    public class MessageServiceContext : DbContext
    {

        public MessageServiceContext(DbContextOptions<MessageServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

    }
}
