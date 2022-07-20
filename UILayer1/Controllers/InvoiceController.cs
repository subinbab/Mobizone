using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UILayer.Data;
using UILayer.Data.ApiServices;

namespace UILayer.Controllers
{
    public class InvoiceController : Controller
    {
        IConfiguration _configuration;
        IUserApi _userApi;
        private readonly IMapper _mapper;
        public InvoiceController(IConfiguration configuration, IMapper mapper, IUserApi userApi)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userApi = userApi;
        }
        public IActionResult Index(int id)
        {
            var checkoutList = _userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.id.Equals(id)).FirstOrDefault();
            adminApi _adminApi = new adminApi(_configuration, _mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            ViewBag.OrderData = checkout;
            ViewBag.Contact = contactData;
            return View();
        }
        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<IActionResult> Download(string GridHtml,int id)
        {
            ConvertPdf _generatepdf = new ConvertPdf();
            string destFile = _generatepdf.GenerateReportPDF(GridHtml, "invoice", "ClientLetterData");
            var memory = new MemoryStream();
            using (var stream = new FileStream(destFile, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(destFile));
        }
    }
}
