// using MinimalAPIWithEFCore.DB;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MinimalAPIWithEFCore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
  {
    c.SwaggerDoc("v1", new OpenApiInfo {
      Title = "PizzaStoreAPI",
      Description = "Making the pizzas you love",
      Version = "v1"
    });
  }
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
  }
);

app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizzas/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
app.MapPost("/pizzas", async (PizzaDb db, Pizza pizza) =>
{
  await db.Pizzas.AddAsync(pizza);
  await db.SaveChangesAsync();
  return Results.Created($"/pizzas/{pizza.Id}", pizza);
});
app.MapPut("/pizzas/{id}", async (PizzaDb db, int id, Pizza updatePizza) =>
{
  var existingPizza = await db.Pizzas.FindAsync(id);

  if (existingPizza is null) return Results.NotFound();

  existingPizza.Name = updatePizza.Name;
  existingPizza.Description = updatePizza.Description;

  await db.SaveChangesAsync();
  return Results.NoContent();
});
app.MapDelete("/pizzas/{id}", async (PizzaDb db, int id) => {
  var existingPizza = await db.Pizzas.FindAsync(id);

  if (existingPizza is null) return Results.NotFound();

  db.Pizzas.Remove(existingPizza);
  await db.SaveChangesAsync();

  return Results.Ok();
});

app.Run();
