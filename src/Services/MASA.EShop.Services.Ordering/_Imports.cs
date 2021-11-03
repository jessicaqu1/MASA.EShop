﻿global using Dapr;
global using Dapr.Actors;
global using Dapr.Actors.Client;
global using Dapr.Actors.Runtime;
global using MASA.BuildingBlocks.Dispatcher.Events;
global using MASA.BuildingBlocks.Dispatcher.IntegrationEvents;
global using MASA.Contrib.Data.UoW.EF;
global using MASA.Contrib.Dispatcher.Events;
global using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
global using MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
global using MASA.Contrib.ReadWriteSpliting.CQRS.Commands;
global using MASA.Contrib.ReadWriteSpliting.CQRS.Queries;
global using MASA.Contrib.Service.MinimalAPIs;
global using MASA.EShop.Contracts.Basket;
global using MASA.EShop.Contracts.Basket.Model.BFF;
global using MASA.EShop.Contracts.Catalog.Event;
global using MASA.EShop.Contracts.Ordering.Event;
global using MASA.EShop.Contracts.Payment;
global using MASA.EShop.Services.Ordering.Actors;
global using MASA.EShop.Services.Ordering.Application.CardTypes.Queries;
global using MASA.EShop.Services.Ordering.Application.Orders.Commands;
global using MASA.EShop.Services.Ordering.Application.Orders.Queries;
global using MASA.EShop.Services.Ordering.Domain.Events;
global using MASA.EShop.Services.Ordering.Domain.Repositories;
global using MASA.EShop.Services.Ordering.Dto;
global using MASA.EShop.Services.Ordering.Entities;
global using MASA.EShop.Services.Ordering.Infrastructure;
global using MASA.EShop.Services.Ordering.Infrastructure.EntityConfigurations;
global using MASA.EShop.Services.Ordering.Infrastructure.Extensions;
global using MASA.EShop.Services.Ordering.Infrastructure.Repositories;
global using MASA.Utils.Data.EntityFrameworkCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.OpenApi.Models;
global using System;
global using System.Text;
global using System.Text.Json;
