namespace SpringBoard.API.Model
{
    public class ProfileModel
    {
        public int id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string role { get; set; }


        public bool valid()
        {
            return (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(email) && id!=0 && !string.IsNullOrEmpty(role));
        }

    }
}
