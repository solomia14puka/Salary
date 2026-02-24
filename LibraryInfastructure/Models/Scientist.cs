using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Scientist
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public int? Departmentid { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Salaryhistory> Salaryhistories { get; set; } = new List<Salaryhistory>();

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();
}
