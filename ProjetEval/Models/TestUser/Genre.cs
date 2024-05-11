using System;
using System.Collections.Generic;

namespace ProjetEval.Models.TestUser;

public partial class Genre
{
    public int Id { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Profil> Profils { get; } = new List<Profil>();
}
