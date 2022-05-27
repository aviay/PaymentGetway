using API_Getway;
using API_Getway.DAL;
using API_Getway.Handlers;
using API_Getway.Handlers.PaymentHandlers;
using API_Getway.Interfaces;
using API_Getway.Middlewares;
using API_Getway.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddDbContext<PaymentContext>(opt => opt.UseInMemoryDatabase(databaseName: "api_getway"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IPaymentHandler, FactoryPaymentInitilazerHandler>();
builder.Services.AddTransient<IPayment, WrapperPaymentHandler>();
builder.Services.AddTransient<IValidation, ValidationHandler>();
builder.Services.AddTransient<IDbConnector, DbConnector>();


builder.Services.AddSwaggerGen(o =>
{
    o.OperationFilter<AddRequiredHeaderParameter>();
});
var jwtSettingsSection = builder.Configuration.GetSection("GeneralSettings");
builder.Services.Configure<GeneralSettings>(jwtSettingsSection);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();