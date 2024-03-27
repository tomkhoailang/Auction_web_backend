using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.Product;

namespace Chat.Service.Services
{
    public interface IProductManagement
    {
        public Task<ApiResponse<Product>> CreateProductAsync(CreateProductModel createProductModel, string UserId);
        public Task<ApiResponse<Product>> AssignToChatRoomAsync(List<Product> products, ChatRoom chatRoom);
        public Task<ApiResponse<Product>> GetProductAsync(int ProductId);

        public Task<ApiResponse<List<Product>>> GetMultipleProductsAsync(List<int> productIds);

        public Task<ApiResponse<List<Product>>> GetProductFromUserAsync(string UserId);
        public Task<ApiResponse<List<ProductStatus>>> GetListProductStatus();
        public Task<ApiResponse<List<Product>>> GetProductListFromChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<Product>> GetCurrentBiddingProductFromChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<Bidding>> CreateBiddingAsync(CreateBiddingModel createBiddingModel);
        public Task<ApiResponse<bool>> IsValidBidding(decimal biddingAmount, Product product);
        public Task<ApiResponse<Message>> DeleteProductAsync(int ProductId);
        public Task<ApiResponse<Product>> EditProductAsync(CreateProductModel createProductModel, int productId, string UserId);
        public Task<ApiResponse<List<Product>>> GetProductsWithStatus(int statusId);
        public Task<ApiResponse<Product>> ContinueBidding(int ProductId);
        public Task<ApiResponse<List<string>>> GetImageNames(int ProductId);
    }
}
