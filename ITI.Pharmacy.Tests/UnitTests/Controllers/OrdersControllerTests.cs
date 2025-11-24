using ITI.Pharmacy.API.Controllers.User;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.Tests.UnitTests.Controllers;

public class OrdersControllerTests
{
    [Fact]
    public async Task CreateOrder_ReturnsCreatedResult()
    {
        var fakeService = new FakeOrderService
        {
            OrderToReturn = new OrderDTO { Id = 10, OrderNumber = "ORD-001" }
        };
        var controller = BuildController(fakeService);

        var result = await controller.CreateOrder(new CreateOrderDTO());

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var order = Assert.IsAssignableFrom<OrderDTO>(created.Value);
        Assert.Equal(10, order.Id);
    }

    [Fact]
    public async Task CancelOrder_WhenServiceFails_ReturnsBadRequest()
    {
        var fakeService = new FakeOrderService { CancelResponse = false };
        var controller = BuildController(fakeService);

        var response = await controller.CancelOrder(1);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    private static OrdersController BuildController(IOrderService service)
    {
        var controller = new OrdersController(service);
        var userIdClaim = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "user-1")
        });
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(userIdClaim) }
        };
        return controller;
    }

    private sealed class FakeOrderService : IOrderService
    {
        public OrderDTO? OrderToReturn { get; set; }
        public bool CancelResponse { get; set; } = true;

        public Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
            => Task.FromResult(OrderToReturn ?? new OrderDTO());

        public Task<bool> CancelOrderAsync(int id, string userId) => Task.FromResult(CancelResponse);

        public Task<List<OrderDTO>> GetUserOrdersAsync(string userId) => Task.FromResult(new List<OrderDTO>());
        public Task<OrderDTO> GetOrderByIdAsync(int id, string? userId = null) => Task.FromResult(OrderToReturn ?? new OrderDTO());
        public Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto) => Task.FromResult(true);
        public Task<List<OrderDTO>> GetAllOrdersAsync() => Task.FromResult(new List<OrderDTO>());
        public Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId) => Task.FromResult(new List<OrderDTO>());
    }
}

