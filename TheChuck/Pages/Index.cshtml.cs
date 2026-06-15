using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheChuck.Core;

namespace TheChuck.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IJokeService _jokeService;

        // Det här innebär att om man lägger till querystring, så kommer ASP.NET att "binda" dessa egenskaper. 
        // Testa att sätta en breakpoint i OnGet, surfa till https://localhost:7070/?Who=Pär&Category=Animals och undersök vad som står i dessa
        [BindProperty(SupportsGet = true)]
        public string? Who { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        // Här i konstruktorn injeceras våra beroenden via Dependency Injection
        // Observera att vi inte ber om JokeService, utan en IJokeServce (Interfacet)
        // Det skapar möjligt att i testsammanhang stoppa in något "Fejkat" så vi kan testa olika scenarion m.m. och gör oss oberoende av den riktiga tjänsten
        public IndexModel(ILogger<IndexModel> logger, IJokeService jokeService)
        {
            _logger = logger;
            _jokeService = jokeService;
        }

        // Den här används i vyn (det som skapar utseendet på websidan)
        public string DisplayText { get; private set; } = "";

        //Den här körs varje gång någon surfar till sidan
        public async Task OnGet()
        {
            try
            {
                Joke? joke;
                if (Category != null) joke = await _jokeService.GetJokeFromCategory(Category);
                else joke = await _jokeService.GetRandomJoke();

                DisplayText = joke?.Value ?? "";

                if (Who != null) DisplayText = DisplayText.Replace("Chuck Norris", Who);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // Att felhanteringen fungerar korrekt är något som också är viktigt att testa
                DisplayText = "Något gick fel. Försök igen lite senare.";
            }

            DisplayText = DisplayText.ToUpper();
        }
    }
}