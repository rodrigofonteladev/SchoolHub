using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Product;

namespace SchoolHub.Services
{
    public class ProductService(AppDbContext context) : IProductService
    {
        public async Task<List<ListProductViewModel>> GetAllAsync() => await context.Products.AsNoTracking().OrderBy(item => item.Name).Select(item => new ListProductViewModel
        { Id = item.Id, Name = item.Name, Description = item.Description, PricePoints = item.PricePoints, Stock = item.Stock, PurchaseCount = item.PurchaseDetails.Count }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterProductViewModel model)
        {
            if (await context.Products.AnyAsync(item => item.Name == model.Name)) return Result.Failure("A product with that name already exists.");
            await context.Products.AddAsync(new Product { Name = model.Name, Description = model.Description, PricePoints = model.PricePoints, Stock = model.Stock });
            await context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<RegisterProductViewModel?> GetForEditAsync(int id) => await context.Products.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterProductViewModel { Name = item.Name, Description = item.Description, PricePoints = item.PricePoints, Stock = item.Stock }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterProductViewModel model)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity is null) return Result.Failure("The product does not exist.");
            if (await context.Products.AnyAsync(item => item.Id != id && item.Name == model.Name)) return Result.Failure("A product with that name already exists.");
            entity.Name = model.Name; entity.Description = model.Description; entity.PricePoints = model.PricePoints; entity.Stock = model.Stock; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
