using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace SensorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
         
            app.MapGet("/sensor", async ([FromBody] SensorReading reading, Channel<SensorReading> channel) =>
            {
                // Write directly to the channel
                await channel.Writer.WriteAsync(reading);
                return Results.Ok(new { message = "Reading queued for processing" });
            })
            .WithName("IngestSensorData");

            app.Run();
        }
    }
}
