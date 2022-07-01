using CoreServices.FileServices.Descriptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreServices.FileServices
{
    public class FileService : IFileService
    {
        public async Task<List<T>> ParseJsonToObjects<T>(string filepath) where T : class
        {
            try
            {
                var content = await ReadFile(filepath);
                return JsonConvert.DeserializeObject<List<T>>(content
                    , new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<string> ReadFile(string filepath) => await File.ReadAllTextAsync(filepath);
    }
}
