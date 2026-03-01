using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Position
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Basesalary { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();
}
