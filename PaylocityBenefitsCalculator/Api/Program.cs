using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Api.Data;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

//ideally add automapper
//builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<PayrollDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PayrollDbConnection"));
});

//transients, services instanced when needed
builder.Services.AddTransient<ICalculatorService, CalculatorService>();
builder.Services.AddTransient<IPaycheckService, PaycheckService>();
builder.Services.AddTransient<IDependentsService, DependentsService>();
builder.Services.AddTransient<IEmployeesService, EmployeesService>();

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowLocalhost,
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
