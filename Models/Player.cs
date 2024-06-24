
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("Players")]
public class Players
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public decimal Value { get; set; }
    public bool IsForTransfer { get; set; }
    public int ClubId { get; set; }
    public Club Club { get; set; }

}
