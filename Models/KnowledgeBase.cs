using System;

namespace SmartDesk.Models
{
    public class KnowledgeBase
    {
        public int Id { get; set; }
        
        // Les 3 niveaux de notre entonnoir 
        public string Category { get; set; } = string.Empty;     // ex: Écran
        public string SubCategory { get; set; } = string.Empty;  // ex: Problème physique
        public string Title { get; set; } = string.Empty;        // ex: L'écran clignote
        
        // Le contenu de la solution
        public string SolutionSteps { get; set; } = string.Empty; // Les étapes textuelles pour réparer
        
        // Nos compteurs de performance (Statistiques)
        public int HelpfulCount { get; set; } = 0; // Nombre de personnes aidées
        public int FailedCount { get; set; } = 0;  // Nombre de personnes pour qui ça n'a pas marché
    }
}