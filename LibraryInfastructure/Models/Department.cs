using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть назву")]
    [Display(Name = "Назва кафедри")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Оберіть факультет")]
    [Display(Name = "Факультет")]
    public int? Facultyid { get; set; }

    [Display(Name = "Дата створення запису")]
    public DateTime? Createdat { get; set; }

    [Display(Name = "Дата останнього оновлення")]
    public DateTime? Updatedat { get; set; }

    [Display(Name = "Факультет")]
    public virtual Faculty? Faculty { get; set; }

    public virtual ICollection<Salaryfund> Salaryfunds { get; set; } = new List<Salaryfund>();

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();

    public virtual ICollection<Scientist> Scientists { get; set; } = new List<Scientist>();
}
