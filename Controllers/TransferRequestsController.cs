using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mercato.Controllers
{
    [Route("Transfers")]
    public class TransferRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public TransferRequestsController(AppDbContext context)
        {
            _context = context;
        }

         [HttpGet("AllTransferRequests")]
        public async Task<IActionResult> AllTransferRequests(string searchTerm, int pageIndex = 1, int pageSize = 4)
        {
            IQueryable<TransferRequest> query = _context.TransferRequests
                .Include(tr => tr.Player)
                .Include(tr => tr.FromClub)
                .Include(tr => tr.ToClub);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(tr => tr.Player.Name.Contains(searchTerm));
            }

            int totalItems = await query.CountAsync();
            var transferRequests = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new TransferRequestViewModel
            {
                TransferRequests = transferRequests,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(model);
        }
    }
    public class TransferRequestViewModel
    {
        public List<TransferRequest> TransferRequests { get; set; }
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
    }

