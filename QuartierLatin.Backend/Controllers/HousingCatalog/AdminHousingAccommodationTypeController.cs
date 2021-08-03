using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.HousingServices;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    public class AdminHousingAccommodationTypeController : Controller
    {
        private readonly IHousingAccommodationTypeAppService _housingAccommodationTypeAppService;
        private readonly JObject _definition;

        public AdminHousingAccommodationTypeController(IHousingAccommodationTypeAppService housingAccommodationTypeAppService)
        {
            _housingAccommodationTypeAppService = housingAccommodationTypeAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
