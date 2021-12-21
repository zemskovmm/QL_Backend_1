using System;
using System.Collections.Generic;
using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto.CommonTraitDto;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("block")]
        public JObject PageBlockDto { get; set; }

        [JsonProperty("previewImageId")]
        public int? PreviewImageId { get; set; }

        [JsonProperty("smallPreviewImageId")]
        public int? SmallPreviewImageId { get; set; }

        [JsonProperty("widePreviewImageId")]
        public int? WidePreviewImageId { get; set; }

        public DateTime? Date { get; set; }
      
    	public DateTime? BlockDate { get; set; }

        public PageType PageType { get; set; }

        public Dictionary<string, List<CommonTraitLanguageDto>>? NamedTraits { get; set; }

        public string? Url { get; set; }
        public JObject? Metadata { get; set; }

        public PageDto(string title, JObject pageBlockDto, 
            DateTime? date, PageType pageType, int? previewImageId, 
            int? smallPreviewImageId, int? widePreviewImageId, 
            Dictionary<string, List<CommonTraitLanguageDto>>? namedTraits, string? url,
            JObject? metadata, DateTime? blockDate = null)
        {
            Title = title;
            PageBlockDto = pageBlockDto;
            Date = date;
            BlockDate = blockDate;
            PageType = pageType;
            PreviewImageId = previewImageId;
            SmallPreviewImageId = smallPreviewImageId;
            WidePreviewImageId = widePreviewImageId;
            NamedTraits = namedTraits;
            Url = url;
            Metadata = metadata;
        }
    }
}