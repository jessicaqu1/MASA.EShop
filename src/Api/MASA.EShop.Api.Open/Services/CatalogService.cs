﻿using MASA.EShop.Api.Open.Callers.Catalog;

namespace MASA.EShop.Api.Open.Services
{
    public class CatalogService : ServiceBase
    {
        private readonly CatalogCaller _catalogCaller;

        public CatalogService(IServiceCollection services, CatalogCaller catalogCaller) : base(services)
        {
            _catalogCaller = catalogCaller;

            App.MapGet("/api/v1/catalog/{id}", GetAsync);
            App.MapGet("/api/v1/catalog/items", GetItemsAsync);
            App.MapGet("/api/v1/catalog/brands", CatalogBrandsAsync);
            App.MapGet("/api/v1/catalog/types", CatalogTypesAsync);
        }

        public async Task<IResult> GetAsync(int id)
        {
            return Results.Ok(await _catalogCaller.GetCatalogById(id));
        }

        public async Task<IResult> GetItemsAsync(int typeId = -1, int brandId = -1, int pageSize = 10, int pageIndex = 0)
        {
            return Results.Ok(await _catalogCaller.GetCatalogItemsAsync(pageIndex, pageSize, brandId, typeId));
        }

        public async Task<IResult> CatalogBrandsAsync()
        {
            return Results.Ok(await _catalogCaller.GetBrandsAsync());
        }

        public async Task<IResult> CatalogTypesAsync()
        {
            return Results.Ok(await _catalogCaller.GetTypesAsync());
        }

    }
}
