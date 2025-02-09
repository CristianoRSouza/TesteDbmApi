using ApiDbmTeste.Data.Context;
using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Repositories;
using ApiDbmTeste.FluentMigrator;
using ApiDbmTeste.FluentValidations;
using ApiDbmTeste.Interfaces.IRepositories;
using ApiDbmTeste.Interfaces.IServices;
using ApiDbmTeste.Services;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddFluentMigratorCore()
.ConfigureRunner(rb => rb
   .AddSqlServer()
   .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MyConnection"))
   .ScanIn(typeof(CreateProductTable).Assembly).For.Migrations()
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"), sqlOptions => sqlOptions.CommandTimeout(120))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IServiceProduct, ProductService>();
builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agora sim, cria a aplicação
var app = builder.Build();

// Verifica e cria o banco de dados se necessário
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    EnsureDatabaseExists(builder.Configuration.GetConnectionString("MyConnection"));
    runner.MigrateUp();
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void EnsureDatabaseExists(string connectionString)
{
    var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
    var masterConnectionString = connectionString.Replace(databaseName, "master");

    using var connection = new SqlConnection(masterConnectionString);
    using var command = new SqlCommand($"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}];", connection);

    connection.Open();
    command.ExecuteNonQuery();
}
