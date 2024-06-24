
using System.ComponentModel.DataAnnotations.Schema;

    [Table("Clubs")]
    public class Club
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Code { get; set; }
    public decimal Budget { get; set; }
    //  public List<Player> Players { get; set; }
    }

