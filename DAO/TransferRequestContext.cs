using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class TransferRequestContext
{
    private readonly string _connectionString;

    public TransferRequestContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<TransferRequest>> GetTransferRequestsAsync()
    {
        var transferRequests = new List<TransferRequest>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT tr.*, p.Name as PlayerName, fc.Name as FromClubName, tc.Name as ToClubName " +
                        "FROM TransferRequests tr " +
                        "JOIN Players p ON tr.PlayerId = p.Id " +
                        "JOIN Clubs fc ON tr.FromClubId = fc.Id " +
                        "JOIN Clubs tc ON tr.ToClubId = tc.Id";

            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var transferRequest = new TransferRequest
                        {
                            Id = reader.GetInt32(0),
                            PlayerId = reader.GetInt32(1),
                            FromClubId = reader.GetInt32(2),
                            ToClubId = reader.GetInt32(3),
                            Offer = reader.GetDecimal(4),
                            Date = reader.GetDateTime(5),
                            Status = (TransferRequestStatus)reader.GetInt32(6),
                            Player = new Players { Name = reader.GetString(7) },
                            FromClub = new Club { Name = reader.GetString(8) },
                            ToClub = new Club { Name = reader.GetString(9) }
                        };
                        transferRequests.Add(transferRequest);
                    }
                }
            }
        }

        return transferRequests;
    }

    // Add other CRUD operations as needed
}
