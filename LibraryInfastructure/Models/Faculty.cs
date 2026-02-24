using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Faculty
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Salaryfund> Salaryfunds { get; set; } = new List<Salaryfund>();
}
