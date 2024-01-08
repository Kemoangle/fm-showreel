using System.Text;
using BackEnd.Models;
using BackEnd.Service;
using BackEnd.Service.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Showreel.Models;
using Showreel.Service;
using Showreel.Service.impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.WebHost.UseUrls("http://localhost:5231");

builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IVideoCategoryService, VideoCategoryService>();
builder.Services.AddScoped<IVideoListService, VideoListService>();
builder.Services.AddScoped<ILanlordAdsService, LandlordAdsService>();
builder.Services.AddScoped<IRestrictionService, RestrcitionService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//  builder.Services.AddCors(options =>
//  {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy =>
//                       {
//                            policy.WithOrigins("http://localhost:5173", "http://192.168.1.14:5173");
//                        });
//  });
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ShowreelContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer",options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


builder.Services.AddCors(options =>
          {
              options.AddPolicy(MyAllowSpecificOrigins,
                 builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials());
          });

var connectionString = builder.Configuration.GetConnectionString("MyDB");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33)); // Thay đổi phiên bản MySQL tương ứng
builder.Services.AddDbContext<ShowreelContext>(options =>
{
    options.UseMySql(connectionString, serverVersion);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
