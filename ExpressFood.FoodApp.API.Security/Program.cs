using Ardalis.GuardClauses;
using ExpressFood.FoodApp.API.Security.DTOs;
using ExpressFood.FoodApp.Core.Entities;
using ExpressFood.FoodApp.Core.Enums;
using ExpressFood.FoodApp.Core.Exceptions;
using ExpressFood.FoodApp.Infrastructure.Data;
using ExpressFood.FoodApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Versioning;
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

app.MapPost("/signup",async (ExpressFoodFoodAppDB db,RegisterRequestDto Register ) =>
    {
        var rg = new Random();
        var user = new ApplicationUser(); 
        user.VerificationCode=rg.Next(100000,1000000).ToString();
        user.BirthDate=DateTime.Now;
        user.RegisterDate=DateTime.Now;
        user.Verified=true;
        user.Username=Register.Username;
        Guard.Against.NullOrEmpty(user.Username,message:"نام کاربری نمی تواند تهی باشد ");
        if(Register.Password.Length > 4)
        {
            throw new InvalidPasswordException();
        }
        user.Password=Register.Password;
        user.Firstname=Register.Firstname;
        user.Lastname=Register.Lastname;
        user.Gender=Register.Gender;
        user.NationalCode=Register.NationalCode;
        user.Type=Register.Type;
        user.EmailAddress=Register.EmailAddress;
        user.Cellphone=Register.Cellphone;
        await db.ApplicationUsers.AddAsync(user);
        try
        {
            await db.SaveChangesAsync();
        }
        catch
        {
            throw new DuplicateUsernameException();
        }
        return Results.Ok(user);
    });

app.MapPost("/signin",async (ExpressFoodFoodAppDB db, LoginDto login) =>
{
    //Thread.Sleep(3000);
    var result =await db.ApplicationUsers.
    FirstOrDefaultAsync(u => u.Type != ApplicationUserType.SystemAdmin && u.Username == login.Username && u.Password == login.Password && u.Verified==true);
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

app.MapPost("/adminsignin", async (ExpressFoodFoodAppDB db, LoginDto login) =>
{
    if (!db.ApplicationUsers.Any())
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
            EmailAddress = "admin@test.com",
            Type = ExpressFood.FoodApp.Core.Enums.ApplicationUserType.SystemAdmin,
            VerificationCode="8888888888",
            Verified = true,
            RegisterDate = DateTime.Now,
            Cellphone="9181111111"
        });
        await db.SaveChangesAsync();
    }
    //Thread.Sleep(3000);
    var result = await db.ApplicationUsers.
    FirstOrDefaultAsync(u => u.Type==ApplicationUserType.SystemAdmin && u.Username == login.Username && u.Password == login.Password && u.Verified == true);
    if (result == null)
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
    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        builder.Configuration["Jwt:Issuer"],
        builder.Configuration["Jwt:Audience"],
        claims,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: signIn);
    return Results.Ok(new LoginResultDto
    {
        Type = result.Type.ToString(),
        Message = "خوش آمدید",
        IsSucceed = true,
        Token = new JwtSecurityTokenHandler().WriteToken(token)
    });
});

app.Run();
