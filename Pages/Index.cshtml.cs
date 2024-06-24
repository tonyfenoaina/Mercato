using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mercato.Service;
using System.Text.Json;

namespace Mercato.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _dbContext;
    private readonly ClubAuthenticationService _clubAuthService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public IndexModel(AppDbContext dbContext,ClubAuthenticationService clubAuthService, ILogger<IndexModel> logger)
    {
        _dbContext = dbContext;
        _clubAuthService = clubAuthService;
        _logger = logger;
    }

    public List<Person> People { get; set; }

    public async Task OnGetAsync()
    {
        People = await _dbContext.People.ToListAsync();
    }

    // public async Task<IActionResult> OnPostAsync(string firstName, string lastName)
    // {
    //     var person = new Person { FirstName = firstName, LastName = lastName };
    //     _dbContext.People.Add(person);
    //     await _dbContext.SaveChangesAsync();

    //     return RedirectToPage();
    // }

    public IActionResult OnPost()
    {
         var club = _clubAuthService.AuthenticateClub(Username, Password);
        if (club != null)
        {
            // Authentification réussie, redirigez vers la page de liste ou toute autre page souhaitée
             Console.Write("Bonjour ");
              TempData["ClubInfo"] = JsonSerializer.Serialize(club);
            return RedirectToPage("./Player/ListPlayers");
        }
        else
        { 
            // Authentification échouée, restez sur la page de connexion avec un message d'erreur
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return Page();
        }
    }
}
