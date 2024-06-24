using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Mercato.Pages.Player;

    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Players Player { get; set; }
        
        [BindProperty]
        public decimal Offer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Player = await _dbContext.Players.FindAsync(id);

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

            var clubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
            var toClub = JsonConvert.DeserializeObject<Club>(clubInfo);

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

