var builder = WebApplication.CreateBuilder(args);
// services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly); 
});
var app = builder.Build();

// pipelines
app.MapCarter();
app.Run();