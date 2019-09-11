using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolovniAutomobiliZavrsniRad.Models
{
    [Table("Vozilo")]
    public partial class Vozilo
    {
        public Vozilo()
        {
            Komentars = new HashSet<Komentar>();
        }

        public int VoziloId { get; set; }
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public int TipVozilaId { get; set; }
        [Required]
        [StringLength(450)]
        public string KorisnikId { get; set; }
        [Required(ErrorMessage ="Morate uneti kubikazu.")]
        [StringLength(20)]
        public string Kubikaza { get; set; }
        [Required(ErrorMessage ="Morate uneti snagu.")]
        [StringLength(20)]
        public string Snaga { get; set; }
        [Required(ErrorMessage ="Morate uneti kilometrazu.")]
        [StringLength(6,ErrorMessage ="Nemoze da sadrzi vise od 6 cifara")]
        public string Kilometraza { get; set; }
        [Required(ErrorMessage ="Morate uneti pogon")]
        [StringLength(20)]
        public string Pogon { get; set; }
        [Required(ErrorMessage ="Morate uneti vrstu menjaca.")]
        [StringLength(20)]
        public string Menjac { get; set; }
        [Display(Name ="Broj Brzina")]
        [StringLength(1)]
        public string BrojBrzina { get; set; }
        [Required(ErrorMessage ="Unesite cenu.")]
        public int Cena { get; set; }
        public byte[] Slika { get; set; }
        [StringLength(20)]
        public string SlikaTip { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage ="Unesite opis.")]
        public string Opis { get; set; }

        [ForeignKey("MarkaId")]
        [InverseProperty("Vozilos")]
        public virtual Marka Marka { get; set; }
        [ForeignKey("ModelId")]
        [InverseProperty("Vozilos")]
        public virtual Modeli Modeli { get; set; }
        [ForeignKey("TipVozilaId")]
        [InverseProperty("Vozilos")]
        public virtual TipVozila TipVozila { get; set; }
        [InverseProperty("Vozilo")]
        public virtual ICollection<Komentar> Komentars { get; set; }
    }
}