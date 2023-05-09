using Lab11_ASP_Identity.Data;
using Lab11_ASP_Identity.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ASPIdentityContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))).AddIdentity<User, Role>(config =>
{
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireDigit = false;
    config.Password.RequiredLength = 6;
    config.Password.RequireLowercase = false;
    config.Password.RequireUppercase=false;
})
    .AddEntityFrameworkStores<ASPIdentityContext>();

builder.Services.ConfigureApplicationCookie(config => {
    config.LoginPath = "/Admin/Login";
    config.AccessDeniedPath = "/Admin/AccessDenied";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();