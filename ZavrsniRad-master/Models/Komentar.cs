using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolovniAutomobiliZavrsniRad.Models
{
    [Table("Komentar")]
    public partial class Komentar
    {
        public int KomentarId { get; set; }
        public int VoziloId { get; set; }
        [Required]
        [StringLength(450)]
        public string KorisnikId { get; set; }
        [Required]
        [StringLength(20)]
        public string Korisnik { get; set; }
        [Required(ErrorMessage ="Morate uneti komentar")]
        [StringLength(300,ErrorMessage ="Minimalno 3 karaktera, maksimalno 300",MinimumLength =3)]
        public string Opis { get; set; }

        [ForeignKey("VoziloId")]
        [InverseProperty("Komentars")]
        public virtual Vozilo Vozilo { get; set; }
    }
}