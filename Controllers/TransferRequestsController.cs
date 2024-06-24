using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Mercato.Models;

namespace Mercato.Controllers
{
    public class TransferRequestsController : Controller
    {
        private readonly string _connectionString;

        public TransferRequestsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index(string searchString, int page = 1, int pageSize = 10)
        {
            var transferRequests = new List<TransferRequest>();
            int skip = (page - 1) * pageSize;

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                // Construction de la requête SQL avec pagination et recherche
                string sqlQuery = "SELECT tr.Id, tr.PlayerId, p.Name as PlayerName, p.Position, tr.FromClubId, fc.Name as FromClubName, tr.ToClubId, tc.Name as ToClubName, tr.Offer, tr.Date, tr.Status " +
                                  "FROM TransferRequests tr " +
                                  "JOIN Players p ON tr.PlayerId = p.Id " +
                                  "JOIN Clubs fc ON tr.FromClubId = fc.Id " +
                                  "JOIN Clubs tc ON tr.ToClubId = tc.Id ";

                if (!string.IsNullOrEmpty(searchString))
                {
                    sqlQuery += $"WHERE p.Name LIKE '%{searchString}%' OR fc.Name LIKE '%{searchString}%' OR tc.Name LIKE '%{searchString}%' ";
                }

                sqlQuery += $"ORDER BY tr.Date DESC " +
                            $"OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                SqlCommand command = new SqlCommand(sqlQuery, (SqlConnection)dbConnection);

                Console.Write(sqlQuery);

                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var transferRequest = new TransferRequest
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                            Player = new Players
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                                Name = reader.GetString(reader.GetOrdinal("PlayerName")),
                                Position = reader.GetString(reader.GetOrdinal("Position"))
                            },
                            FromClubId = reader.GetInt32(reader.GetOrdinal("FromClubId")),
                            FromClub = new Club
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FromClubId")),
                                Name = reader.GetString(reader.GetOrdinal("FromClubName"))
                            },
                            ToClubId = reader.GetInt32(reader.GetOrdinal("ToClubId")),
                            ToClub = new Club
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ToClubId")),
                                Name = reader.GetString(reader.GetOrdinal("ToClubName"))
                            },
                            Offer = reader.GetDecimal(reader.GetOrdinal("Offer")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Status = (TransferRequestStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                        };

                        transferRequests.Add(transferRequest);
                    }
                }
            }

            return View(transferRequests);
        }
    }
}
