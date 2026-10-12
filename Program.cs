var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

try
{
   var testConnectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(testConnectionString))
    {
        throw new InvalidOperationException(
            "DefaultConnection is missing from appsettings.json");
    }

    await using var connection =
         new MySqlConnector.MySqlConnection(testConnectionString);

    await connection.OpenAsync();

    Console.WriteLine("MySQL connection successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"MySQL connection failed: {ex.Message}");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
