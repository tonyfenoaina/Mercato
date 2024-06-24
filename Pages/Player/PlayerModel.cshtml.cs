using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mercato.Pages.Player;

    public class PlayerModel : PageModel
    {

         private readonly AppDbContext _dbContext;

         private readonly IHttpContextAccessor _httpContextAccessor;

    [BindProperty]
    public Players Players { get; set; }

    public PlayerModel(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
    }

    public void OnGet()
    {
          Console.Write("Bonjour ");
    }

    public async Task<IActionResult> OnPostAsync()
    {
         var clubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
          var club = JsonConvert.DeserializeObject<Club>(clubInfo);
        Players.ClubId = club.Id;
        Players.IsForTransfer= false;

        // if (!ModelState.IsValid)
        // {
        //     Console.Write("Bonjour tsyppoinsa"+Players.Name);
        //     return Page();
        // }

        _dbContext.Players.Add(Players);
        _dbContext.SaveChanges();

        // _dbContext.Club.Add(Club);
        // await _dbContext.SaveChangesAsync();
         Console.Write("Bonjour ");
        return RedirectToPage("./ListPlayers");
    }
    }
