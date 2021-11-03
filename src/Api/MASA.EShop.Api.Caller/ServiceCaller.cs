namespace MASA.EShop.Api.Caller;

public abstract class ServiceCaller : IHttpClientCaller, IDaprClientCaller, ICaller
{
    private readonly IServiceProvider _serviceProvider;
    private DaprClient _daprClient { get; set; }
    private HttpClient _httpClient { get; set; }

    public virtual CallerModes Mode { get; set; } = CallerModes.OriginalHttp;
    public virtual string? BaseAddress { get; set; }
    public virtual string? AppId { get; init; }

    public ServiceCaller(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    void Check()
    {
        //todo change better design
        if (Mode == CallerModes.OriginalHttp)
        {
            if (_httpClient is null)
            {
                if (string.IsNullOrEmpty(BaseAddress))
                {
                    throw new($"Original caller mode {nameof(BaseAddress)} must be a value");
                }
                NewExpression newExpression;
                var handler = CreateHttpMessageHandler();
                if (handler == null)
                {
                    newExpression = Expression.New(typeof(HttpClient)); // 无参构造函数方式
                }
                else
                {
                    newExpression = Expression.New(typeof(HttpClient).GetConstructor(new[] { typeof(HttpMessageHandler) }), Expression.Constant(handler)); // 有参构造函数方式
                }
                var lambda = Expression.Lambda<Func<HttpClient>>(newExpression);
                _httpClient = lambda.Compile()();
                _httpClient.BaseAddress = new Uri(BaseAddress);
            }
        }
        else if (Mode == CallerModes.DaprHttp || Mode == CallerModes.DaprGrpc)
        {
            if (string.IsNullOrEmpty(AppId))
            {
                throw new($"Dapr caller mode {nameof(AppId)} must be a value");
            }
            if (_daprClient is null)
            {
                _daprClient = _serviceProvider.GetRequiredService<DaprClient>();
            }
        }
    }

    protected virtual HttpMessageHandler? CreateHttpMessageHandler()
    {
        return null;
    }

    Task<HttpResponseMessage> InvokeBase(HttpMethod httpMethod, string urlOrMethod, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        Check();
        switch (Mode)
        {
            case CallerModes.OriginalHttp:
                return _httpClient.SendAsync(
                    new HttpRequestMessage(httpMethod, urlOrMethod) { Content = httpContent },
                    cancellationToken);
            case CallerModes.OriginalGrpc:
                throw new NotImplementedException();
            case CallerModes.DaprHttp:
                var request = _daprClient.CreateInvokeMethodRequest(httpMethod, AppId, urlOrMethod);
                request.Content = httpContent;
                return _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
            case CallerModes.DaprGrpc:
                throw new NotImplementedException();
            default:
                break;
        }
        return Task.FromResult<HttpResponseMessage>(new());
    }

    async Task<string> InvokeProxy(HttpMethod httpMethod, string urlOrMethod, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        var taskResponse = InvokeBase(httpMethod, urlOrMethod, httpContent, cancellationToken);
        return await GetStringAsyncCore(taskResponse, cancellationToken);
    }

    async Task<TValue?> InvokeProxy<TValue>(HttpMethod httpMethod, string urlOrMethod, JsonSerializerOptions? options, HttpContent? httpContent, CancellationToken cancellationToken = default)
    {
        var taskResponse = InvokeBase(httpMethod, urlOrMethod, httpContent, cancellationToken);
        return await GetFromJsonAsyncCore<TValue>(taskResponse, options, cancellationToken);
    }

    private async Task<string> GetStringAsyncCore(Task<HttpResponseMessage> taskResponse, CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }

    private async Task<T?> GetFromJsonAsyncCore<T>(Task<HttpResponseMessage> taskResponse, JsonSerializerOptions? options, CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();
            // Nullable forgiving reason:
            // GetAsync will usually return Content as not-null.
            // If Content happens to be null, the extension will throw.
            return await response.Content.ReadFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<TValue?> GetFromJsonAsync<TValue>(string requestUri, CancellationToken cancellationToken = default)
    {
        return await GetFromJsonAsync<TValue>(requestUri, null, cancellationToken);
    }

    public async Task<TValue?> GetFromJsonAsync<TValue>(string requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
    {
        return await InvokeProxy<TValue>(HttpMethod.Get, requestUri, options, null, cancellationToken);
    }

    public async Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken)
    {
        return await InvokeProxy(HttpMethod.Get, requestUri, null, cancellationToken);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        JsonContent content = JsonContent.Create(value, mediaType: null, options);
        return await InvokeBase(HttpMethod.Post, requestUri, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent? content)
    {
        return await InvokeBase(HttpMethod.Put, requestUri, content);
    }

    public void Dispose()
    {
        if (_daprClient != null)
        {
            _daprClient.Dispose();
        }
    }
}

public enum CallerModes
{
    OriginalHttp,
    OriginalGrpc,
    DaprHttp,
    DaprGrpc
}

