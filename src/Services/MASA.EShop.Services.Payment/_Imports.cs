﻿global using Dapr;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using MASA.BuildingBlocks.Data.UoW;
global using MASA.BuildingBlocks.DDD.Domain.Entities.Auditing;
global using MASA.BuildingBlocks.DDD.Domain.Events;
global using MASA.BuildingBlocks.DDD.Domain.Repositories;
global using MASA.BuildingBlocks.Dispatcher.Events;
global using MASA.Contrib.Data.UoW.EF;
global using MASA.Contrib.DDD.Domain;
global using MASA.Contrib.DDD.Domain.Events;
global using MASA.Contrib.DDD.Domain.Repository.EF;
global using MASA.Contrib.Dispatcher.Events;
global using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
global using MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
global using MASA.Contrib.Service.MinimalAPIs;
global using MASA.EShop.Contracts.Ordering;
global using MASA.EShop.Contracts.Payment;
global using MASA.EShop.Services.Payment.Application.Middleware;
global using MASA.EShop.Services.Payment.Application.Payments.Commands;
global using MASA.EShop.Services.Payment.Domain.Repositories;
global using MASA.EShop.Services.Payment.Domain.Services;
global using MASA.EShop.Services.Payment.Infrastructure;
global using MASA.EShop.Services.Payment.Infrastructure.Extensions;
global using MASA.EShop.Services.Payment.Service;
global using MASA.Utils.Data.EntityFrameworkCore;
global using MASA.Utils.Models.Config;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
