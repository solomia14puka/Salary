using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Scientist
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "ПІБ науковця")]
    public string Fullname { get; set; } = null!;

    [Display(Name = "Код підрозділу")]
    public int? Departmentid { get; set; }

    [Required(ErrorMessage = "Зарплата обов'язкова")]
    [Display(Name = "Зарплата")]
    public decimal? Salary { get; set; }

    [Display(Name = "Дата створення запису")]
    public DateTime? Createdat { get; set; }

    [Display(Name = "Дата останнього оновлення")]
    public DateTime? Updatedat { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Salaryhistory> Salaryhistories { get; set; } = new List<Salaryhistory>();

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();
}
