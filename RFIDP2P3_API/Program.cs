using RFIDP2P3_API.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options => {
        options.AddDefaultPolicy(
            builder => {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }
        );
    }
);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(
     options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; }
 );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RFIDP2P3_API", Version = "v1" });
    options.AddSecurityDefinition("XApiKey", new OpenApiSecurityScheme
    {
        Name = "XApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        In = ParameterLocation.Header,
        Description = "API Key Must Apper in Header"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "XApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement{{ key, new List<string>() }};
    options.AddSecurityRequirement(requirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<APIKeyMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
