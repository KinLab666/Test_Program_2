using Microsoft.AspNetCore.Mvc;
using WebApplicationTest3.DataBase;
using WebApplicationTest3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register database context as Singleton
builder.Services.AddSingleton<DatabaseContext>(sp =>
{
    var connectionString = builder.Configuration.GetSection("Database")["ConnectionString"];
    return new DatabaseContext(connectionString);
});

builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TagService>();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new BadRequestObjectResult(context.ModelState);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();