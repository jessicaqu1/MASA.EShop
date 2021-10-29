using MASA.EShop.Web.Client.Services;

namespace MASA.EShop.Web.Client.Pages;

public partial class Login : EShopPageBase
{
    private string _userName = "masa";
    private string _password = "eshop";

    [Inject]
    private IHttpClientFactory _clientFactory { get; set; }

    private async void LoginHandler()
    {
        var httpClient = _clientFactory.CreateClient(nameof(CallerService));
        if (_userName.Equals("masa") && _password.Equals("eshop"))
        {
            await ProtectedSessionStore.SetAsync("user", _userName);
            Navigation("/catalog");
        }
        else
        {
            Message("UserName Or Password Invalid");
        }
    }
}

