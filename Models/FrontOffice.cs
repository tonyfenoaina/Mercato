namespace Mercato.Models
{
    public class TransferRequest
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int FromClubId { get; set; }
        public Club FromClub { get; set; }
        public int ToClubId { get; set; }
        public Club ToClub { get; set; }
        public decimal Offer { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }  // Use an integer for status
    }

    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }

    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
    }
}
