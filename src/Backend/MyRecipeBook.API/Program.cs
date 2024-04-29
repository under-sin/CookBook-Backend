using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração para o filter de exception
builder.Services.AddMvc(options
    => options.Filters.Add(typeof(ExceptionFilter)));

// Configuração para a DI do projeto de Application e Infrastructure
builder.Services.AddApplication(builder.Configuration);

// builder.Configuration passado como parâmetro vai servir para recuperar a string de conexão dentro do método AddInfrastructure
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    
    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}