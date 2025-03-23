using System.Diagnostics.CodeAnalysis;
using adapter.amazon.s3;
using adapter.amazon.sqs;
using adapter.api.Configuration;
using adapter.api.Workers;
using adapter.video;
using core.application;
using core.domain.options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<SqsOptions>(builder.Configuration.GetSection("SqsOptions"));
builder.Services.Configure<AmazonOptions>(builder.Configuration.GetSection("AmazonOptions"));
builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3Options"));

builder.Services.AddAmazonSqs();
builder.Services.AddAmazonS3();


builder.Services.AddAdapterAmazonS3Services();
builder.Services.AddAdapterAmazonSqsServices();
builder.Services.AddAdapterVideoServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<WorkerConsumerSqsVideoMessage>();
//TODO: Criar um consumidor da fila de DLQ?

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
