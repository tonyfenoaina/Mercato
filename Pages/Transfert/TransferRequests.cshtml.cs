using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mercato.Pages
{
    public class TransferRequestsModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransferRequestsModel(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<TransferRequest> TransferRequests { get; set; }
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm, int pageIndex = 1)
        {
            var clubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
            if (string.IsNullOrEmpty(clubInfo))
            {
                return RedirectToPage("/Login"); // Redirige vers la page de connexion si le club n'est pas connecté
            }

            var club = JsonConvert.DeserializeObject<Club>(clubInfo);
            var clubId = club.Id;

            IQueryable<TransferRequest> query = _dbContext.TransferRequests
                .Include(tr => tr.Player)
                .Include(tr => tr.ToClub)
                .Where(tr => tr.FromClubId == clubId && tr.Status == TransferRequestStatus.Pending);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(tr => tr.Player.Name.Contains(searchTerm));
            }

            int pageSize = 10;
            int count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            TransferRequests = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            SearchTerm = searchTerm;
            PageIndex = pageIndex;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int requestId, int status)
        {
            var request = await _dbContext.TransferRequests.FindAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }

            if (status == 1) // Si la demande est validée
            {
                // Mettre à jour le statut
                request.Status = TransferRequestStatus.Accepted;

                // Mettre à jour le club du joueur
                var player = await _dbContext.Players.FindAsync(request.PlayerId);
                if (player != null)
                {
                    player.ClubId = request.ToClubId;
                }

                // Mettre à jour les budgets des clubs
                var fromClub = await _dbContext.Club.FindAsync(request.FromClubId);
                var toClub = await _dbContext.Club.FindAsync(request.ToClubId);
                if (fromClub != null && toClub != null)
                {
                    if (fromClub.Budget >= request.Offer)
                    {
                        fromClub.Budget -= request.Offer; // Déduction du budget du club vendeur
                        toClub.Budget += request.Offer;   // Ajout au budget du club acheteur
                    }
                    else
                    {
                        ModelState.AddModelError("", "Le budget du club vendeur est insuffisant.");
                        return await OnGetAsync(SearchTerm, 1); // Retourne à la page avec l'erreur
                    }
                }
                var currentClubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
                if (!string.IsNullOrEmpty(currentClubInfo))
                {
                    var club = JsonConvert.DeserializeObject<Club>(currentClubInfo);
                    if (club.Id == fromClub.Id)
                    {
                        club.Budget -= request.Offer; // Déduire du budget du club actuel
                    }
                    else if (club.Id == toClub.Id)
                    {
                        club.Budget += request.Offer; // Ajouter au budget du club actuel
                    }

                    // Mettre à jour les informations du club dans les cookies
                    var clubInfoJson = JsonConvert.SerializeObject(club);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("ClubInfo", clubInfoJson);
                }
            }
            else if (status == 2) // Si la demande est refusée
            {
                // Mettre à jour le statut
                request.Status = TransferRequestStatus.Rejected;
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToPage(new { searchTerm = SearchTerm, pageIndex = 1 });
        }
    }
}
