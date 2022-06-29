using DogFactsWeb.Models;

namespace DogFactsWeb.Services.Contracts
{
    public interface DogFactsContract
    {
        Task<GenericResponseVM> GetDogFacts(DogFactsVM model);
    }
}
