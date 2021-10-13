# <center>MASA.EShop</center>

# ����

## Ŀ¼�ṹ

```
MASA.EShop
������ dapr
��   ������ components                           dapr�����������Ŀ¼
��   ��   ������ pubsub.yaml                      �������������ļ�
��   ��   ������ statestore.yaml                  ״̬���������ļ�
������ src                                      Դ�ļ�Ŀ¼
��   ������ Api
��   ��   ������ MASA.EShop.Api.Open              BFF�㣬�ṩ�ӿڸ�Web.Client
��   ������ Contracts                            ����Ԫ����ȡ��������ͨ�ŵ�Event Class
��   ��   ������ MASA.EShop.Contracts.Basket
��   ��   ������ MASA.EShop.Contracts.Catalog
��   ��   ������ MASA.EShop.Contracts.Ordering
��   ��   ������ MASA.EShop.Contracts.Payment
��   ������ Services                             ������
��   ��   ������ MASA.EShop.Services.Basket
��   ��   ������ MASA.EShop.Services.Catalog
��   ��   ������ MASA.EShop.Services.Ordering
��   ��   ������ MASA.EShop.Services.Payment
��   ������ Web
��   ��   ������ MASA.EShop.Web.Admin
��   ��   ������ MASA.EShop.Web.Client
������ test
|   ������ MASA.EShop.Services.Catalog.Tests
������ docker-compose                          docker-compose ��������
��   ������ MASA.EShop.Web.Admin
��   ������ MASA.EShop.Web.Client
������ .gitignore                               git�ύ�ĺ����ļ�
������ LICENSE                                  ��Ŀ���
������ .dockerignore                            docker�����ĺ����ļ�
������ README.md                                ��Ŀ˵���ļ�
```

## ��Ŀ�ṹ

![�ṹͼ](img/eshop.png)

## ��Ŀ�ܹ��������£�

![�ܹ�ͼ](img/eshop-architectureks.png)

## ��������

- ׼������

  - Docker
  - VS 2022
  - .Net 6.0
  - Dapr

- ������Ŀ

  - VS 2022(�Ƽ�)

    ���� docker-compose Ϊ������Ŀ,Ctrl + F5 ������

    ![vs-run](img/vs_run.png)

    ��������Կ���������ͼ�Ķ�Ӧ���

    ![vs-result](img/vs_result.png)

  - CLI

    ��Ŀ��Ŀ¼��ִ������

    ```
    docker-compose build
    docker-compose up
    ```

    ������Ч��

    ![cli-result](img/cli_result.png)

  - VS Code (Todo)

- ����Ч��

  Baseket Service: http://localhost:8081/swagger/index.html  
  Catalog Service: http://localhost:8082/swagger/index.html  
  Ordering Service: http://localhost:8083/swagger/index.html  
  Payment Service: http://localhost:8084/swagger/index.html

## ����

#### MinimalAPI

��Ŀ�еķ���ʹ�� .Net 6.0 ������ Minimal API ��ʽ����ԭ�е� Web API ʵ��

> ���� Minimal API ���ݲο�[mvc-to-minimal-apis-aspnet-6](https://benfoster.io/blog/mvc-to-minimal-apis-aspnet-6/)

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/v1/helloworld", ()=>"Hello World");
app.Run();
```

`MASA.Contrib.Service.MinimalAPIs`�� Minimal API ��һ����װ,�޸Ĵ���Ϊ:

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Services.AddServices(builder);
app.Run();
```

```C#
public class HelloService : ServiceBase
{
    public HelloService(IServiceCollection services): base(services) =>
        App.MapGet("/api/v1/helloworld", ()=>"Hello World"));
}
```

> ������ ServiceBase �ࣨ�൱�� ControllerBase����ʹ��ʱ�����Լ��� Service �ࣨ�൱�� Controller�����ڹ��캯����ά��·��ע�ᡣ`AddServices(builder)`�������ҵ����з��������ע�ᡣ�̳� ServiceBase ��Ϊ����ģʽ,���캯��ע��ֻ����ע�뵥������ ILogger,�ִ��� Repostory ��Ӧ�ý��� FromService ʵ�ַ���ע�롣

#### Dapr

�ٷ� Dapr ʹ�ý��ܣ�MASA.Contrib ��װ Dapr ʵ��ʹ�òο� Event ����

���� Dapr ���ݲο�:https://docs.microsoft.com/zh-cn/dotnet/architecture/dapr-for-net-developers/

1. ��� Dapr

```C#
builder.Services.AddDaprClient();
...
app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
```

2. �����¼�

```C#
 [Topic("pubsub", nameof(OrderStatusChangedToValidatedIntegrationEvent)]
 public async Task OrderStatusChangedToValidatedAsync(
     OrderStatusChangedToValidatedIntegrationEvent integrationEvent,
     [FromServices] ILogger<IntegrationEventService> logger)
 {
     logger.LogInformation("----- integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", integrationEvent.Id, Program.AppName, integrationEvent);
 }
```

> Topic ��һ������ pubsub Ϊ�����ļ� pubsub.yaml ��ָ���� name

3. �����¼�

```C#
var @event = new OrderStatusChangedToValidatedIntegrationEvent();
await _daprClient.PublishEventAsync
(
    "pubsub",
    nameof(OrderStatusChangedToValidatedIntegrationEvent),
    @event
);
```

#### Actor

1. ��Ŀ������ Actor ֧��

```C#
app.UseEndpoints(endpoint =>
{
    ...
    endpoint.MapActorsHandlers(); //Actor ֧��
});
```

2. ���� Actor �ӿڣ��̳� IActor��

```C#
public interface IOrderingProcessActor : IActor
{
```

3. ʵ��`IOrderingProcessActor`�����̳�`Actor`�ࡣʾ����Ŀ��ʵ����`IRemindable`�ӿڣ�ʵ�ָýӿں�ͨ������`RegisterReminderAsync`���ע�����ѡ�

```C#
public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
{
    //todo
}
```

4. ע�� Actor

```C#
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
```

5. Actor ���ô���

```C#
var actorId = new ActorId(order.Id.ToString());
var actor = ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
```

#### EventBus

��֧�ַ��ͽ������¼�

1. ��� EventBus

```C#
builder.Services.AddEventBus();
```

2. �Զ��� Event

```C#
public class DemoEvent : Event
{
    //todo �Զ��������¼�����
}
```

3. ���� Event

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new DomeEvent());
```

4. �����¼�

```C#
[EventHandler]
public async Task DemoHandleAsync(DomeEvent @event)
{
    //todo
}
```

#### IntegrationEventBus

���Ϳ�����¼�������ͬʱ��� EventBus ʱ��Ҳ֧�ֽ������¼�

1. ��� IntegrationEventBus

```C#
builder.Services
    .AddDaprEventBus<IntegrationEventLogService>();
