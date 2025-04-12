using Background_service;
using BackgroundServiceRPC;

using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<Worker>();

var app = builder.Build();
var worker = app.Services.GetRequiredService<Worker>();
app.MapGrpcService<Worker>();

app.Run();
