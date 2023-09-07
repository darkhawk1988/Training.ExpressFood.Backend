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
        var rg = new Random();
        user.VerificationCode=rg.Next(100000,1000000).ToString();
        user.BirthDate=DateTime.Now;
        user.RegisterDate=DateTime.Now;
        user.Verified=true;
        await db.ApplicationUsers.AddAsync(user);
        await db.SaveChangesAsync();
        return Results.Ok(user);
    });

app.MapPost("/signin",async (ExpressFoodFoodAppDB db, LoginDto login) =>
{
    if(!db.ApplicationUsers.Any())
    {
        await db.ApplicationUsers.AddAsync(new ApplicationUser
        {
            Username = "admin",
            Password = "admin",
            Firstname = "administrator",
            Lastname = "administrator",
            Gender = "male",
            NationalCode = "1111111111",
            BirthDate = DateTime.Now,
            RegisterDate=DateTime.Now,
            Type=ExpressFood.FoodApp.Core.Enums.ApplicationUserType.SystemAdmin,
            EmailAddress = "admin@test.com",
            Verified = true
        });
        await db.SaveChangesAsync();
    }
    //Thread.Sleep(3000);
    var result =await db.ApplicationUsers.
    FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password && u.Verified==true);
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
        Type=result.Type.ToString(),
        Message = "خوش آمدید",
        IsSucceed = true,
        Token= new JwtSecurityTokenHandler().WriteToken(token)
    });
});

app.Run();
