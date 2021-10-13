# <center>MASA.EShop</center>

# 介绍

## 目录结构

```
MASA.EShop
├── dapr
�?  ├── components                           dapr本地组件定义目录
�?  �?  ├── pubsub.yaml                      发布订阅配置文件
�?  �?  └── statestore.yaml                  状态管理配置文�?
├── src                                      源文件目�?
�?  ├── Api
�?  �?  └── MASA.EShop.Api.Open              BFF层，提供接口给Web.Client
�?  ├── Contracts                            公用元素提取，如服务间通信的Event Class
�?  �?  ├── MASA.EShop.Contracts.Basket
�?  �?  ├── MASA.EShop.Contracts.Catalog
�?  �?  ├── MASA.EShop.Contracts.Ordering
�?  �?  └── MASA.EShop.Contracts.Payment
�?  ├── Services                             服务拆分
�?  �?  ├── MASA.EShop.Services.Basket
�?  �?  ├── MASA.EShop.Services.Catalog
�?  �?  ├── MASA.EShop.Services.Ordering
�?  �?  └── MASA.EShop.Services.Payment
�?  ├── Web
�?  �?  ├── MASA.EShop.Web.Admin
�?  �?  └── MASA.EShop.Web.Client
├── test
|   └── MASA.EShop.Services.Catalog.Tests
├── docker-compose                          docker-compose 服务配置
�?  ├── MASA.EShop.Web.Admin
�?  └── MASA.EShop.Web.Client
├── .gitignore                               git提交的忽略文�?
├── LICENSE                                  项目许可
├── .dockerignore                            docker构建的忽略文�?
└── README.md                                项目说明文件
```

## 项目结构

![结构图](img/eshop.png)

## 项目架构（待更新�?

![架构图](img/eshop-architectureks.png)

## 快速入�?

- 准备工作

  - Docker
  - VS 2022
  - .Net 6.0
  - Dapr

- 启动项目

  - VS 2022(推荐)

    设置 docker-compose 为启动项�?Ctrl + F5 启动�?

    ![vs-run](img/vs_run.png)

    启动后可以看到容器视图的对应输出

    ![vs-result](img/vs_result.png)

  - CLI

    项目根目录下执行命令

    ```
    docker-compose build
    docker-compose up
    ```

    启动后效�?

    ![cli-result](img/cli_result.png)

  - VS Code (Todo)

- 启动效果

  Baseket Service: http://localhost:8081/swagger/index.html  
  Catalog Service: http://localhost:8082/swagger/index.html  
  Ordering Service: http://localhost:8083/swagger/index.html  
  Payment Service: http://localhost:8084/swagger/index.html

## 特�?

#### MinimalAPI

项目中的服务使用 .Net 6.0 新增�?Minimal API 方式代替原有�?Web API 实现

