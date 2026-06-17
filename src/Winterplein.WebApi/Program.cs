using Winterplein.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();

public partial class Program { }
