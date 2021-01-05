using Faces.Shared.Messaging.InterfacesConstants;
using Faces.WebMvc.Models;
using Faces.WebMvc.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Faces.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public HomeController(ILogger<HomeController> logger, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            MemoryStream memory = new MemoryStream();
            using (var uplodadFile = model.ImageFile.OpenReadStream())
            {
                await uplodadFile.CopyToAsync(memory);
            }
            model.OrderId = Guid.NewGuid();
            model.ImageData = memory.ToArray();
            model.PictureUri = model.ImageFile.FileName;
            var sendToUri = new Uri(RabbitmqMassTransitConstants.RabbitmqUri + RabbitmqMassTransitConstants.RegisterOrderCommandQueue);
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(sendToUri);
            await endpoint.Send<IRegisterOrderCommand>(new
            {
                model.OrderId,
                model.UserEmail,
                model.ImageData,
                model.PictureUri
            });
            ViewData["OrderId"] = model.OrderId;
            return View("Thanks");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
