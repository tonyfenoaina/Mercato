using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; 

namespace Mercato.Pages.Backoffice.Clubs
{
    public class Edit : PageModel
    {
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public Club Clubtest { get; set; }

    public Edit(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult OnGet(int id)
    {
        Clubtest = _dbContext.Club.Find(id);
        
        Console.Write("Bonjour "+Clubtest.Name);
        if (Clubtest == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _dbContext.Attach(Clubtest).State = EntityState.Modified;
        _dbContext.SaveChanges();

        return RedirectToPage("./ListClubs");
    }
    }
}