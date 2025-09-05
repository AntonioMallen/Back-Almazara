namespace Back_Almazara.ViewModel
{
    public class LoginViewModel
    {

        public string token { get; set; } = "";
        public UserData userData { get; set; }


        public class UserData() { 
            public string userID { get; set; }
            public string name { get; set; }
            public string role { get; set; }
        }

    }
}
