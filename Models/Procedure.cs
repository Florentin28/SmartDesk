using System;

namespace SmartDesk.Models
{
    public class Procedure
    {
        // 
        public int Id { get; init; }
        
        // Les 3 niveaux de l'entonnoir (Valeur par défaut vide pour éviter les valeurs nulles)
        public string Category { get; init; } = string.Empty;   // Par exemple : "Hardware", "Software", "Network"  
        public string SubCategory { get; init; } = string.Empty; // Par exemple : "Printer", "Operating System", "Router"
        public string Title { get; init; } = string.Empty;   // Titre de la procédure
        
        // La procédure à suivre pour résoudre le problème
        public string SolutionSteps { get; init; } = string.Empty; 
        
        // Compteur pour savoir si la procédure à été utile ou non pour l'utilisateur
        public int HelpfulCount { get; set; } = 0; 
        public int FailedCount { get; set; } = 0;  
    }
}