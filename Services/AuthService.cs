namespace SmartDesk.Services
{
    public class AuthService
    {
        // Ces variables stockent les infos de l'utilisateur connecté en mémoire vive
        public string CurrentEmail { get; private set; } = string.Empty;
        public bool IsLoggedIn { get; private set; } = false;
        public bool IsTechnician { get; private set; } = false;

        // Méthode pour se connecter
        public void Login(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            CurrentEmail = email;
            IsLoggedIn = true;

            // Logique métier : si l'email contient "tech@", c'est un technicien
            if (email.ToLower().StartsWith("tech@"))
            {
                IsTechnician = true;
            }
            else
            {
                IsTechnician = false;
            }
        }

        // Méthode pour se déconnecter
        public void Logout()
        {
            CurrentEmail = string.Empty;
            IsLoggedIn = false;
            IsTechnician = false;
        }
    }
}