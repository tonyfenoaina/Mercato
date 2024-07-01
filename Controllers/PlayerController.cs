using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercato.Models;

namespace Mercato.Controllers
{
    public class PlayerController : Controller
    {
        private readonly AppDbContext _dbContext;

        public PlayerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> ListPlayersBtClub(int clubId)
        {
            // Récupérer le club spécifique avec ses joueurs
            var club = await _dbContext.Club.Include(c => c.Players)
                                             .FirstOrDefaultAsync(c => c.Id == clubId);

            if (club == null)
            {
                return NotFound();
            }

            // Passer les joueurs du club à la vue
             return View("ListPlayersBtClub", club.Players);
        }
    }
}
