using Microsoft.EntityFrameworkCore;

namespace MinimalAPIWithEFCore.Models
{
  public class Pizza
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
  }

  class PizzaDb : DbContext
  {
    public PizzaDb(DbContextOptions options) : base(options) { }
    public DbSet<Pizza> Pizzas { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //   optionsBuilder.UseInMemoryDatabase("Pizzas");
    // }
  }
}
