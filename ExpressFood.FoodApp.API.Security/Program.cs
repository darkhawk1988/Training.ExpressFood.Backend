using ExpressFood.FoodApp.API.Security.DTOs;
using ExpressFood.FoodApp.Core.Entities;
using ExpressFood.FoodApp.Infrastructure.Data;
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

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/signup", (ExpressFoodFoodAppDB db,ApplicationUser user ) =>
    {
        db.ApplicationUsers.Add(user);
        db.SaveChanges();
        return Results.Ok(user);
    });

app.MapPost("/signin", (ExpressFoodFoodAppDB db, LoginDto login) =>
{
    var result = db.ApplicationUsers.
    FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
    if(result == null)
    {
        return Results.Ok(new
        {
            message = "نام کاربری یا کلمه عبور صحیح نیست",
            success = false
        });
    }
    return Results.Ok(new
    {
        message = "خوش آمدید",
        success = true
    });
});

app.Run();
