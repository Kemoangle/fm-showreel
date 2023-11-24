using Microsoft.EntityFrameworkCore;
using Showreel.Models;
using Showreel.Service;
using Showreel.Service.impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:5124");

builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IVideoCategoryService, VideoCategoryService>();

builder.Services.AddScoped<IRestrictionService, RestrictionService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//  builder.Services.AddCors(options =>
//  {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy =>
//                       {
//                            policy.WithOrigins("http://localhost:5173", "http://192.168.1.14:5173");
//                        });
//  });

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
Console.WriteLine("sss",connectionString);
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

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
