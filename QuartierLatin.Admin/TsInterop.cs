using System;
using System.Collections.Generic;
using System.Linq;
using CoreRPC;
using CoreRPC.AspNetCore;
using QuartierLatin.Admin.Utils;
using CoreRPC.Typescript;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json.Serialization;

namespace QuartierLatin.Admin
{
    public static class TsInterop
    {
        public static string GenerateTsRpc()
        {
            return GenerateInternalTsRpc();
        }

        private static void Configure(TypescriptGenerationOptions config)
        {
            var orig = config.ApiFieldNamingPolicy;
            config.DtoClassNamingPolicy = t => t == typeof(Result<>) ? "ResultT" : t.Name;
            config.ApiFieldNamingPolicy = type => orig(type).Replace("Rpc", "");
            config.DtoFieldNamingPolicy = TypescriptGenerationOptions.ToCamelCase;
            config.CustomTypeMapping = _ => null;
            config.CustomTsTypeMapping = (type, _) =>
            {
                if (type == typeof(byte[])) return "string";
                if (type == typeof(object)) return "any";
                if (type == typeof(decimal)) return "number";
                if (type == typeof(Guid)) return "string";
                if (type == typeof(DateTimeOffset)) return "string";
                if (type == typeof(DateTime)) return "string";
                return null;
            };
        }

        private static string GenerateInternalTsRpc()
        {
            return AspNetCoreRpcTypescriptGenerator.GenerateCode(GetRpcTypes(), Configure);
        }


        private static IEnumerable<Type> GetRpcTypes()
        {
            var types = typeof(Program).Assembly.GetTypes()
                .Where(type => typeof(IHttpContextAwareRpc).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();
            return types;
        }

        public static void Register(IApplicationBuilder app, List<IMethodCallInterceptor> interceptors = null)
        {
            app.UseCoreRpc("/tsrpc", config =>
            {
                config.RpcTypeResolver = GetRpcTypes;
                config.Interceptors.AddRange(interceptors ?? new List<IMethodCallInterceptor>());
                config.JsonSerializer.ContractResolver = new FixedJsonContractResolver();
            });
        }

        public class FixedJsonContractResolver : CamelCasePropertyNamesContractResolver
        {
            public FixedJsonContractResolver()
            {
                ((CamelCaseNamingStrategy) NamingStrategy).ProcessDictionaryKeys = false;
            }
        }
    }
}