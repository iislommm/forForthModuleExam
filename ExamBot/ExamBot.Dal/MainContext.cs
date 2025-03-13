using ExamBot.Dal.Entities;
using ExamBot.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExamBot.Dal;

public class MainContext : DbContext
{
    public DbSet<BotUser> botUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=WIN-GB0R10P0CFD\\SQLEXPRESS;Database=ExamBot;User Id=sa;Password=1;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BotUserConfiguration());
    }
}
//static void Main(string[] args)
//{
//    var serviceCollection = new ServiceCollection();

//    serviceCollection.AddSingleton<MainContext>();

//    var serviceProvider = serviceCollection.BuildServiceProvider();

//    //Console.WriteLine("Hello, World!");
//}
