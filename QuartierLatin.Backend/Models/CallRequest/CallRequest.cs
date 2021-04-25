namespace QuartierLatin.Backend.Models.CallRequest
{
    public class CallRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }

        public CallRequest(string firstName, string lastName, string email, string phone, string url, string comment)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Url = url;
            Comment = comment;
        }
    }
}
