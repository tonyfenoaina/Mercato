using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mercato.Pages.Backoffice.Clubs
{
public class Delete : PageModel
{
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public Club Club { get; set; }

    public Delete(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult OnGet(int id)
    {
        Club = _dbContext.Club.Find(id);

        if (Club == null)
        {
            return NotFound();
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        _dbContext.Club.Remove(Club);
        _dbContext.SaveChanges();
        return RedirectToPage("./ListClubs");
    }
}

}