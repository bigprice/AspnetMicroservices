﻿using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name = "GetShoopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // Get basket with username
            var basket = await _basketService.GetBasket(userName);

            // Iterate basket items and consume prducts with basket item productid member
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // Set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            // Map product related members into basket item dto with extended column


            // Consume ordering microservices in order to retrieve order list
            var orders = await _orderService.GetOrdersByUserName(userName);

            // return root ShoppingModel dto classs which including all responses
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                 BasketWithProducts = basket,
                 Orders = orders
            };

            return Ok(shoppingModel);

        }

    }
}
