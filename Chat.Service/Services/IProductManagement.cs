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
        public Task<ApiResponse<List<Product>>> GetProductListFromChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<Product>> GetCurrentBiddingProductFromChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<Bidding>> CreateBiddingAsync(CreateBiddingModel createBiddingModel);
        public Task<ApiResponse<bool>> IsValidBidding(decimal biddingAmount, Product product);



    }
}
