
using DriversTestEvaluation.Business.Analyzers;
using DriversTestEvaluation.Business.Services;
using DriversTestEvaluation.Core.IServices;
using DriversTestEvaluation.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
namespace DriversTestEvaluation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IFrameBuffer, FrameBuffer>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IResultsService, ResultsService>();
            builder.Services.AddScoped<IDrivingEventService, DrivingEventService>();
            builder.Services.AddScoped<LlavaVisionAnalyzer, LlavaVisionAnalyzer>();
            builder.Services.AddScoped<SimulatorAnalyzer, SimulatorAnalyzer>();
            builder.Services.AddDbContext<DriversTestEvaluationDbContext>(options =>
                options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("React",
                    policy =>
                    {
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:5173");
                    });
            });

            var ApiKey = builder.Configuration["GeminiApi:ApiKey"];
            var BaseUrl = builder.Configuration["GeminiApi:BaseUrl"];
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("React");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
