using Microsoft.EntityFrameworkCore;
using MiniIAM.Data;
using MiniIAM.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar DbContext con EF Core InMemory
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MiniIAM_Db"));
// to use an in-memory database for simplicity


builder.Services.AddControllers();

//services dependencies injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(dbContext);
}

app.MapControllers();

app.Run();
