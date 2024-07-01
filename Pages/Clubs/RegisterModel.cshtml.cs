using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mercato.Service;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mercato.Pages.Clubs
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        [BindProperty]
        public Club Club { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; } // Propriété pour le fichier téléchargé

        public RegisterModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Console.Write("Bonjour ");
        }

        public async Task<IActionResult> OnPostAsync()
        {
           

            if (Image != null)
            {
                // Enregistrer l'image sur le serveur
                var fileName = Path.GetFileName(Image.FileName);
                var filePath = Path.Combine("wwwroot/images/clubs", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                Club.Image = fileName; // Enregistrer le nom de l'image dans la base de données
            }

            //  if (!ModelState.IsValid)
            // {
            //     Console.Write("Bonjour tsyppoinsa" + Club.Image);
            //     return Page();
            // }

            _dbContext.Club.Add(Club);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
