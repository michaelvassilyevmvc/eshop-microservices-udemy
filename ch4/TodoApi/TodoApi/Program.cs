using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));


var app = builder.Build();

app.MapGet("/todoitems", async (TodoDb db) => await db.TodoItems.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => await db.TodoItems.FindAsync(id));
app.MapPost("/todoitems/", async (TodoItem item, TodoDb db) =>
{
    await db.TodoItems.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});
app.MapPut("/todoitems/{id}", async (int id, TodoItem item, TodoDb db) =>
{
    var itemUpdated = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
    if (itemUpdated == null)
    {
        return Results.NotFound();
    }

    itemUpdated.IsComplete = item.IsComplete;
    itemUpdated.Name = item.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    var itemDeleted = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
    if (itemDeleted == null)
    {
        return Results.NotFound();
    }

    db.TodoItems.Remove(itemDeleted);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();