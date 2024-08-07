using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// AddJsonOptions é usado para adicionar um conversor de string que remove espaços extras
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração para o filter de exception
builder.Services.AddMvc(options
    => options.Filters.Add(typeof(ExceptionFilter)));

// Configuração para a DI do projeto de Application e Infrastructure
builder.Services.AddApplication(builder.Configuration);

// builder.Configuration passado como parâmetro vai servir para recuperar a string de conexão dentro do método AddInfrastructure
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    var connectionString = builder.Configuration.ConnectionString();
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

/*
 * É preciso criar esse partial para as configuracoes do Program.cs serem 
 * acessíveis nos testes de integração 
 */
public partial class Program
{
    protected Program(){}
}