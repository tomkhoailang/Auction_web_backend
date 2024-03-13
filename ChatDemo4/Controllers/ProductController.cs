using Chat.Service.Models;
using Chat.Service.Models.Product;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductManagement _productManager;
        private readonly IUserManagement _userManager;

        public ProductController(IProductManagement productManager, IUserManagement userManager)
        {
            _productManager = productManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(IFormCollection createProductForm)
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var createProductModel = new CreateProductModel();
            createProductModel.Name = createProductForm["Name"]!;
            createProductModel.Description = createProductForm["Description"]!;
            createProductModel.InitialPrice = decimal.Parse(createProductForm["InitialPrice"]!);
            createProductModel.MinimumStep = decimal.Parse(createProductForm["MinimumStep"]!);
            createProductModel.Files = createProductForm.Files;

            var createProductRes = await _productManager.CreateProductAsync(createProductModel, userInfoRes.Response!.Id);
            return StatusCode(201, new { createProductRes.Message });
        }
        [Authorize]
        [HttpPost("{productId}/biddings")]
        public async Task<IActionResult> createBidding(int productId, [FromBody] CreateBiddingModel createBiddingModel)
        {
            var getProduct = await _productManager.GetProductAsync(productId);
            if (!getProduct.IsSuccess)
            {
                return StatusCode(getProduct.StatusCode, new { getProduct.Message });
            }
            var isValidBidding = _productManager.IsValidBidding(createBiddingModel.BiddingAmount, getProduct.Response!).Result;
            if (!isValidBidding.IsSuccess)
            {
                return StatusCode(isValidBidding.StatusCode, new { isValidBidding.Message });
            }
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);
            createBiddingModel.ProductId = productId;
            createBiddingModel.BiddingUserId = userInfoRes.Response!.Id;
            var createBidding = await _productManager.CreateBiddingAsync(createBiddingModel);
            return StatusCode(createBidding.StatusCode, new { createBidding.Message });
        }
    }
}
