using StudiBase.Shared.Services;
using StudiBase.Web.Components;
using StudiBase.Web.Services;
using Microsoft.EntityFrameworkCore;
using StudiBase.Web.Data;
using AutoMapper;
using StudiBase.Shared.Clients;
using Microsoft.AspNetCore.Components;
using StudiBase.Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
 .AddInteractiveServerComponents();

builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10 MB
});

// Add device-specific services used by the StudiBase.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IFileService, FileService>();
var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=StudiBase.db"; 
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(cs));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(o => o.LowercaseUrls = true);
builder.Services.AddCors(p => p.AddPolicy("Dev", b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// Tambahkan baris ini agar Project Web tidak error mencari CameraService
builder.Services.AddScoped<ICameraService, BrowserCameraService>();

// Daftarkan Service Geolocation Browser (Dummy)
builder.Services.AddScoped<IGeolocationService, BrowserGeolocationService>();

// HttpClient for Blazor Server with BaseAddress from current NavigationManager
builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiBaseUrl = config["ApiBaseUrl"];

    return new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl!)
    };
});

// Typed API clients reuse the scoped HttpClient
builder.Services.AddScoped<FileApiClient>();
builder.Services.AddScoped<TrainerApiClient>();
builder.Services.AddScoped<CourseApiClient>();

builder.Services.AddScoped<INetworkService, BrowserNetworkService>();

builder.Services.AddScoped<IGeocodingService, BrowserGeocodingService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                               Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();
app.UseForwardedHeaders();
app.UseCors("Dev");

using (var scope = app.Services.CreateScope()) {
 var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); db.Database.Migrate();
};
// atau db.Database.EnsureCreated(); }

if (app.Environment.IsDevelopment()) { 
 app.UseSwagger(); app.UseSwaggerUI(); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Error", createScopeForErrors: true);
 // The default HSTS value is30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
 app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Map API controllers
app.MapControllers();

app.MapRazorComponents<App>()
 .AddInteractiveServerRenderMode()
 .AddAdditionalAssemblies(typeof(StudiBase.Shared._Imports).Assembly);

app.Run();
