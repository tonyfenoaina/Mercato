using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Mercato.Service
{
    public class ClubAuthenticationService
    {
         private readonly AppDbContext _dbContext;
         private readonly IHttpContextAccessor _httpContextAccessor;

    public ClubAuthenticationService(AppDbContext dbContext,IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public Club AuthenticateClub(string username, string password)
    {
        // Vérifiez si le club existe avec le nom d'utilisateur et le mot de passe fournis
        var club = _dbContext.Club.SingleOrDefault(c => c.Code == username && c.Password == password);

    var clubInfo = JsonConvert.SerializeObject(club);
        var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(1)
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("ClubInfo", clubInfo, cookieOptions);

        return club ;
    }
    }
}