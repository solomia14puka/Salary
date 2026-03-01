using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Faculty
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть назву")]
    [Display(Name = "Назва факультету")]
    public string Name { get; set; } = null!;

    [Display(Name = "Дата створення запису")]
    public DateTime? Createdat { get; set; }

    [Display(Name = "Дата останнього оновлення")]
    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Salaryfund> Salaryfunds { get; set; } = new List<Salaryfund>();
}
