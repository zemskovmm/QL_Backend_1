using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CreateCommonTraitDto
    {
        public JObject Names { get; set; }
        public long? IconBlobId { get; set; }
        public int Order { get; set; }
    }
}
