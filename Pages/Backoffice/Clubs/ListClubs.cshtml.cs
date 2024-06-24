
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Mercato.Pages.Backoffice.Clubs;

    public class ListClubs : PageModel
    {
    private readonly AppDbContext _dbContext;

    public ListClubs(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

       public List<Club> Club { get; set; }

    //      public async Task OnGetAsync()
    // {
    //     Club = await _dbContext.Club.ToListAsync();
    // }

        public void OnGet()
        {
             Club = _dbContext.Club.ToList();
        }
    }
