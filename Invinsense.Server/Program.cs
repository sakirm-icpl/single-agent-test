using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseStaticFiles(); // For serving static files
app.UseDirectoryBrowser(); // If you want to enable directory browsing

app.UseFileServer(new FileServerOptions
{
    EnableDirectoryBrowsing = true,
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/files"
});

var host = "https://65.1.109.28:5001";

var osQueryname = "osquery";
var sysmonName = "sysmon";
var wazuhName = "wazuh";

var tools = new Dictionary<string, dynamic>
{
    { osQueryname, new
    {
        name = osQueryname,
        group = 400,
        description = $"{osQueryname} v5.10.2",
        minVersion = "5.10.2",
        maxVersion = "5.10.2",
        version = "5.10.2",
        runtimeIdentifier = "win-x64",
        downloadUrl = $"{host}/files/{osQueryname}/{osQueryname}-win-x64.zip",
        destinationPath = $"{osQueryname}.zip",
        isActive = true
    } },
    { sysmonName, new
    {
        name = sysmonName,
        group = 200,
        description = $"{sysmonName} v15.11",
        minVersion = "15.11",
        maxVersion = "15.11",
        version = "15.11",
        runtimeIdentifier = "win-x64",
        downloadUrl = $"{host}/files/{sysmonName}/{sysmonName}-win-x64.zip",
        destinationPath = $"{sysmonName}.zip",
        isActive = true
    }
    },
    { wazuhName, new
    {
        name = wazuhName,
        group = 300,
        description = $"{wazuhName} v4.7.1",
        minVersion = "4.7.1",
        maxVersion = "4.7.1",
        version = "4.7.1",
        runtimeIdentifier = "win-x86",
        downloadUrl = $"{host}/files/{wazuhName}/{wazuhName}-win-x86.zip",
        destinationPath = $"{wazuhName}.zip",
        isActive = true
    }
    }
};

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/tools", () =>
{
    return tools;
})
    .WithName("TOOLS")
    .WithOpenApi();

app.Run();