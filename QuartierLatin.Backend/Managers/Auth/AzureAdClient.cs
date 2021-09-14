using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using QuartierLatin.Backend.Config;

namespace QuartierLatin.Backend.Managers.Auth
{
    public interface IAzureAdClient
    {
        string GetLogoutUrl(string returnUrl);
        string CreateLoginUrl();
        string GetReloginUrl();
        Task<AuthenticationResult> GetTokenFromCode(string code);
        Task<AzureAdClient.AzureAdUserInfo> GetUserInfo(AuthenticationResult authResult);
    }

    public class NoopAzureAdClient : IAzureAdClient
    {
        public string GetLogoutUrl(string returnUrl)
        {
            throw new NotImplementedException();
        }

        public string CreateLoginUrl()
        {
            throw new NotImplementedException();
        }

        public string GetReloginUrl()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> GetTokenFromCode(string code)
        {
            throw new NotImplementedException();
        }

        public async Task<AzureAdClient.AzureAdUserInfo> GetUserInfo(AuthenticationResult authResult)
        {
            throw new NotImplementedException();
        }
    }

    public class AzureAdClient : IAzureAdClient
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUrl;
        private readonly string _tenant;

        public AzureAdClient(IOptions<AzureAuthConfiguration> config)
        {
            var (tenant, clientId, clientSecret, redirectUrl) = config.Value;
            _tenant = tenant;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUrl = redirectUrl;

            _app = ConfidentialClientApplicationBuilder.Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithTenantId(_tenant)
                .WithRedirectUri(_redirectUrl)
                .Build();
        }

        public string GetLogoutUrl(string returnUrl)
        {
            return $"https://login.microsoftonline.com/{_tenant}/oauth2/logout?post_logout_redirect_uri=" +
                   Uri.EscapeDataString(returnUrl);
        }

        public string CreateLoginUrl()
        {
            ////https://login.microsoftonline.com/b63664dd-3512-445f-8578-5f94b2ff06ad/oauth2/v2.0/authorize?client_id=
            //a298ce03-0baa-4cd4-a0f7-a7101366eb51&response_type=code&redirect_uri=http%3A%2F%2Flocalhost:123321%2Fmyapp%2F
            //&response_mode=query&scope=user.read&state=12345
            return QueryStringDictionary.AppendToUrl(
                $"https://login.microsoftonline.com/{_tenant}/oauth2/v2.0/authorize",
                new QueryStringDictionary
                {
                    ["client_id"] = _clientId,
                    ["response_type"] = "code",
                    ["redirect_uri"] = _redirectUrl,
                    ["response_mode"] = "query",
                    ["scope"] = "user.read",
                    ["state"] = "123"
                });
        }


        public string GetReloginUrl()
        {
            return GetLogoutUrl(CreateLoginUrl());
        }

        public async Task<AuthenticationResult> GetTokenFromCode(string code)
        {
            var authRes = await _app.AcquireTokenByAuthorizationCode(new[] {"user.read"},
                code).ExecuteAsync();
            return authRes;
        }

        public async Task<AzureAdUserInfo> GetUserInfo(AuthenticationResult authResult)
        {
            var graphClient = new GraphServiceClient(new TokenProvider(authResult.AccessToken));
            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(authResult.IdToken);
            var profile = await graphClient.Me.Request().GetAsync();

            var groups = decodedToken.Claims.Where(x => x.Type == "roles")
                .Select(x => x.Value)
                .ToList();

            return new AzureAdUserInfo
            {
                Id = profile.Id,
                Name = profile.DisplayName,
                Groups = groups,
                // Because Mail can be null
                Email = profile.Mail ?? profile.UserPrincipalName
            };
        }

        private class TokenProvider : IAuthenticationProvider
        {
            private readonly string _accessToken;

            public TokenProvider(string accessToken)
            {
                _accessToken = accessToken;
            }

            public async Task AuthenticateRequestAsync(HttpRequestMessage request)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", _accessToken
                );
            }
        }

        public class AzureAdUserInfo
        {
            public string Name { get; set; }
            public List<string> Groups { get; set; }
            public string Id { get; set; }
            public string Email { get; set; }
        }

        private class QueryStringDictionary : Dictionary<string, string>
        {
            public QueryStringDictionary()
            {
            }

            public QueryStringDictionary(Dictionary<string, string> dic) : base(dic)
            {
            }

            public string ToString(bool skipQuestionMark)
            {
                return GetString(this, skipQuestionMark);
            }

            public override string ToString()
            {
                return ToString(false);
            }

            public static string GetString(Dictionary<string, string> dict, bool skipQuestionMark)
            {
                if (dict.Count == 0)
                    return "";
                return (skipQuestionMark ? "" : "?") + string.Join("&", dict.Where(x => x.Value != null)
                    .Select(x => x.Key + "=" + Uri.EscapeDataString(x.Value)));
            }

            public static QueryStringDictionary Parse(string query)
            {
                var rv = new QueryStringDictionary();
                foreach (var pair in query.TrimStart('?').Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(new[] {'='}, 2)))
                    rv[pair[0]] = Uri.UnescapeDataString(pair[1]);
                return rv;
            }

            public static string AppendToUrl(string url, Dictionary<string, string> args)
            {
                var builder = new UriBuilder(url);
                var qs = Parse(builder.Query);
                foreach (var arg in args)
                    qs[arg.Key] = arg.Value;
                builder.Query = qs.ToString(true);
                return builder.Uri.ToString();
            }
        }
    }
}