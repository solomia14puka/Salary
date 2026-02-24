using System;
using System.Collections.Generic;

namespace LibraryInfrastructure.Models;

public partial class Salaryfund
{
    public int Id { get; set; }

    public DateOnly Periodstart { get; set; }

    public DateOnly Periodend { get; set; }

    public int? Facultyid { get; set; }

    public int? Departmentid { get; set; }

    public decimal? Totalamount { get; set; }

    public DateTime? Calculationdate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Faculty? Faculty { get; set; }
}
