using MASA.EShop.Api.Caller;

namespace MASA.EShop.Web.Client.Services
{
    public class CallerService : HttpClientCaller
    {
        public CallerService(HttpClient httpClient) : base(httpClient)
        {
        }


    }
}
