// OpenAPI description constants
const string OpenApiDocName = "TodoApi";
const string OpenApiDocVersion = "2021-09-01";

var builder = WebApplication.CreateBuilder(args);

// Enable OpenAPI description generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(OpenApiDocName, new() { Title = "TodoApi", Version = OpenApiDocVersion });
});

// Build the app host
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{OpenApiDocName}/swagger.json", OpenApiDocName));
}

// Start the host and thus, the app
app.Run();

// The Todo model
public record Todo(int Id, string Title);