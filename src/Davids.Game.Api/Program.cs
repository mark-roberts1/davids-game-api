using Davids.Game.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    var types = assembly.GetTypes();

    foreach (var type in types)
    {
        var attributes = type.GetCustomAttributes().Where(att => att.GetType().IsAssignableTo(typeof(ServiceAttribute))).Cast<ServiceAttribute>();

        foreach (var attribute in attributes)
        {
            builder.Services.Add(new ServiceDescriptor(attribute.Type, type, attribute.Lifetime));
        }
    }
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
