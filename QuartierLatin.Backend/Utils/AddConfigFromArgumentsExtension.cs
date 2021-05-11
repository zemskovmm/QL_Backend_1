using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace QuartierLatin.Backend.Utils
{
    public static class AddConfigFromArgumentsExtension
    {
        public static IConfigurationBuilder AddConfigFromArguments(this IConfigurationBuilder builder, string[] args)
        {
            var configIndex = args.ToList().FindIndex(x => x.Equals("--Config"));
            var path = args.ElementAtOrDefault(configIndex + 1);

            if (configIndex is not -1)
            {
                if (!File.Exists(path)) 
                    throw new ApplicationException($"{path} doesn't exist");
                builder.AddJsonFile(path, true);
            }
            return builder;
        }
    }
}