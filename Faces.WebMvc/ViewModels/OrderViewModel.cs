using Faces.Shared.Messaging.InterfacesConstants;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Faces.WebMvc.ViewModels
{
    public class OrderViewModel: IRegisterOrderCommand
    {
        [Display(Name = "Order Id")]
        public Guid OrderId { get; set; }

        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Image File")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Image URL")]
        public string PictureUri { get; set; }

        [Display(Name = "Order Status")]
        public string StatusString { get; set; }

        public byte[] ImageData { get; set; }
    }
}
