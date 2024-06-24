using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


   public class TransferRequest
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Players Player { get; set; }
    public int FromClubId { get; set; }
    public Club FromClub { get; set; }
    public int ToClubId { get; set; }
    public Club ToClub { get; set; }
    public decimal Offer { get; set; }
    public DateTime Date { get; set; }
    public TransferRequestStatus Status { get; set; }  // Etat de la demande de transfert
}

