namespace SpringBoard.Model
{
    public class ConsultantModel
    {
        
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string role { get; set; }

        public string commid { get; set; }

        public string competence { get; set; }

        public bool valid()
        {
            return (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) 
                && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(role) && !string.IsNullOrWhiteSpace(commid));
        }
    }
}
