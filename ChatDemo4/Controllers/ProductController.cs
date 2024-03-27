using Chat.Service.Models;
using Chat.Service.Models.Product;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatusList()
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var listStatus = await _productManager.GetListProductStatus();
            if (!listStatus.IsSuccess)
            {
                return StatusCode(listStatus.StatusCode, new { listStatus.Message });
            }
            return StatusCode(listStatus.StatusCode, new { listStatus.Response });
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _productManager.DeleteProductAsync(productId);
            if (!product.IsSuccess)
            {
                return StatusCode(product.StatusCode, new { product.Message });
            }
            return StatusCode(product.StatusCode, new { product.Message });
        }

        //[Authorize]
        //[HttpPost("{productId}/edit")]
        //public async Task<IActionResult> EditProduct(IFormCollection createProductForm, int productId)
        //{
        //    var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

        //    var createProductModel = new CreateProductModel();
        //    createProductModel.Name = createProductForm["Name"]!;
        //    createProductModel.Description = createProductForm["Description"]!;
        //    createProductModel.InitialPrice = decimal.Parse(createProductForm["InitialPrice"]!);
        //    createProductModel.MinimumStep = decimal.Parse(createProductForm["MinimumStep"]!);
        //    createProductModel.Files = createProductForm.Files;

        //    var createProductRes = await _productManager.EditProductAsync(createProductModel, productId, userInfoRes.Response!.Id);
        //    return StatusCode(201, new { createProductRes.Message });
        //}
        [Authorize]
        [HttpPatch("{productId}")]
        public async Task<IActionResult> EditProduct(IFormCollection createProductForm, int productId)
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var createProductModel = new CreateProductModel();
            createProductModel.Name = createProductForm["Name"]!;
            createProductModel.Description = createProductForm["Description"]!;

            createProductModel.InitialPrice = Convert.ToDecimal(createProductForm["InitialPrice"]);
            createProductModel.MinimumStep = Convert.ToDecimal(createProductForm["MinimumStep"]);
            createProductModel.Files = createProductForm.Files;

            var createProductRes = await _productManager.EditProductAsync(createProductModel, productId, userInfoRes.Response!.Id);
            return StatusCode(201, new { createProductRes.Message });
        }

        [Authorize]
        [HttpPost("{productId}/productStatuses")]
        public async Task<IActionResult> ContinueBidding(int productId)
        {
            var createProductRes = await _productManager.ContinueBidding(productId);
            return StatusCode(201, new { createProductRes.Message });
        }


        [HttpGet("status/{statusId}")]
        public async Task<IActionResult> getProductsWithStatus(int statusId)
        {
            var listProduct = await _productManager.GetProductsWithStatus(statusId);
            if (!listProduct.IsSuccess)
            {
                return StatusCode(listProduct.StatusCode, new { listProduct.Message });
            }
            return StatusCode(listProduct.StatusCode, new { listProduct.Response });
        }

        [HttpGet("{chatRoomId}/get")]
        public async Task<IActionResult> getProductsFromChatRoom(int chatRoomId)
        {
            var listProduct = await _productManager.GetProductListFromChatRoomAsync(chatRoomId);
            if (!listProduct.IsSuccess)
            {
                return StatusCode(listProduct.StatusCode, new { listProduct.Message });
            }
            return StatusCode(listProduct.StatusCode, new { listProduct.Response });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> getProduct(int productId)
        {
            var product = await _productManager.GetProductAsync(productId);
            if (!product.IsSuccess)
            {
                return StatusCode(product.StatusCode, new { product.Message });
            }
            return StatusCode(product.StatusCode, new { product.Response });
        }
        [HttpGet("images")]
        public async Task<IActionResult> getImages([FromQuery] string imgName)
        {
            var rDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var imagePath = Path.Combine(rDirectory, "Chat.Service", "Images", "ProductImages", imgName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        [HttpGet("user")]
        public async Task<IActionResult> getProductListFromUser()
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var listProduct = await _productManager.GetProductFromUserAsync(userInfoRes.Response!.Id);
            if (!listProduct.IsSuccess)
            {
                return StatusCode(listProduct.StatusCode, new { listProduct.Message });
            }
            return StatusCode(listProduct.StatusCode, new { listProduct.Response });
        }

    }
}
