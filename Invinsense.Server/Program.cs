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

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/tools/osquery", () =>
{
    return new
    {
        name = "osquery",
        group = 400,
        description = "OSQuery v5.10.2",
        minVersion = "5.10.2",
        maxVersion = "5.10.2",
        version = "5.10.2",
        runtimeIdentifier = "win-x64",
        downloadUrl = "http://localhost:5197/files/osquery/osquery-win-x64.zip",
        destinationPath = "artifacts\\osquery",
        isActive = true
    };
})
.WithName("TOOLS_OSQUERY")
.WithOpenApi();

app.MapGet("/api/tools/sysmon", () =>
{
    return new
    {
        name = "sysmon",
        group = 300,
        description = "Microsoft Sysmon v15.11",
        minVersion = "15.11",
        maxVersion = "15.11",
        version = "15.11",
        runtimeIdentifier = "win-x64",
        downloadUrl = "http://localhost:5197/files/sysmon/sysmon-win-x64.zip",
        destinationPath = "artifacts\\sysmon",
        isActive = true
    };
})
.WithName("TOOLS_SYSMON")
.WithOpenApi();

app.MapGet("/api/tools/wazuh", () =>
{
    return new
    {
        name = "osquery",
        group = 100,
        description = "Wazuh Windows Agent v4.7",
        minVersion = "4.7.1",
        maxVersion = "4.7.1",
        version = "4.7.1",
        runtimeIdentifier = "win-x86",
        downloadUrl = "http://localhost:5197/files/wazuh/wazuh-win-x86.zip",
        destinationPath = "artifacts\\wazuh",
        isActive = true
    };
})
.WithName("TOOLS_WAZUH")
.WithOpenApi();

app.Run();
