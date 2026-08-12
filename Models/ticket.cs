using System;

namespace SmartDesk.Models
{
    public class Ticket
    {
        //  get pour lire le ticket, init pour qu'il puisse être crée mais plus modidifié par la suite 
        public int Id { get; init;}

        //  Valeur par défaut vide en cas d'omission de remplissage du titre 
        public string Title { get; init;} = string.Empty;

        //  Valeur par défaut vide en cas d'omission de remplissage de la description 
        public string Description { get; init;} = string.Empty;

        // Valeur par défaut "New" après la création du ticket 
        public string Status { get; set; } = "New";

        // Valeur par défaut "Medium" après la création du ticket 
        public string Priority { get; set; } = "Medium";

        // Valeur par défaut vide en cas d'omission de remplissage de l'auteur du ticket 
        public string SubmittedBy { get; init;} = string.Empty; 

        // Valeur par défaut en UtcNow soit l'heure à laquelle le ticket est crée 
        public DateTime CreatedAt { get; init;} = DateTime.UtcNow;

        // Valeur par défaut null en cas d'omission de remplissage du chemin de la capture d'écran
        public string? ScreenshotPath { get; init;} = null;
        public DateTime GetDeadline()
{

        if (Priority == "High")
            {
                return CreatedAt.AddHours(4);
            }

        if (Priority == "Medium")
            {
                return CreatedAt.AddHours(24);
            }
        
        else 
            {
                return CreatedAt.AddHours(48);
            }
}
}
}