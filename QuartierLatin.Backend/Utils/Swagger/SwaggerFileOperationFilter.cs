using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuartierLatin.Backend.Utils.Swagger
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        private const string formDataMimeType = "multipart/form-data";
        private static readonly string[] formFilePropertyNames =
            typeof(IFormFile).GetTypeInfo().DeclaredProperties.Select(p => p.Name).ToArray();

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "apivaluesuploadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "formFile",
                    In = ParameterLocation.Header,
                    Description = "Upload File",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "file",
                        Format = "binary"
                    }
                });
                var uploadFileMediaType = new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "object",
                        Properties =
                        {
                            ["uploadedFile"] = new OpenApiSchema()
                            {
                                Description = "Upload File",
                                Type = "file",
                                Format = "binary"

                            }
                        },
                        Required = new HashSet<string>()
                        {
                            "uploadedFile"
                        }
                    }
                };
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                    {
                        ["multipart/form-data"] = uploadFileMediaType
                    }
                };
            }
        }
    }
}
