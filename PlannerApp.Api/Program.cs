using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerApp.Api.Auth;
using PlannerApp.Api.Data;
using PlannerApp.Api.Models;
using PlannerApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddDbContext<IPlannerContext, PlannerContext>(options =>
{
    options.UseNpgsql(builder.Configuration["PlannerAppDatabase:ConnectionString"]);
});
builder.Services.AddScoped<IPlannerService, PlannerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PlannerAppClient", x => x
    .WithOrigins("http://localhost:44398")
    .SetIsOriginAllowed((host) => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("PlannerAppClient");

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("planner", [EnableCors("PlannerAppClient")] async (PlannerItem plannerItem, IPlannerService plannerService) =>
{
    if (plannerItem is null || string.IsNullOrEmpty(plannerItem.Id)) return Results.BadRequest();

    var created = await plannerService.CreatePlannerItemAsync(plannerItem);
    if (!created) return Results.BadRequest();   

    return Results.Created($"/planner/{plannerItem.Id}", plannerItem);    
}).WithName("CreatePlannerItem");

app.MapGet("planner/{id}", [EnableCors("PlannerAppClient")] async (string id, IPlannerService plannerService) =>
{
    var plannerItem = await plannerService.GetPlannerItemAsync(id);
    if(plannerItem is null) return Results.BadRequest();

    return Results.Ok(plannerItem);
}).WithName("GetPlannerItem");

app.MapGet("planner", [EnableCors("PlannerAppClient")] async (IPlannerService plannerService) =>
{
    var plannerItems = await plannerService.GetAllPlannerItemsAsync();

    return Results.Ok(plannerItems);
}).WithName("GetPlannerItems");

app.MapPut("planner", [EnableCors("PlannerAppClient")] async (PlannerItem plannerItem, IPlannerService plannerService) => 
{
    if (plannerItem is null || string.IsNullOrEmpty(plannerItem.Id)) return Results.BadRequest();

    var updated = await plannerService.UpdatePlannerItemAsync(plannerItem);

    return updated ? Results.Ok(plannerItem) : Results.NotFound();
}).WithName("UpdatePlannerItem");

app.MapDelete("planner/{id}", [EnableCors("PlannerAppClient")] async (string id, IPlannerService plannerService) =>
{
    if (string.IsNullOrEmpty(id)) return Results.BadRequest();

    var deleted = await plannerService.DeletePlannerItemAsync(id);

    return deleted ? Results.NoContent() : Results.NotFound();
}).WithName("DeletePlannerItem");

app.Run();