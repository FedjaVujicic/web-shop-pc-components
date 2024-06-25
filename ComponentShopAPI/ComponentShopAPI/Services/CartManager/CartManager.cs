﻿using ComponentShopAPI.Entities;
using ComponentShopAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComponentShopAPI.Services.CartManager
{
    public class CartManager : ICartManager
    {
        private readonly ComponentShopDBContext _context;

        public CartManager(ComponentShopDBContext context)
        {
            _context = context;
        }

        public async Task AddProductToCartAsync(Cart cart, Product product)
        {
            var cartProduct = await GetCartProduct(cart, product);
            if (cartProduct == null)
            {
                cart.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            cartProduct = await GetCartProduct(cart, product);
            cartProduct!.Quantity += 1;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductFromCartAsync(Cart cart, Product product, int quantity)
        {
            var cartProduct = await GetCartProduct(cart, product);
            if (cartProduct == null || cartProduct.Quantity == 0)
            {
                throw new BadHttpRequestException($"Product {product.Name} is not in cart");
            }
            if (quantity > cartProduct.Quantity)
            {
                throw new BadHttpRequestException($"Not enough products {product.Name} in cart.");
            }

            cartProduct.Quantity -= quantity;
            if (cartProduct.Quantity == 0)
            {
                cart.Products.Remove(product);
            }
            await _context.SaveChangesAsync();

            if (IsCartEmpty(cart))
            {
                await DeleteCartAsync(cart);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart?> GetCartAsync(string userId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        private async Task<CartProduct?> GetCartProduct(Cart cart, Product product)
        {
            return await _context.CartProduct.FirstOrDefaultAsync(cp => cp.CartId == cart.Id && cp.ProductId == product.Id);
        }

        public bool IsCartEmpty(Cart cart)
        {
            var productsInCart = GetProductsInCart(cart);
            return productsInCart.Count <= 0;
        }

        public List<CartProduct> GetProductsInCart(Cart cart)
        {
            return _context.CartProduct.Where(cp => cp.CartId == cart.Id).ToList();
        }

        public async Task<double> GetCartTotalAsync(Cart cart)
        {
            var cartProducts = GetProductsInCart(cart);

            double total = 0;
            foreach (var cartProduct in cartProducts)
            {
                var product = await GetProductByIdAsync(cartProduct.ProductId);
                if (product == null)
                {
                    throw new Exception("Product from cart was deleted");
                }
                total += cartProduct.Quantity * product.Price;
            }

            return total;
        }

        public async Task ProcessPurchaseAsync(Cart cart, ApplicationUser user)
        {
            var cartProducts = GetProductsInCart(cart);

            foreach (var cartProduct in cartProducts)
            {
                var product = await GetProductByIdAsync(cartProduct.ProductId);
                if (product == null)
                {
                    throw new Exception("Product from cart was deleted");
                }

                user.Credits -= cartProduct.Quantity * product.Price;

                product.Quantity -= cartProduct.Quantity;

                await RemoveProductFromCartAsync(cart, product, cartProduct.Quantity);
            }
        }
    }
}
