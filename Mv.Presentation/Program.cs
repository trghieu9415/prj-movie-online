using Microsoft.EntityFrameworkCore;
using Mv.Infrastructure;
using Mv.Infrastructure.Seeding;
using Mv.Presentation.Extensions;
using Mv.Presentation.Hubs;
using Mv.Presentation.Middlewares;
using Mv.Worker;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// --- Infrastructure ---
builder.Services.AddInfrastructure(builder.Configuration);
builder.Configuration.AddJsonFile("secrets.json", true, true);

// --- Worker ---
if (!args.Contains("--seeding") && !EF.IsDesignTime) {
  builder.Services.AddWorker(builder.Configuration);
}

// --- Presentation Extension ---
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddWebApiDefaults();
builder.Services.AddSwaggerDocument();
builder.Services.AddSignalRAdapters();
builder.AddSerilogCustom();

builder.Services.AddHttpContextAccessor();


// =========================================================================
// || -_-_-_-_-_-_-_-_-_-_-_-_-_-_ APP BUILD _-_-_-_-_-_-_-_-_-_-_-_-_-_- ||
// =========================================================================
var app = builder.Build();

// --- CLI Flag: Seeding Data ---
if (args.Contains("--seeding")) {
  using var scope = app.Services.CreateScope();
  try {
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.SeedAsync();
  } catch (Exception ex) {
    Console.WriteLine($"[-] Lỗi trong quá trình Seeding: {ex.Message}");
  }

  return;
}


// --- Custom Middlewares ---
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

// --- Swagger UI ---
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Dashboard API");
    c.DocExpansion(DocExpansion.None);
  });
}

// --- Allow Static Files ---
app.UseStaticFiles();

// --- CORS ---
app.UseCors(options => options
  .AllowAnyMethod()
  .AllowAnyHeader()
  .SetIsOriginAllowed(_ => true)
  .AllowCredentials());

// --- Authentication & Authorization ---
app.UseAuthentication();
app.UseAuthorization();

// --- Endpoints ---
app.MapControllers();
app.MapHub<ShowtimeHub>("/hubs/showtime");
app.MapHub<UserHub>("/hubs/notification");

app.Run();
