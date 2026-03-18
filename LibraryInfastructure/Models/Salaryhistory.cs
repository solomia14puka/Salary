using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Salaryhistory
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Оберіть науковця")]
    [Display(Name = "Науковець")]
    public int? Scientistid { get; set; }

    [Required(ErrorMessage = "Введіть суму виплати")]
    [Display(Name = "Сума виплати (грн)")]
    [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за нуль")]
    public decimal? Oldsalary { get; set; }

    [Display(Name = "Оновлена сума виплати")]
    [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за нуль")]
    public decimal? Newsalary { get; set; }

    [Required(ErrorMessage = "Вкажіть дату виплати")]
    [Display(Name = "Дата виплати")]
    [DataType(DataType.Date)]
    public DateTime? Changedate { get; set; }

    [Display(Name = "Причина зміни")]
    public string? Reason { get; set; }

    [Display(Name = "Науковець")]
    public virtual Scientist? Scientist { get; set; }
}
