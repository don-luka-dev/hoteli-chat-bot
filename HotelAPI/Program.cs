using FluentValidation;
using FluentValidation.AspNetCore;
using HotelAPI.AI.Plugins;
using HotelAPI.Data;
using HotelAPI.Models;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using HotelAPI.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SemanticKernel;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true  // Možeš staviti false ako si veæ ruèno stvorio tablicu
        })
    .CreateLogger();

Log.Logger = logger;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:50495") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Host.UseSerilog();

builder.Services.RegisterMapsterConfiguration();

builder.Services.AddHttpContextAccessor();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Hotel API",
        Version = "v1"
    });

    // Add JWT Bearer Auth support
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your valid token.\n\nExample: \"abc123xyz\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<Korisnik, KorisnikovaUloga>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddScoped<IKorisnikService, KorisnikService>();
builder.Services.AddScoped<IMjestoService, MjestoService>();
builder.Services.AddScoped<IPostanskiUredService, PostanskiUredService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelSobaService, HotelSobaService>();
builder.Services.AddScoped<ISobaService, SobaService>();
builder.Services.AddScoped<IRezervacijaService, RezervacijaService>();
builder.Services.AddScoped<ISobaRezervacijaService, SobaRezervacijaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISlikaSobeService, SlikaSobeService>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddValidatorsFromAssemblyContaining<PostanskiUredDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MjestoDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SobaDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<HotelDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SlikaSobeDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RezervacijaDtoValidator>();


//chatBotKernel
builder.Services.AddScoped<RezervacijaPlugin>();
builder.Services.AddScoped<HotelPlugin>();
builder.Services.AddScoped<SobaPlugin>();


var apiKey = builder.Configuration["OpenAi:ApiKey"];
builder.Services.AddOpenAIChatCompletion("gpt-4o", apiKey!);

builder.Services.AddScoped<KernelPluginCollection>(serviceProvider =>
{
    var rezervacijaPlugin = KernelPluginFactory.CreateFromObject(
     serviceProvider.GetRequiredService<RezervacijaPlugin>()
 );
    var hotelPlugin = KernelPluginFactory.CreateFromObject(
       serviceProvider.GetRequiredService<HotelPlugin>(),
       nameof(HotelPlugin) 
 );
    var sobaPlugin = KernelPluginFactory.CreateFromObject(
      serviceProvider.GetRequiredService<SobaPlugin>(),
      nameof(SobaPlugin) 
);

    foreach (var function in rezervacijaPlugin)
        Console.WriteLine($"Registrirana funkcija: {function.Name}");
    
    foreach (var function in hotelPlugin)
        Console.WriteLine($"Registrirana funkcija (hotel): {function.Name}");

    foreach (var function in sobaPlugin)
        Console.WriteLine($"Registrirana funkcija (soba): {function.Name}");


    return new KernelPluginCollection { rezervacijaPlugin, hotelPlugin, sobaPlugin };
});

builder.Services.AddTransient((serviceProvider) => {
    KernelPluginCollection pluginCollection = serviceProvider.GetRequiredService<KernelPluginCollection>();

    return new Kernel(serviceProvider, pluginCollection);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
