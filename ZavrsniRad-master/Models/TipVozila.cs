using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolovniAutomobiliZavrsniRad.Models
{
    [Table("TipVozila")]
    public partial class TipVozila
    {
        public TipVozila()
        {
            Vozilos = new HashSet<Vozilo>();
        }

        public int TipVozilaId { get; set; }
        [Required(ErrorMessage = "Unesite Naziv.")]
        [StringLength(50,ErrorMessage ="Naziv moze da imam maksimum 50 znakova a minimalno 3",MinimumLength =3)]
        public string Naziv { get; set; }

        [InverseProperty("TipVozila")]
        public virtual ICollection<Vozilo> Vozilos { get; set; }
    }
}