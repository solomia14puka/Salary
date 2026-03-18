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

    public decimal? Newsalary { get; set; }

    public DateTime? Changedate { get; set; }

    public string? Reason { get; set; }

    [Display(Name = "Науковець")]
    public virtual Scientist? Scientist { get; set; }
}
