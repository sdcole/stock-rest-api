using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;
using StockRestAPI.Models;
using StockRestAPI.Helpers;
public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<DailyBar> DailyBars { get; set; }  // Add this DbSet for BarData
    public DbSet<MinuteBar> MinuteBars { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("AppConnection");
        optionsBuilder.UseNpgsql(connectionString);  // Use PostgreSQL, as indicated
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        // Configure Company entity
        modelBuilder.Entity<Company>().ToTable("companies");

        modelBuilder.Entity<Company>()
            .Property(c => c.Id).HasColumnName("id").IsRequired();

        modelBuilder.Entity<Company>()
            .Property(c => c.Symbol).HasColumnName("symbol").IsRequired().HasMaxLength(10);

        modelBuilder.Entity<Company>()
            .Property(c => c.CompanyDescription).HasColumnName("company_description").IsRequired();

        modelBuilder.Entity<Company>()
            .Property(c => c.Sector).HasColumnName("sector").IsRequired().HasMaxLength(100);

        // Configure BarData entity
        modelBuilder.Entity<MinuteBar>()
            .ToTable("minute_bars");  // Table name in the database

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Id)
            .HasColumnName("id")
            .IsRequired();  // Primary Key, auto-generated

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Symbol)
            .HasColumnName("symbol")
            .IsRequired()
            .HasMaxLength(10);  // Max length for stock symbols like "AAPL", "MSFT"

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Timestamp)
            .HasColumnName("timestamp")
            .IsRequired();  // Timestamp in RFC-3339 format

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Open)
            .HasColumnName("open")
            .IsRequired();  // Opening price

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.High)
            .HasColumnName("high")
            .IsRequired();  // Highest price

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Low)
            .HasColumnName("low")
            .IsRequired();  // Lowest price

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Close)
            .HasColumnName("close")
            .IsRequired();  // Closing price

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.Volume)
            .HasColumnName("volume")
            .IsRequired();  // Trade volume

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.TradeCount)
            .HasColumnName("trade_count")
            .IsRequired();  // Number of trades

        modelBuilder.Entity<MinuteBar>()
            .Property(b => b.VW)
            .HasColumnName("vw")
            .IsRequired();  // Volume Weighted Average Price

        // Define unique constraint for Symbol and Timestamp
        modelBuilder.Entity<MinuteBar>()
            .HasIndex(b => new { b.Symbol, b.Timestamp })
            .IsUnique();


        // Configure BarData entity
        modelBuilder.Entity<DailyBar>()
            .ToTable("daily_bars");  // Table name in the database

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Id)
            .HasColumnName("id")
            .IsRequired();  // Primary Key, auto-generated

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Symbol)
            .HasColumnName("symbol")
            .IsRequired()
            .HasMaxLength(10);  // Max length for stock symbols like "AAPL", "MSFT"

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Timestamp)
            .HasColumnName("timestamp")
            .IsRequired();  // Timestamp in RFC-3339 format

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Open)
            .HasColumnName("open")
            .IsRequired();  // Opening price

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.High)
            .HasColumnName("high")
            .IsRequired();  // Highest price

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Low)
            .HasColumnName("low")
            .IsRequired();  // Lowest price

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Close)
            .HasColumnName("close")
            .IsRequired();  // Closing price

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.Volume)
            .HasColumnName("volume")
            .IsRequired();  // Trade volume

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.TradeCount)
            .HasColumnName("trade_count")
            .IsRequired();  // Number of trades

        modelBuilder.Entity<DailyBar>()
            .Property(b => b.VW)
            .HasColumnName("vw")
            .IsRequired();  // Volume Weighted Average Price

        // Define unique constraint for Symbol and Timestamp
        modelBuilder.Entity<DailyBar>()
            .HasIndex(b => new { b.Symbol, b.Timestamp })
            .IsUnique();
    }
}
