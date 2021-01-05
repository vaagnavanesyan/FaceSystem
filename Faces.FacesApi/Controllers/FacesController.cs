using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Faces.FacesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacesController : ControllerBase
    {
        [HttpPost]
        public async Task<List<byte[]>> ReadFaces()
        {
            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                var faces = GetFaces(ms.ToArray());
                return faces;
            }
        }

        private List<byte[]> GetFaces(byte[] image)
        {
            Mat source = Cv2.ImDecode(image, ImreadModes.Color);
            source.SaveImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg"), new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cascades", "haarcascade_frontalface_default.xml");
            CascadeClassifier faceCascade = new CascadeClassifier();
            faceCascade.Load(file);
            var faceRects = faceCascade.DetectMultiScale(source, 1.1, 6, HaarDetectionType.DoRoughSearch, new Size(60, 60));
            var faces = new List<byte[]>();
            int j = 0;
            foreach (var rect in faceRects)
            {
                var face = new Mat(source, rect);
                faces.Add(face.ToBytes(".jpg"));
                face.SaveImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "face" + j + ".jpg"), new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
                j++;
            }
            return faces;
        }
    }
}
