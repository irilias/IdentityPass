using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreServices.FileServices.Descriptions
{
    public interface IFileService
    {
        Task<List<T>> ParseJsonToObjects<T>(string filepath) where T : class;


    }
}
