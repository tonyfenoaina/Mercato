using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercato.Models;

   public class PlayerImportModel
{
    public string Name { get; set; }
    public string Position { get; set; }
    public decimal Value { get; set; }
    public bool IsForTransfer { get; set; }
}
