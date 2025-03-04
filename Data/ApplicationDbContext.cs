using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LUXLocks_projekt.Models;

namespace LUXLocks_projekt.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet <AppointmentModel> Appointments { get; set; }
    public DbSet <StylistModel> Stylists { get; set; }
    public DbSet <TreatmentModel> Treatments { get; set; }
    public DbSet <ReviewModel> Reviews { get; set; }
    public DbSet <CustomerProfileModel> CustomerProfiles { get; set; }
}
