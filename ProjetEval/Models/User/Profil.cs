using System;
using System.Collections.Generic;

namespace ProjetEval.Models.User
{
    public partial class Profil
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public DateTime Naissance { get; set; }
        public int? Idgenre { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
    }
}
