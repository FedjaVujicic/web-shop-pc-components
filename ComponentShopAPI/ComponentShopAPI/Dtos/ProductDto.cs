﻿using ComponentShopAPI.Models;
using ComponentShopAPI.Services.Image;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ComponentShopAPI.Dtos
{
    [JsonDerivedType(typeof(MonitorDto))]
    [JsonDerivedType(typeof(GpuDto))]
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public bool Availability { get; set; }
        public FileContentResult? ImageFile { get; set; }

        public ProductDto(Product product, IImageService imageService)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Availability = product.Quantity > 0;
            ImageFile = imageService.Download(product.ImageName);
        }
    }
}
