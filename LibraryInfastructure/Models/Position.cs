using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.Models;

public partial class Position
{
    public int Id { get; set; }

    [Display(Name = "Посада")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Введіть базову ставку")]
    [Display(Name = "Базова ставка (грн)")]
    public decimal Basesalary { get; set; }

    [Display(Name = "Дата створення запису")]
    public DateTime? Createdat { get; set; }

    [Display(Name = "Дата останнього оновлення")]
    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Scientistposition> Scientistpositions { get; set; } = new List<Scientistposition>();
}
