using ExpressFood.FoodApp.API.Security.DTOs;
using ExpressFood.FoodApp.Core.Entities;
using ExpressFood.FoodApp.Infrastructure.Data;
using ExpressFood.FoodApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

app.UseHttpsRedirection();

app.MapPost("/signup",async (ExpressFoodFoodAppDB db,ApplicationUser user ) =>
    {
        await db.ApplicationUsers.AddAsync(user);
        await db.SaveChangesAsync();
        return Results.Ok(user);
    });

app.MapPost("/signin",async (ExpressFoodFoodAppDB db, LoginDto login) =>
{
    var result =await db.ApplicationUsers.
    FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password);
    if(result == null)
    {
        return Results.Ok(new LoginResultDto
        {
            Message = "نام کاربری یا کلمه عبور صحیح نیست",
            IsSucceed = false
        });
    }
    var claims = new[]
    {
        new Claim("Type",result.Type.ToString()),
        new Claim("Username",result.Username.ToString()),
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key" ?? ""]));
    var signIn= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
var token = new JwtSecurityToken(
    builder.Configuration["Jwt:Issuer"],
    builder.Configuration["Jwt:Audience"],
    claims,
    expires: DateTime.UtcNow.AddDays(1),
    signingCredentials: signIn);
    return Results.Ok(new LoginResultDto
    {
        Message = "خوش آمدید",
        IsSucceed = true,
        Token= new JwtSecurityTokenHandler().WriteToken(token)
    });
});

app.Run();
