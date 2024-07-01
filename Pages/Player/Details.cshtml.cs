using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Mercato.Pages.Player
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Players Player { get; set; } // Le joueur

        public Club Club { get; set; } // Le club associé au joueur

         [BindProperty]
        public decimal Offer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Player = await _dbContext.Players
                .Include(p => p.Club) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Player == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
             Player = await _dbContext.Players.FindAsync(id);

            if (Player == null)
            {
                return NotFound();
            }

            if (Player == null)
            {
                return NotFound();
            }

            var clubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
            var toClub = JsonConvert.DeserializeObject<Club>(clubInfo);

            // Récupérer le budget actuel du club
            var currentClub = await _dbContext.Club.FindAsync(toClub.Id);
            if (currentClub == null)
            {
                return NotFound("Club non trouvé");
            }

            // Valider si l'offre dépasse le budget actuel du club
            if (Offer > currentClub.Budget)
            {
                 Player = await _dbContext.Players
                .Include(p => p.Club) 
                .FirstOrDefaultAsync(p => p.Id == id);
                ModelState.AddModelError("Offer", "Votre budget est insuffisant pour cette offre.");
                return Page();
            }

            // Créer une nouvelle demande de transfert
            var transferRequest = new TransferRequest
            {
                PlayerId = Player.Id,
                FromClubId = Player.ClubId,
                ToClubId = toClub.Id,
                Offer = Offer,
                Date = DateTime.Now,
                Status = TransferRequestStatus.Pending
            };

            _dbContext.TransferRequests.Add(transferRequest);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("./PlayerTransfertList");
        }
    }
}
