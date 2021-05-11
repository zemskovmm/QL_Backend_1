using System.Collections.Generic;
using System.Net.Http;

namespace QuartierLatin.Backend.Utils
{
    public static class GetCallRequestContent
    {
        public static FormUrlEncodedContent GetContent(string title, string firstName, string lastName, string phone, string email,
            string comment) => new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["fields[TITLE]"] = title,
            ["fields[STATUS_ID]"] = "NEW",
            ["fields[OPENED]"] = "Y",
            ["fields[NAME]"] = firstName,
            ["fields[LAST_NAME]"] = lastName,
            ["fields[SOURCE_ID]"] = "WEB",
            ["fields[PHONE][0][VALUE_TYPE]"] = "WORK",
            ["fields[PHONE][0][TYPE_ID]"] = "PHONE",
            ["fields[PHONE][0][VALUE]"] = phone,
            ["fields[EMAIL][0][VALUE_TYPE]"] = "WORK",
            ["fields[EMAIL][0][TYPE_ID]"] = "EMAIL",
            ["fields[EMAIL][0][VALUE]"] = email,
            ["fields[COMMENTS]"] = comment,
            ["params[REGISTER_SONET_EVENT]"] = "Y"
        });
    }
}
