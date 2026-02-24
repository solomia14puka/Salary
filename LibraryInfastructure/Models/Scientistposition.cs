using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Scientistposition
{
    public int Id { get; set; }

    public int? Scientistid { get; set; }

    public int? Positionid { get; set; }

    public int? Departmentid { get; set; }

    public decimal? Employmentrate { get; set; }

    public DateOnly Startdate { get; set; }

    public DateOnly? Enddate { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Position? Position { get; set; }

    public virtual Scientist? Scientist { get; set; }
}
