using System;
using System.ComponentModel.DataAnnotations;

namespace SmartDesk.Models
{
    public class MissingProcedure
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty; // Le texte que l'utilisateur va taper 

        public string? SearchKeyword { get; set; } = string.Empty; // Optionnel : on sauvegarde ce qu'il avait cherché s'il a utilisé la barre

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Pour savoir quand ça a été demandé

        public bool IsResolved { get; set; } = false; // Le technicien cochera ça quand il aura créé la procédure
    }
}