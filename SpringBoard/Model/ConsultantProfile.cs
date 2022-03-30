namespace SpringBoard.Model
{
    public class ConsultantProfile
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string password { get; set; }



        public string competence { get; set; }

        public bool valid()
        {
            return (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName)
                && !string.IsNullOrEmpty(email) );
        }
    }
}
