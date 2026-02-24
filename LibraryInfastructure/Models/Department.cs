using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Facultyid { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Faculty? Faculty { get; set; }

    public virtual ICollection<Salaryfund> Salaryfunds { get; set; } = new List<Salaryfund>();

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();

    public virtual ICollection<Scientist> Scientists { get; set; } = new List<Scientist>();
}
