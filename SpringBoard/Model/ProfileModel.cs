namespace SpringBoard.API.Model
{
    public class ProfileModel
    {
        public string id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string password { get; set; }



        public bool valid()
        {
            return (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(email) 
                && !string.IsNullOrWhiteSpace(id));
        }

    }
}
