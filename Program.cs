using mcp_core_service.Helpper;
using mcp_core_service.Services.Implement;
using mcp_core_service.Services.Interface;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TuyaOptions>(builder.Configuration.GetSection(TuyaOptions.SectionName));
builder.Services.Configure<BasicAuthOptions>(builder.Configuration.GetSection(BasicAuthOptions.SectionName));
builder.Services.Configure<APIKeysOptions>(builder.Configuration.GetSection(APIKeysOptions.SectionName));
builder.Services.AddScoped<IMCPCoreService, MCPCoreService>();
builder.Services.AddScoped<IIoTCoreService, IoTCoreService>();
builder.Services.AddScoped<ITuyaService, TuyaService>();

// Đăng ký Basic Authentication
builder.Services.AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuth", null);
// Đăng ký các dịch vụ khác
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://*:8026");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
