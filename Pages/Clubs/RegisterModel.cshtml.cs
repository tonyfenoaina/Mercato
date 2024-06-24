using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mercato.Service; 

namespace Mercato.Pages.Clubs;

    public class RegisterModel : PageModel
    {
         private readonly AppDbContext _dbContext;

    [BindProperty]
    public Club Club { get; set; }

    public RegisterModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnGet()
    {
          Console.Write("Bonjour ");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Console.Write("Bonjour tsyppoinsa"+Club.Code);
            return Page();
        }

        _dbContext.Club.Add(Club);
        _dbContext.SaveChanges();

        // _dbContext.Club.Add(Club);
        // await _dbContext.SaveChangesAsync();
         Console.Write("Bonjour ");
        return RedirectToPage("../Backoffice/Clubs/ListClubs");
    }
    }
