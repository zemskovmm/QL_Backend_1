using System;

namespace QuartierLatin.Backend
{
    //Temporary hack to disable all kinds of authorization
    public class AuthorizeAttribute : Attribute
    {
        public string Roles { get; set; }

        public string Policy { get; set; }

        public string AuthenticationSchemes { get; set; }
    }
}