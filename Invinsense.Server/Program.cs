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

var tools = new Dictionary<string, dynamic>
{
    { "osquery", new
    {
        name = "osquery",
        group = 400,
        description = "OSQuery v5.10.2",
        minVersion = "5.10.2",
        maxVersion = "5.10.2",
        version = "5.10.2",
        runtimeIdentifier = "win-x64",
        downloadUrl = "http://localhost:5197/files/osquery/osquery-win-x64.zip",
        destinationPath = "osquery.zip",
        isActive = true
    } },
    { "sysmon", new
    {
        name = "sysmon",
        group = 200,
        description = "Sysmon v13.10",
        minVersion = "13.10",
        maxVersion = "13.10",
        version = "13.10",
        runtimeIdentifier = "win-x64",
        downloadUrl = "http://localhost:5197/files/sysmon/sysmon-win-x64.zip",
        destinationPath = "sysmon.zip",
        isActive = true
    }
    },
    { "wazuh", new
    {
        name = "wazuh",
        group = 300,
        description = "Wazuh v4.7.1",
        minVersion = "4.7.1",
        maxVersion = "4.7.1",
        version = "4.7.1",
        runtimeIdentifier = "win-x86",
        downloadUrl = "http://localhost:5197/files/wazuh/wazuh-win-x86.zip",
        destinationPath = "wazuh.zip",
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


app.MapGet("/api/tools/osquery", () =>
{
    return tools["osquery"];
})
    .WithName("TOOLS_OSQUERY")
    .WithOpenApi();

app.MapGet("/api/tools/sysmon", () =>
{
    return tools["sysmon"];
})
    .WithName("TOOLS_SYSMON")
    .WithOpenApi();

app.MapGet("/api/tools/wazuh", () =>
{
    return tools["wazuh"];
})
    .WithName("TOOLS_WAZUH")
    .WithOpenApi();

app.Run();