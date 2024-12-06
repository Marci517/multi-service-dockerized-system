using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Retrieve environment variables for database connection
string dbHost = builder.Configuration["DB_HOST"];
string dbName = builder.Configuration["DB_NAME"];
string dbUser = builder.Configuration["DB_USER"];
string dbPassword = builder.Configuration["DB_PASSWORD"];

// Construct the connection string dynamically
string connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}";

// Add services to the container
builder.Services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

var app = builder.Build();
app.Urls.Add("http://0.0.0.0:8082");

// Get restaurant by ID
app.MapGet("/restaurant/{id:int}", async (int id, NpgsqlConnection dbConnection) =>
{
    await dbConnection.OpenAsync();
    using var cmd = new NpgsqlCommand("SELECT * FROM Restaurant WHERE id = @id", dbConnection);
    cmd.Parameters.AddWithValue("id", id);
    using var reader = await cmd.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
        var restaurant = new
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Adress = reader.GetString(2),
            Rating = reader.GetInt32(3)
        };
        return Results.Json(restaurant);  // Ensure Results is recognized
    }

    return Results.NotFound(new { error = "Restaurant not found" });
});

// Get all restaurants
app.MapGet("/restaurants", async (NpgsqlConnection dbConnection) =>
{
    await dbConnection.OpenAsync();
    using var cmd = new NpgsqlCommand("SELECT * FROM Restaurant", dbConnection);
    using var reader = await cmd.ExecuteReaderAsync();

    var restaurants = new List<object>();
    while (await reader.ReadAsync())
    {
        restaurants.Add(new
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Adress = reader.GetString(2),
            Rating = reader.GetInt32(3)
        });
    }

    return Results.Json(restaurants.Count > 0 ? restaurants : new { error = "No restaurants found" });
});

app.Run();