> 更多 Minimal API 内容参考[mvc-to-minimal-apis-aspnet-6](https://benfoster.io/blog/mvc-to-minimal-apis-aspnet-6/)

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/v1/helloworld", ()=>"Hello World");
app.Run();
```

`MASA.Contrib.Service.MinimalAPIs`�?Minimal API 进一步封�?修改代码�?

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

> 增加�?ServiceBase 类（相当�?ControllerBase），使用时定义自己的 Service 类（相当�?Controller），在构造函数中维护路由注册。`AddServices(builder)`方法会找到所有服务类完成注册。继�?ServiceBase 类为单例模式,构造函数注入只可以注入单例，如 ILogger,仓储�?Repostory 等应该借助 FromService 实现方法注入�?

#### Dapr

官方 Dapr 使用介绍，MASA.Contrib 封装 Dapr 实现使用参�?Event 部分

更多 Dapr 内容参�?https://docs.microsoft.com/zh-cn/dotnet/architecture/dapr-for-net-developers/

1. 添加 Dapr

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

2. 订阅事件

```C#
 [Topic("pubsub", nameof(OrderStatusChangedToValidatedIntegrationEvent)]
 public async Task OrderStatusChangedToValidatedAsync(
     OrderStatusChangedToValidatedIntegrationEvent integrationEvent,
     [FromServices] ILogger<IntegrationEventService> logger)
 {
     logger.LogInformation("----- integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", integrationEvent.Id, Program.AppName, integrationEvent);
 }
```

> Topic 第一个参�?pubsub 为配置文�?pubsub.yaml 中指定的 name

3. 发布事件

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

1. 项目中增�?Actor 支持

```C#
app.UseEndpoints(endpoint =>
{
    ...
    endpoint.MapActorsHandlers(); //Actor 支持
});
```

2. 定义 Actor 接口，继�?IActor�?

```C#
public interface IOrderingProcessActor : IActor
{
```

3. 实现`IOrderingProcessActor`，并继承`Actor`类。示例项目还实现了`IRemindable`接口，实现该接口后通过方法`RegisterReminderAsync`完成注册提醒�?

```C#
public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
{
    //todo
}
```

4. 注册 Actor

```C#
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
```

5. Actor 调用代码

```C#
var actorId = new ActorId(order.Id.ToString());
var actor = ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
```

#### EventBus

仅支持发送进程内事件

1. 添加 EventBus

```C#
builder.Services.AddEventBus();
```

2. 自定�?Event

```C#
public class DemoEvent : Event
{
    //todo 自定义属性事件参�?
}
```

3. 发�?Event

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new DomeEvent());
```

4. 处理事件

```C#
[EventHandler]
public async Task DemoHandleAsync(DomeEvent @event)
{
    //todo
}
```

#### IntegrationEventBus

发送跨进程事件，但当同时添�?EventBus 时，也支持进程内事件

1. 添加 IntegrationEventBus

```C#
builder.Services
    .AddDaprEventBus<IntegrationEventLogService>();
//   .AddDaprEventBus<IntegrationEventLogService>(options=>{
//    	//todo
//   	options.UseEventBus();//添加EventBus
//	});
```

2. 自定�?Event

```C#
public class DomeIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(DomeIntegrationEvent);
    //todo 自定义属性事件参�?
}
```

> Topic 属性值为 Dapr pub/sub 相关特�?TopicAttribute 第二个参数的�?

3. 发�?Event

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

4. 处理事件

```C#
[Topic("pubsub", nameof(DomeIntegrationEvent))]
public async Task DomeIntegrationEventHandleAsync(DomeIntegrationEvent @event)
{
    //todo
}
```

#### CQRS

更多关于 CQRS 文档请参考：https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs

##### Query

1. 定义 Query

```c#
public class CatalogItemQuery : Query<List<CatalogItem>>
{
	public string Name { get; set; } = default!;

	public override List<CatalogItem> Result { get; set; } = default!;
}
```

2. 添加 QueryHandler, 例：

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

3. 发�?Query

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new CatalogItemQuery(){
	Name = "Rolex"
});//进程内使用使用IEventBus
```

##### Command

1. 定义 Command

```c#
public class CreateCatalogItemCommand : Command
{
	public string Name { get; set; } = default!;

    //todo
}
```

2. 添加 CommandHandler, 例：

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

3. 发�?Command

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new CreateCatalogItemCommand());//进程内使用使用IEventBus
```

#### DDD

DDD 更多内容参�?https://xie.infoq.cn/article/097316aecce39cdc5709e7d73

既可以可发送进程内事件、也可发送跨进程事件

1. 添加 DomainEventBus

```c#
.AddDomainEventBus(options =>
{
    options.UseEventBus()//使用进程内事�?
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))//使用工作单元
        .UseDaprEventBus<IntegrationEventLogService>()///使用跨进程事�?
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//使用Repository的EF版实�?
})
```

2. 定义 DomainCommand( 进程�?)

```C#
//校验支付的Command, 需要继承DomainCommand, 如果是查�? 则需要继承DomainQuery<>
public class OrderStatusChangedToValidatedCommand : DomainCommand
{
    public Guid OrderId { get; set; }
}
```

3. 发�?DomainCommand

```C#
IDomainEventBus domainEventBus;
await domainEventBus.PublishAsync(new OrderStatusChangedToValidatedCommand()
{
    OrderId = "OrderId"
});//发送DomainCommand
```

4. 添加 Handler

```C#
[EventHandler]
public async Task ValidatedHandleAsync(OrderStatusChangedToValidatedCommand command)
{
    //todo
}
```

5. 定义 DomainEvent（跨进程�?

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

6. 定义领域服务并发�?IntegrationDomainEvent（跨进程�?

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
        await EventBus.PublishAsync(orderPaymentDomainEvent);//用于发送DomainEvent
    }
}
```

## 服务说明

#### MASA.EShop.Services.Basket

1. 添加[MinimalAPI](####MinimalAPI)
2. 添加、使用[Dapr](####Dapr)

#### MASA.EShop.Services.Catalog

1. 添加[MinimalAPI](####MinimalAPI)
2. 添加[DaprEventBus](####IntegrationEventBus)

```c#
builder.Services
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()//使用进程内事�?
           .UseUow<CatalogDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=catalog"))//使用工作单元
           .UseEventLog<CatalogDbContext>();//将CatalogDbContext上下文交于事件日志使�? CatalogDbContext需要继承IntegrationEventLogContext
})
```

3. 使用[CQRS](####CQRS)

#### MASA.EShop.Services.Ordering

1. 添加[MinimalAPI](####MinimalAPI)
2. 添加[DaprEventBus](####IntegrationEventBus)

```C#
builder.Services
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()
           .UseUoW<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
           .UseEventLog<OrderingContext>();
});
```

3. 使用[CQRS](####CQRS)
4. 添加[Actor](####Actor)

修改 docker-compse 文件

docker-compose.yml 中增�?dapr 服务;

```yaml
dapr-placement:
  image: 'daprio/dapr:1.4.0'
