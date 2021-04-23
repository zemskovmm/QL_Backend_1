using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces;

namespace QuartierLatin.Backend.Controllers
{
    public class AdminSpecialtyController : Controller
    {
        private readonly ISpecialtyAppService _specialtyAppService;
        public AdminSpecialtyController(ISpecialtyAppService specialtyAppService)
        {
            _specialtyAppService = specialtyAppService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
