using ExpressFood.FoodApp.Infrastructure.Data;
using ExpressFood.FoodApp.Infrastructure.Security;
using ExpressFood.FoodApp.Infrastructure.UI;
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
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/list",async(ExpressFoodFoodAppDB db)=>
{
    return Results.Ok(db.ApplicationUsers.ToList());
});

app.MapPost("/alist", async (ExpressFoodFoodAppDB db, ListRequestDTO listRequest) =>
{
    return Results.Ok(db.ApplicationUsers.ToList());
});

app.Run();

