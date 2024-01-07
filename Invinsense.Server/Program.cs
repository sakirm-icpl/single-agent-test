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
        runtimeIdentifier = "win-x64",
        downloadUrl = $"{host}/files/{osQueryname}/{osQueryname}-win-x64.zip",
        downloadFileName = $"{osQueryname}.zip",
        versionDetectionInstruction = new {
            type = "PROGRAM_REGISTRY",
            key = osQueryname,
            version = "5.10.2",
            minVersion = "5.10.2",
            maxVersion = "5.10.2"
        },
        installInstruction = new {
            installType = "INSTALLER",
            installerFile = $"{osQueryname}-5.10.2.msi",
            installArgs = new[] { "ALLUSERS=1", "ACCEPTEULA=1" },
            uninstallArgs = Array.Empty<object>()
        },
        description = $"{osQueryname} v5.10.2",
        isActive = true,
        updatedOn = new DateTime(2024, 1, 1)
    } },
    { sysmonName, new
    {
        name = sysmonName,
        group = 200,
        runtimeIdentifier = "win-x64",
        downloadUrl = $"{host}/files/{sysmonName}/{sysmonName}-win-x64.zip",
        downloadFileName = $"{sysmonName}.zip",
        versionDetectionInstruction = new {
            type = "SERVICE_REGISTRY",
            key = "Sysmon64",
            version = "15.11",
            minVersion = "15.11",
            maxVersion = "15.11"
        },
        installInstruction = new {
            installType = "EXECUTABLE",
            installerFile = "Sysmon64.exe",
            installArgs = new[] { "-accepteula", "-i", "C:\\Windows\\sysmonconfig.xml" },
            uninstallArgs = new[] { "-u" }
        },
        description = $"{sysmonName} v15.11",
        isActive = true,
        updatedOn = new DateTime(2024, 1, 1)
    }
    },
    { wazuhName, new
    {
        name = wazuhName,
        group = 300,
        runtimeIdentifier = "win-x86",
        downloadUrl = $"{host}/files/{wazuhName}/{wazuhName}-win-x86.zip",
        downloadFileName = $"{wazuhName}.zip",
        versionDetectionInstruction = new {
            type = "PROGRAM_REGISTRY",
            key = wazuhName,
            version = "4.7.1",
            minVersion = "4.7.1",
            maxVersion = "4.7.1"
        },
        installInstruction = new {
            installType = "INSTALLER",
            installerFile = $"{wazuhName}-agent-4.7.1.msi",
            installArgs = new[] { "ALLUSERS=1", "ACCEPTEULA=1", "WAZUH_MANAGER=\"65.1.109.28\"", "WAZUH_REGISTRATION_SERVER=\"65.1.109.28\"", "WAZUH_AGENT_GROUP={{reg64.local.SOFTWARE\\Infopercept.Groups}}" },
            uninstallArgs = Array.Empty<object>()
        },
        description = $"{wazuhName} v4.7.1",
        isActive = true,
        updatedOn = new DateTime(2024, 1, 1)
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

//Wazuh Params
//WAZUH_MANAGER=
//WAZUH_REGISTRATION_SERVER=
//WAZUH_AGENT_GROUP={{reg.local.SOFTWARE\\Infopercept.Groups}}
//REGISTRATION_TYPE = PASSWORD / CERTIFICATE
//WAZUH_REGISTRATION_PASSWORD
//WAZUH_REGISTRATION_CERTIFICATE
//WAZUH_REGISTRATION_CERTIFICATE_KEY
//<Environment Id="PATH" Name="PATH" Value="[INSTALLDIR]" Permanent="yes" Part="last" Action="set" System="yes" />
//<Environment Id="WAZUH_MANAGER" Name="WAZUH_MANAGER" Value="[WAZUH_MANAGER]" Permanent="yes" Part="last" Action="set" System="yes" />