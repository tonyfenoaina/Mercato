using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mercato.Controllers
{
    public class ClubsController : Controller
    {
        private readonly AppDbContext _context;

        public ClubsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Club.ToListAsync();
            return View(clubs);
        }
    }
}
