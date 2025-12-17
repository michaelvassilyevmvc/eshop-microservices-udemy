var builder = WebApplication.CreateBuilder(args);
// services
builder.Services.AddCarter();
builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddMarten(opts => { opts.Connection(builder.Configuration.GetConnectionString("Database")!); })
    .UseLightweightSessions();
var app = builder.Build();

// pipelines
app.MapCarter();
app.Run();