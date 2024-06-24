using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mercato.Pages.Player
{
    public class PlayerTransfertList : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerTransfertList(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Players> Players { get; set; }
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

            IQueryable<Players> query = _dbContext.Players
                .Include(p => p.Club)
                .Where(p => p.ClubId != clubId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            int pageSize = 10;
            int count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Players = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            SearchTerm = searchTerm;
            PageIndex = pageIndex;

            return Page();
        }
    }
}
