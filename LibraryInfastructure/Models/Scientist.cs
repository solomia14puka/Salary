using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Scientist
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "ПІБ науковця")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "ПІБ має містити від 2 до 255 символів")]
    public string Fullname { get; set; } = null!;

    [Required(ErrorMessage = "Оберіть кафедру")]
    [Display(Name = "Кафедра")]
    public int? Departmentid { get; set; }

    [Required(ErrorMessage = "Введіть базову ставку")]
    [Display(Name = "Базова ставка (грн)")]
    [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за нуль")]
    public decimal? Salary { get; set; }

    [Display(Name = "Дата створення запису")]
    public DateTime? Createdat { get; set; }

    [Display(Name = "Дата останнього оновлення")]
    public DateTime? Updatedat { get; set; }

    [Display(Name = "Кафедра")]
    public virtual Department? Department { get; set; }

    public virtual ICollection<Salaryhistory> Salaryhistories { get; set; } = new List<Salaryhistory>();

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();
}
