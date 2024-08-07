using UserManagementAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container using the extension method
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline using the extension method
app.ConfigureApplication();

app.Run();
