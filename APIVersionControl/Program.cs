using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using APIVersionControl;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 1. Add HttpClient to send HttpRequests in controller
builder.Services.AddHttpClient();

// 2. Add App version Control
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;

});

// 3. Add configuration to document versions
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV"; // 1.0.0 1.0.1
    setup.SubstituteApiVersionInUrl = true;

});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Configure Options
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// 5. Configure EndPoints for swagger DOCS for each of the versions of our API
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    // 6. Configure Swagger DOCS
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            //$"/swagger/{description.GroupName}/swagger.json",
            //options.SwaggerEndpoint(
            //    description.GroupName.ToUpperInvariant()
            //);   
            options.SwaggerEndpoint(
               $"/swagger/{description.GroupName}/swagger.json",
               description.GroupName.ToUpperInvariant()
           );
        }
    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//  Esto permite que funcione en Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