//   .AddDaprEventBus<IntegrationEventLogService>(options=>{
//    	//todo
//   	options.UseEventBus();//���EventBus
//	});
```

2. �Զ��� Event

```C#
public class DomeIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(DomeIntegrationEvent);
    //todo �Զ��������¼�����
}
```

> Topic ����ֵΪ Dapr pub/sub ������� TopicAttribute �ڶ���������ֵ

3. ���� Event

```C#
public class DemoService
{
    private readonly IIntegrationEventBus _eventBus;

    public DemoService(IIntegrationEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    //todo

    public async Task DemoPublish()
    {
        //todo
        await _eventBus.PublishAsync(new DomeIntegrationEvent());
    }
}
```

4. �����¼�

```C#
[Topic("pubsub", nameof(DomeIntegrationEvent))]
public async Task DomeIntegrationEventHandleAsync(DomeIntegrationEvent @event)
{
    //todo
}
```

#### CQRS

������� CQRS �ĵ���ο���https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs

##### Query

1. ���� Query

```c#
public class CatalogItemQuery : Query<List<CatalogItem>>
{
	public string Name { get; set; } = default!;

	public override List<CatalogItem> Result { get; set; } = default!;
}
```

2. ��� QueryHandler, ����

```c#
public class CatalogQueryHandler
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogQueryHandler(ICatalogItemRepository catalogItemRepository) => _catalogItemRepository = catalogItemRepository;

    [EventHandler]
    public async Task ItemsWithNameAsync(CatalogItemQuery query)
    {
        query.Result = await _catalogItemRepository.GetListAsync(query.Name);
    }
}
```

3. ���� Query

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new CatalogItemQuery(){
	Name = "Rolex"
});//������ʹ��ʹ��IEventBus
```

##### Command

1. ���� Command

```c#
public class CreateCatalogItemCommand : Command
{
	public string Name { get; set; } = default!;

    //todo
}
```

2. ��� CommandHandler, ����

```c#
public class CatalogCommandHandler
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogCommandHandler(ICatalogItemRepository catalogItemRepository) => _catalogItemRepository = catalogItemRepository;

    [EventHandler]
    public async Task CreateCatalogItemAsync(CreateCatalogItemCommand command)
    {
        //todo
    }
}
```

3. ���� Command

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new CreateCatalogItemCommand());//������ʹ��ʹ��IEventBus
```

#### DDD

DDD �������ݲο�:https://xie.infoq.cn/article/097316aecce39cdc5709e7d73

�ȿ��Կɷ��ͽ������¼���Ҳ�ɷ��Ϳ�����¼�

1. ��� DomainEventBus

```c#
.AddDomainEventBus(options =>
{
    options.UseEventBus()//ʹ�ý������¼�
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))//ʹ�ù�����Ԫ
        .UseDaprEventBus<IntegrationEventLogService>()///ʹ�ÿ�����¼�
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//ʹ��Repository��EF��ʵ��
})
```

2. ���� DomainCommand( ������ )

```C#
//У��֧����Command, ��Ҫ�̳�DomainCommand, ����ǲ�ѯ, ����Ҫ�̳�DomainQuery<>
public class OrderStatusChangedToValidatedCommand : DomainCommand
{
    public Guid OrderId { get; set; }
}
```

3. ���� DomainCommand

```C#
IDomainEventBus domainEventBus;
await domainEventBus.PublishAsync(new OrderStatusChangedToValidatedCommand()
{
    OrderId = "OrderId"
});//����DomainCommand
```

4. ��� Handler

```C#
[EventHandler]
public async Task ValidatedHandleAsync(OrderStatusChangedToValidatedCommand command)
{
    //todo
}
```

5. ���� DomainEvent������̣�

```c#
public class OrderPaymentSucceededDomainEvent : IntegrationDomainEvent
{
     public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentSucceededIntegrationEvent);

    private OrderPaymentSucceededDomainEvent()
    {
    }

    public OrderPaymentSucceededDomainEvent(Guid orderId) => OrderId = orderId;
}

