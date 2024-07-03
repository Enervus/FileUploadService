using FileUploadService.DAL.DependencyInjection;
using FileUploadService.Application.DependencyInjection;
using System.Runtime.CompilerServices;
using Serilog;
using FileUploadService.Middlewares;
using FileUploadService.Domain.Settings;
using Amazon.S3;
using Amazon.Runtime;
using FileUploadService.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var awsOptions = sp.GetRequiredService<AwsSettings>();
    var awsCredentials = new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey);
    var config = new AmazonS3Config
    {
        ServiceURL = awsOptions.ServiceURL,
        ForcePathStyle = true,
    };
    return new AmazonS3Client(awsCredentials, config);
});
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<FileSizeLimitMiddleware>(100 *1024*1024);//исправить
// Configure the HTTP request pipeline.
app.MapGrpcService<FileEntityController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
