using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolovniAutomobiliZavrsniRad.Models
{
    [Table("Marka")]
    public partial class Marka
    {
        public Marka()
        {
            Modelis = new HashSet<Modeli>();
            Vozilos = new HashSet<Vozilo>();
        }

        public int MarkaId { get; set; }
        [Required(ErrorMessage ="Unesite Naziv.")]
        [StringLength(50, ErrorMessage = "Naziv moze da imam maksimum 50 znakova a minimalno 3", MinimumLength = 3)]
        public string Naziv { get; set; }

        [InverseProperty("Marka")]
        public virtual ICollection<Modeli> Modelis { get; set; }
        [InverseProperty("Marka")]
        public virtual ICollection<Vozilo> Vozilos { get; set; }
    }
}