using Abp.Zero.EntityFrameworkCore;
using ProjectBlack.Authorization.Roles;
using ProjectBlack.Authorization.Users;
using ProjectBlack.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using ProjectBlack.CreditRequests;


namespace ProjectBlack.EntityFrameworkCore;

public class ProjectBlackDbContext : AbpZeroDbContext<Tenant, Role, User, ProjectBlackDbContext>
{
    /* Define a DbSet for each entity of the application */

    public ProjectBlackDbContext(DbContextOptions<ProjectBlackDbContext> options)
        : base(options)
    {
    }
    public DbSet<CreditRequest> CreditRequests { get; set; }

}
