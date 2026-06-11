using DriversTestEvaluation.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Data.Context
{
    public class DriversTestEvaluationDbContext : DbContext
    {
        public DriversTestEvaluationDbContext(DbContextOptions<DriversTestEvaluationDbContext> options)
       : base(options)
        {
        }
        public DbSet<DrivingEvent> DrivingEvent { get; set; }
        public DbSet<DrivingSession> DrivingSession { get; set; }
        public DbSet<Results> Results { get; set; }

        public DbSet<Coordinates> Coordinates { get; set; }

        public DbSet<Entry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DrivingSession>()
                .HasMany(s => s.Events)
                .WithOne(e => e.Session)
                .HasForeignKey(e => e.SessionId);
            modelBuilder.Entity<DrivingSession>()
                .HasMany(s => s.Coordinates)
                .WithOne(e => e.Session)
                .HasForeignKey(e => e.SessionId);
            modelBuilder.Entity<DrivingSession>()
                .HasMany(s => s.Entries)
                .WithOne(e => e.Session)
                .HasForeignKey(e => e.SessionId);
        }

        


    }
}
