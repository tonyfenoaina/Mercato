using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mercato.Pages.Backoffice.Clubs
{
    public class Create : PageModel
    {
        
    private readonly AppDbContext _dbContext;

    public Create(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public Club Club { get; set; }

      public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _dbContext.Club.Add(Club);
        _dbContext.SaveChanges();

        return RedirectToPage("./ListClubs");
    }

      
    }
}