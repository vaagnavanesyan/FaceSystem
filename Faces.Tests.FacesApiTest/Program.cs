using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Faces.Tests.FacesApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"oscars-2017.jpg";
            var orderId = Guid.NewGuid();
            var urlAddress = @$"http://localhost:5000/api/faces/{orderId}";
            var utils = new ImageUtility();
            var bytes = utils.ConvertToBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath));
            List<byte[]> faces = null;
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(urlAddress, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<Guid, List<byte[]>>>(apiResponse);
                    faces = result[orderId];
                }
            }
            for (int i = 0; i < faces.Count; i++)
            {
                utils.SaveToFile(faces[i], Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"face{i}.jpg"));
            }
        }
    }
}
