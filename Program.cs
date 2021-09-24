using Microsoft.Extensions.Caching.Memory;

// OpenAPI description constants
const string OpenApiDocName = "TodoApi";
const string OpenApiDocVersion = "2021-09-01";
const string TodosCacheKey = "todos";

var builder = WebApplication.CreateBuilder(args);

// Enable OpenAPI description generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(OpenApiDocName, new() { Title = "TodoApi", Version = OpenApiDocVersion });
});

// Use Memory Cache for storage for sample purposes
builder.Services.AddMemoryCache();

// Build the app host
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{OpenApiDocName}/swagger.json", OpenApiDocName));
}

// Add sample data to the memory cache
app.Services.GetRequiredService<IMemoryCache>()
    .Set<List<Todo>>(TodosCacheKey,
        new List<Todo>(new[] {
            new Todo(1, "Implement real database storage"),
            new Todo(2, "Remove the memory cache code")
        }));

// Get all todos
app.MapGet("/todos", (IMemoryCache memoryCache) => Results.Ok(memoryCache.Get<List<Todo>>(TodosCacheKey)))
   .WithName("GetAllTodos")
   .Produces<List<Todo>>();

// Create a new todo
app.MapPost("/todos", (IMemoryCache memoryCache, Todo todo) =>
    {
        var todos = memoryCache.Get<List<Todo>>(TodosCacheKey);
        if(todos.Any(t => t.Id == todo.Id))
        {
            return Results.Conflict($"Todo with id {todo.Id} already exists.");
        }
        else
        {
            todos.Add(todo);
            memoryCache.Set<List<Todo>>(TodosCacheKey, todos);
            return Results.Created($"/todo/{todo.Id}", todo);
        }
    })
    .WithName("CreateTodo")
    .Produces(StatusCodes.Status409Conflict)
    .Produces<Todo>(StatusCodes.Status201Created);

// Update an existing todo
app.MapPut("/todos", (IMemoryCache memoryCache, int id, Todo todo) =>
    {
        var todos = memoryCache.Get<List<Todo>>(TodosCacheKey);
        if (!todos.Any(t => t.Id == todo.Id))
        {
            return Results.NotFound($"Todo with id {todo.Id} not found.");
        }

        todos.RemoveAll(x => x.Id == id);
        todos.Add(todo);
        memoryCache.Set<List<Todo>>(TodosCacheKey, todos);
        return Results.Accepted($"/todo/{id}", todo);
    })
    .WithName("UpdateTodo")
    .Produces(StatusCodes.Status404NotFound)
    .Produces<Todo>(StatusCodes.Status202Accepted);

// Get a specific todo
app.MapGet("/todo/{id}", (IMemoryCache memoryCache, int id) =>
    {
        var todos = memoryCache.Get<List<Todo>>(TodosCacheKey);
        if (!todos.Any(t => t.Id == id))
        {
            return Results.NotFound($"Todo with id {id} not found.");
        }
        return Results.Ok(todos.First(x => x.Id == id));
    })
    .WithName("GetTodo")
    .Produces(StatusCodes.Status404NotFound)
    .Produces<Todo>(StatusCodes.Status200OK);

// Start the host and thus, the app
app.Run();
