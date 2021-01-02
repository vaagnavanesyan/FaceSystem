using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FacesApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"oscars-2017.jpg";
            var urlAddress = @"http://localhost:6000/api/faces";
            var utils = new ImageUtility();
            var bytes = utils.ConvertToBytes(imagePath);
            List<byte[]> faces = null;
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(urlAddress, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faces = JsonSerializer.Deserialize<List<byte[]>>(apiResponse);
                }
            }
            for (int i = 0; i < faces.Count; i++)
            {
                utils.SaveToFile(faces[i], Path.Combine(Directory.GetCurrentDirectory(), $"face{i}.jpg"));
            }
        }
    }
}
