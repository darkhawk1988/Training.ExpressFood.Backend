using ExpressFood.FoodApp.Infrastructure.Data;
using ExpressFood.FoodApp.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ExpressFoodFoodAppDB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
});
builder.Services.AddCors(options
    => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

SecurityServices.AddServices(builder);
var app = builder.Build();
SecurityServices.UseServices(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/requestlist", async (ExpressFoodFoodAppDB db) =>
{
    return Results.Ok(db.Restaurants.Where
        (r=>r.IsApproved==false).ToList());
});

app.MapGet("/requestcount", async (ExpressFoodFoodAppDB db) =>
{
    return Results.Ok(new
    {
        count = await db.Restaurants.CountAsync(r=>r.IsApproved==false)
    });
});

app.Run();

