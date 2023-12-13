﻿using JobOffersService.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.DbContexts
{
    public class JobOffersContext : DbContext
    {
        public JobOffersContext(DbContextOptions<JobOffersContext> options) : base(options) { }

        public DbSet<Technology> Technologies { get; set; } = null!;

        public DbSet<JobOffer> JobOffers { get; set; } = null!;
    }
}