```

docker-compose.override.yml 中增加具体命令和端口映射

```yaml
dapr-placement:
  command: ['./placement', '-port', '50000', '-log-level', 'debug']
  ports:
    - '50000:50000'
```

对应�?ordering.dapr 服务上增加命�?

```yaml
"-placement-host-address", "dapr-placement:50000"
```

#### MASA.EShop.Services.Payment

1. 添加[MinimalAPI](####MinimalAPI)
2. 添加[DomainEventBus](####DDD)

```C#
builder.Services
.AddDomainEventBus(options =>
{
    options.UseEventBus()//使用进程内事�?
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
        .UseDaprEventBus<IntegrationEventLogService>()///使用跨进程事�?
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//使用Repository的EF版实�?
})
```

3. 使用[CQRS](####CQRS)

4. 使用[DDD](####DDD)

# 功能介绍

待补�?

# Nuget 包介�?

```c#
Install-Package MASA.Contrib.Service.MinimalAPIs //MinimalAPI使用
```

```c#
Install-Package MASA.Contrib.Dispatcher.Events //发送进程内消息
```

```c#
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.Dapr //发送跨进程消息使用
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF //记录跨进程消息日�?
```

```c#
Install-Package MASA.Contrib.Data.Uow.EF //工作单元，确保事务的一致�?
```

```c#
Install-Package MASA.Contrib.ReadWriteSpliting.CQRS //CQRS实现
```

```c#
Install-Package MASA.BuildingBlocks.DDD.Domain //DDD相关实现
Install-Package MASA.Contribs.DDD.Domain.Repository.EF //Repository实现
```

# 许可�?/ License

MASA.EShop 采用 [MIT License](http://gitlab-hz.lonsid.cn/MASA-Stack/Framework/MASA.EShop/-/blob/develop/LICENSE.txt) 开源许可证�?