public class OrderPaymentFailedDomainEvent : IntegrationDomainEvent
{
    public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentFailedIntegrationEvent);

    private OrderPaymentFailedDomainEvent()
    {
    }

    public OrderPaymentFailedDomainEvent(Guid orderId) => OrderId = orderId;
}
```

6. ����������񲢷��� IntegrationDomainEvent������̣�

```c#
public class PaymentDomainService : DomainService
{
	private readonly ILogger<PaymentDomainService> _logger;

	public PaymentDomainService(IDomainEventBus eventBus, ILogger<PaymentDomainService> logger) : base(eventBus)
        => _logger = logger;

    public async Task StatusChangedAsync(Aggregate.Payment payment)
    {
        IIntegrationDomainEvent orderPaymentDomainEvent;
        if (payment.Succeeded)
        {
            orderPaymentDomainEvent = new OrderPaymentSucceededDomainEvent(payment.OrderId);
        }
        else
        {
            orderPaymentDomainEvent = new OrderPaymentFailedDomainEvent(payment.OrderId);
        }
        _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentDomainEvent.Id, Program.AppName, orderPaymentDomainEvent);
        await EventBus.PublishAsync(orderPaymentDomainEvent);//���ڷ���DomainEvent
    }
}
```

## ����˵��

#### MASA.EShop.Services.Basket

1. ���[MinimalAPI](####MinimalAPI)
2. ��ӡ�ʹ��[Dapr](####Dapr)

#### MASA.EShop.Services.Catalog

1. ���[MinimalAPI](####MinimalAPI)
2. ���[DaprEventBus](####IntegrationEventBus)

```c#
builder.Services
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()//ʹ�ý������¼�
           .UseUow<CatalogDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=catalog"))//ʹ�ù�����Ԫ
           .UseEventLog<CatalogDbContext>();//��CatalogDbContext�����Ľ����¼���־ʹ��, CatalogDbContext��Ҫ�̳�IntegrationEventLogContext
})
```

3. ʹ��[CQRS](####CQRS)

#### MASA.EShop.Services.Ordering

1. ���[MinimalAPI](####MinimalAPI)
2. ���[DaprEventBus](####IntegrationEventBus)

```C#
builder.Services
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()
           .UseUoW<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
           .UseEventLog<OrderingContext>();
});
```

3. ʹ��[CQRS](####CQRS)
4. ���[Actor](####Actor)

�޸� docker-compse �ļ�

docker-compose.yml ������ dapr ����;

```yaml
dapr-placement:
  image: 'daprio/dapr:1.4.0'
```

docker-compose.override.yml �����Ӿ�������Ͷ˿�ӳ��

```yaml
dapr-placement:
  command: ['./placement', '-port', '50000', '-log-level', 'debug']
  ports:
    - '50000:50000'
```

��Ӧ�� ordering.dapr ��������������

```yaml
"-placement-host-address", "dapr-placement:50000"
```

#### MASA.EShop.Services.Payment

1. ���[MinimalAPI](####MinimalAPI)
2. ���[DomainEventBus](####DDD)

```C#
builder.Services
.AddDomainEventBus(options =>
{
    options.UseEventBus()//ʹ�ý������¼�
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
        .UseDaprEventBus<IntegrationEventLogService>()///ʹ�ÿ�����¼�
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//ʹ��Repository��EF��ʵ��
})
```

3. ʹ��[CQRS](####CQRS)

4. ʹ��[DDD](####DDD)

# ���ܽ���

������

# Nuget ������

```c#
Install-Package MASA.Contrib.Service.MinimalAPIs //MinimalAPIʹ��
```

```c#
Install-Package MASA.Contrib.Dispatcher.Events //���ͽ�������Ϣ
```

```c#
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.Dapr //���Ϳ������Ϣʹ��
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF //��¼�������Ϣ��־
```

```c#
Install-Package MASA.Contrib.Data.Uow.EF //������Ԫ��ȷ�������һ����
```

```c#
Install-Package MASA.Contrib.ReadWriteSpliting.CQRS //CQRSʵ��
```

```c#
Install-Package MASA.BuildingBlocks.DDD.Domain //DDD���ʵ��
Install-Package MASA.Contribs.DDD.Domain.Repository.EF //Repositoryʵ��
```

# ���֤ / License

MASA.EShop ���� [MIT License](http://gitlab-hz.lonsid.cn/MASA-Stack/Framework/MASA.EShop/-/blob/develop/LICENSE.txt) ��Դ���֤��
