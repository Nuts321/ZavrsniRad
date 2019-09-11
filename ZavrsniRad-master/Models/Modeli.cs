using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolovniAutomobiliZavrsniRad.Models
{
    [Table("Modeli")]
    public partial class Modeli
    {
        public Modeli()
        {
            Vozilos = new HashSet<Vozilo>();
        }

        public int ModelId { get; set; }
        public int MarkaId { get; set; }
        [Required(ErrorMessage = "Unesite Naziv.")]
        [StringLength(50, ErrorMessage = "Naziv moze da imam maksimum 50 znakova a minimalno 3", MinimumLength = 3)]
        public string Naziv { get; set; }

        [ForeignKey("MarkaId")]
        [InverseProperty("Modelis")]
        public virtual Marka Marka { get; set; }
        [InverseProperty("Modeli")]
        public virtual ICollection<Vozilo> Vozilos { get; set; }
    }
}