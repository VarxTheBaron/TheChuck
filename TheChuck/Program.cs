
using TheChuck.Core;
using TheChuck.Infrastructure;
using TheChuck.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// DEPENDENCY INJECTION
// När koden ber om en IJokeService, så skall den få en JokeService
// I testprojektet stoppar vi in något annat för att INTE anropa api.chucknorris.io. Vi kan på så sätt kontrollera vad vi testar oavsett vad den riktiga tjänsten svarar.
// Vi är ju bara intresserade av att testa att vår tjänst fungerar och gör rätt. Vi vill inte testat api.chucknorris.io.
builder.Services.AddScoped<IJokeService, JokeService>();
builder.Services.AddScoped<IWebClient, WebClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

Console.WriteLine("Jag gillar att pusha ändringar");

app.Run();
