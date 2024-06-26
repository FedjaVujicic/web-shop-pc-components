﻿using ComponentShopAPI.Dtos;
using ComponentShopAPI.Entities;

namespace ComponentShopAPI.Services.CartManager
{
    public interface ICartManager
    {
        public Task AddProductToCartAsync(Cart cart, Product product);

        public Task RemoveProductFromCartAsync(Cart cart, Product product, int quantity);

        public Task<Cart> CreateCartAsync(string userId);

        public Task<Cart?> GetCartAsync(string userId);

        public bool IsCartEmpty(Cart cart);

        public bool CheckProductsAvailable(Cart cart);

        public Task<double> GetCartTotalAsync(Cart cart);

        public Task ProcessPurchaseAsync(Cart cart, ApplicationUser user);

        public List<CartDto> GetCartDtos(Cart cart);
    }
}
