using Microsoft.AspNetCore.Mvc;
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
            return Ok();
        }
    }
}
