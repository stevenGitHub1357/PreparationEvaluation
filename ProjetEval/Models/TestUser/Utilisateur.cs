using System;
using System.Collections.Generic;

namespace ProjetEval.Models.TestUser;

public partial class Utilisateur
{
    public int Id { get; set; }

    public string? Adress { get; set; }

    public string Mdp { get; set; } = null!;

    public int? Idprofil { get; set; }

    public string? Recuperation { get; set; }

    public virtual Profil? IdprofilNavigation { get; set; }
}
