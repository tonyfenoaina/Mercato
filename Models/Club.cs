
using System.ComponentModel.DataAnnotations.Schema;

    [Table("Clubs")]
    public class Club
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Code { get; set; }
    public decimal Budget { get; set; }
    public string Image { get; set;}

      public List<Players> Players { get; set; }
    }

