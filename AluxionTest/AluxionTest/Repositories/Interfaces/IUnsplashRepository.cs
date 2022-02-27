using AluxionTest.Models.Internal;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AluxionTest.Repositories.Interfaces
{
    public interface IUnsplashRepository
    {
        Task<List<UnsplashImageItem>> ListImagesByQuery(string query);
        Task<UnsplashImageItem> SelectImageById(string imageId);
        Task<StreamData> DownloadImage(UnsplashImageItem image);
    }
}
