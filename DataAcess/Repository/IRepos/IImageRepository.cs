using Models.Domain;

namespace DataAcess.Repos
{
    public interface IImageRepository
    {
        Task Upload(Image image);
    }
}