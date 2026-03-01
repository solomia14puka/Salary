using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Salaryhistory
{
    public int Id { get; set; }

    public int? Scientistid { get; set; }

    public decimal? Oldsalary { get; set; }

    public decimal? Newsalary { get; set; }

    public DateTime? Changedate { get; set; }

    public string? Reason { get; set; }

    public virtual Scientist? Scientist { get; set; }
}
