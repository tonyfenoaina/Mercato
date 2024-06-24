using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Mercato.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mercato.Pages.Player;
public class ImportPlayersModel : PageModel
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImportPlayersModel(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> OnPostAsync(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Veuillez sélectionner un fichier CSV.");
            return Page();
        }

        try
        {
            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var clubInfo = _httpContextAccessor.HttpContext.Request.Cookies["ClubInfo"];
                var club = JsonConvert.DeserializeObject<Club>(clubInfo);


                var records = csv.GetRecords<PlayerImportModel>(); // Lire les enregistrements CSV en tant que liste de joueurs pour l'importation

                var players = records.Select(p => new Players
                {
                    Name = p.Name,
                    Position = p.Position,
                    Value = p.Value,
                    IsForTransfer = p.IsForTransfer,
                    ClubId = club.Id
                }).ToList();

                // Ajouter les joueurs à la base de données
                await _dbContext.Players.AddRangeAsync(players);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erreur lors de l'import du fichier CSV : {ex.Message}");
            return Page();
        }

        return RedirectToPage("./ListPlayers"); // Rediriger vers la liste des joueurs après l'import
    }

}
