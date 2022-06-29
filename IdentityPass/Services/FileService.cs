using IdentityPass.Services.Descriptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPass.Services
{
    public class FileService : IFileService
    {
        public async Task<List<T>> ParseJsonToObjects<T>(string filepath) where T : class
        {
            try
            {
                var content = await ReadFile(filepath);
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<string> ReadFile(string filepath) => await File.ReadAllTextAsync(filepath);
    }
}
