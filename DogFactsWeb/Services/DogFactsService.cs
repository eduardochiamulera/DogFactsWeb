using DogFactsWeb.Models;
using DogFactsWeb.Services.Contracts;
using DogFactsWeb.Utils;
using System.Text.Json;

namespace DogFactsWeb.Services
{

    public class DogFactsService : DogFactsContract
    {
        private IList<string> _dogsIds = new List<string>();
        private IList<DogFactsResponse> _dogFacts = new List<DogFactsResponse>();
        private const string apiEndpoint = "/api/v1/facts/?number=";
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;

        public DogFactsService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<GenericResponseVM> GetDogFacts(DogFactsVM model)
        {
            try
            {
                _dogsIds = ExcelUtils.GetExcelData(model.File);

                HttpClient client;
                string api;
                ValidateAPIUrl(model, out client, out api);

                await GetAPIResponse(client, api);

                if (_dogFacts.Count == 0)
                    return new GenericResponseVM().AddError("Não foi possivel localizar os fatos, por favor revise os dados!");

                var response = ExcelUtils.GenerateExcel(_dogFacts);

                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponseVM().AddError(ex.Message);
            }

        }

        private async Task GetAPIResponse(HttpClient client, string api)
        {
            foreach (var item in _dogsIds)
            {
                using (var responseAPI = await client.GetAsync(api + item))
                {
                    if (responseAPI.IsSuccessStatusCode)
                    {
                        var apiResponseStream = await responseAPI.Content.ReadAsStreamAsync();
                        var responseModel = await JsonSerializer.DeserializeAsync<DogFactsResponse>(apiResponseStream, _options);
                        _dogFacts.Add(responseModel);
                    }
                }
            }
        }

        private void ValidateAPIUrl(DogFactsVM model, out HttpClient client, out string api)
        {
            client = _clientFactory.CreateClient("DogFactsApi");
            api = $"{apiEndpoint}";
            if (!string.IsNullOrWhiteSpace(model.Url))
            {
                client.BaseAddress = new Uri(model.Url);
                api = model.Url.Substring(model.Url.IndexOf("?"), model.Url.Length - model.Url.IndexOf("?")).Replace("#", "");
            }
        }
    }
}
