using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models; 
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddGameStoreDb();

builder.Services.AddValidation();

var app = builder.Build();

app.MapGameEndpoints();
app.MapGenreEndpoints();

app.MigrateDb();

app.Run();
