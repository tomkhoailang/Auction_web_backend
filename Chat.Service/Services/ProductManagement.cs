using Chat.Data.Data;
using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Chat.Service.Services
{
    public class ProductManagement : IProductManagement
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProductManagement(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<ApiResponse<Product>> GetProductAsync(int ProductId)
        {
            Product product = await _dbcontext.Products.Include(p => p.Biddings).Include(p => p.Images).Include(p => p.ProductInStatuses).ThenInclude(p => p.ProductStatus).FirstOrDefaultAsync(p => p.ProductId == ProductId);
            if (product == null)
            {
                return new ApiResponse<Product> { IsSuccess = false, Message = "No product found with that id", StatusCode = 404 };
            }
            return new ApiResponse<Product> { IsSuccess = true, Message = "product is found", StatusCode = 200, Response = product };
        }
        public async Task<ApiResponse<List<Product>>> GetMultipleProductsAsync(List<int> productIds)
        {
            var products = await _dbcontext.Products.Where(p => productIds.Contains(p.ProductId)).Include(p => p.ProductInStatuses).Include(p => p.Seller).ToListAsync();
            if (products == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product found with that id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "product is found", StatusCode = 200, Response = products };
        }

        public async Task<ApiResponse<Product>> AssignToChatRoomAsync(List<Product> products, ChatRoom chatRoom)
        {
            var message = "";
            chatRoom.ChatRoomProducts ??= new List<ChatRoomProduct>();
            chatRoom.Users ??= new List<ApplicationUser>();
            var currentTime = chatRoom.EndDate;
            foreach (var product in products)
            {
                var existedInUpcoming = from crp in _dbcontext.ChatRoomProducts
                                        join cr in _dbcontext.ChatRooms
                                        on crp.ChatRoomId equals cr.ChatRoomId
                                        where crp.ProductId == product.ProductId
                                        && cr.EndDate > DateTime.UtcNow
                                        select crp;
                if (!product.IsSold && !existedInUpcoming.Any())
                {
                    ChatRoomProduct chatRoomProduct = new ChatRoomProduct
                    {
                        ProductId = product.ProductId,
                        BiddingStartTime = currentTime,
                        BiddingEndTime = currentTime.AddMinutes(30),  
                    };
                    chatRoom.ChatRoomProducts.Add(chatRoomProduct);
                    product.ProductInStatuses?.Add(new ProductInStatus { ProductStatusId = 2 });
                    currentTime = currentTime.AddMinutes(35);
                    if (!chatRoom.Users.Any(u => u.Id == product.SellerId))
                    {
                        chatRoom.Users.Add(product.Seller);
                    }

                }
                else
                {
                    message += " some product can not be added to this chat room duo to it's assigned to another one";
                }
            }
            var totalTime = products.Count * 35;
            chatRoom.EndDate = chatRoom.EndDate.AddMinutes(totalTime);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Product> { IsSuccess = true, Message = "Assign product to chat room succesfully" + message, StatusCode = 201 };
            }

            return new ApiResponse<Product> { IsSuccess = false, Message = "Assign product to chat room failed " + message, StatusCode = 400 };
        }

        public async Task<ApiResponse<Product>> CreateProductAsync(CreateProductModel createProductModel, string UserId)
        {
            List<ProductImage> productImages = new List<ProductImage>();
            if (createProductModel.Files != null)
            {
                foreach (var file in createProductModel.Files)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //change this path to the one in assets/productImages in angular project
                    //var path = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Máy tính\\code\\source code\\api/bidding_web\\Auction_web_backend\\Chat.Service\\Images\\ProductImages", fileName);

                    var path = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Máy tính\\code\\source code\\api\\bidding_web\\Auction_web\\src\\productImages", fileName);

                    using (var stream = File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ProductImage productImage = new ProductImage
                    {
                        Image = fileName
                    };
                    productImages.Add(productImage);
                }
            }
            List<ProductInStatus> productInStatuses = new List<ProductInStatus>();
            ProductInStatus productInStatus = new ProductInStatus { ProductStatusId = 1 };
            productInStatuses.Add(productInStatus);

            Product product = new Product()
            {
                Name = createProductModel.Name,
                Description = createProductModel.Description,
                InitialPrice = createProductModel.InitialPrice,
                MinimumStep = createProductModel.MinimumStep,
                SellerId = UserId,
                Images = productImages,
                ProductInStatuses = new List<ProductInStatus>() { new ProductInStatus { ProductStatusId = 1 } }
            };

            await _dbcontext.Products.AddAsync(product);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Product> { IsSuccess = true, Message = "Create product successfully", Response = product, StatusCode = 201 };
            }
            return new ApiResponse<Product> { IsSuccess = false, Message = "Create product failed", Response = product, StatusCode = 400 };
        }

        public async Task<ApiResponse<List<Product>>> GetProductListFromChatRoomAsync(int ChatRoomId)
        {
            var products = await _dbcontext.ChatRoomProducts.Include(crp => crp.Product).ThenInclude(p => p.ChatRoomProducts).OrderBy(crp => crp.BiddingEndTime).Include(crp => crp.Product).ThenInclude(p => p.Images).Include(crp => crp.Product).ThenInclude(p => p.Biddings).Where(crp => crp.ChatRoomId == ChatRoomId).Select(crp => crp.Product).ToListAsync();
            if (products == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that chat room id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product successfully", Response = products, StatusCode = 200 };
        }

        public async Task<ApiResponse<Product>> GetCurrentBiddingProductFromChatRoomAsync(int ChatRoomId)
        {
            var currentProduct = await _dbcontext.ChatRoomProducts.Include(c => c.Product).ThenInclude(p => p.Biddings).Where(c => c.ChatRoomId == ChatRoomId && c.BiddingStartTime >= DateTime.Now && DateTime.Now <= c.BiddingEndTime).Select(c => c.Product).ToListAsync();
            return new ApiResponse<Product> { IsSuccess = true, Message = "Find product by current time successfully", Response = currentProduct[0], StatusCode = 200 };
        }
        public async Task<ApiResponse<Bidding>> CreateBiddingAsync(CreateBiddingModel createBiddingModel)
        {
            Bidding bidding = new Bidding()
            {
                BiddingAmount = createBiddingModel.BiddingAmount,
                BiddingUserId = createBiddingModel.BiddingUserId,
                ProductId = createBiddingModel.ProductId,
            };
            await _dbcontext.Biddings.AddAsync(bidding);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Bidding> { IsSuccess = true, Message = "Bidding successfully", StatusCode = 201 };
            }
            return new ApiResponse<Bidding> { IsSuccess = false, Message = "Bidding failed", StatusCode = 400 };
        }

        public Task<ApiResponse<bool>> IsValidBidding(decimal biddingAmount, Product product)
        {
            decimal minBidding = product.MinimumStep;
            if (product.Biddings == null || product.Biddings.Count == 0)
            {
                minBidding += product.InitialPrice;
            }
            else
            {
                minBidding += product.Biddings.Max(b => b.BiddingAmount);
            };
            if (biddingAmount < minBidding)
            {
                return Task.FromResult(new ApiResponse<bool> { IsSuccess = false, Message = "Min bidding is not reached", StatusCode = 400, Response = false });
            }
            return Task.FromResult(new ApiResponse<bool> { IsSuccess = true, Message = "Min bidding is qualified", StatusCode = 200, Response = true });
        }

        public async Task<ApiResponse<List<Product>>> GetProductFromUserAsync(string UserId)
        {
            var products = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ChatRoomProducts).Include(p => p.ProductInStatuses).ThenInclude(p => p.ProductStatus).Include(p => p.Biddings).Where(p => p.SellerId == UserId).ToListAsync();
            var a = products[0];
            if (products == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product by current time successfully", Response = products, StatusCode = 200 };

        }

        public async Task<ApiResponse<List<ProductStatus>>> GetListProductStatus()
        {
            var statuses = await _dbcontext.ProductStatuses.ToListAsync();
            if (statuses == null)
            {
                return new ApiResponse<List<ProductStatus>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<ProductStatus>> { IsSuccess = true, Message = "Get productstatus list successfully", Response = statuses, StatusCode = 200 };
        }

        public async Task<ApiResponse<Message>> DeleteProductAsync(int ProductId)
        {
            var product = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ProductInStatuses).FirstOrDefaultAsync(p => p.ProductId == ProductId);
            if (product == null)
            {
                return new ApiResponse<Message> { IsSuccess = false, Message = "Error", StatusCode = 404 };
            }
            //var imagelist = await _dbcontext.ProductImages.Where(p => p.ProductId == ProductId).ToListAsync();
            //foreach(var image in imagelist)
            //{
            //    _dbcontext.ProductImages.Remove(image);
            //}
            
            _dbcontext.Products.Remove(product);    
            var rs = await _dbcontext.SaveChangesAsync();
            if(rs > 0)
            {
                return new ApiResponse<Message> { IsSuccess = true, Message = "Bidding successfully", StatusCode = 201 };
            }
            return new ApiResponse<Message> { IsSuccess = true, Message = "Delete product successfully", Response = new Message(), StatusCode = 400 };
        }


        public async Task<ApiResponse<Product>> EditProductAsync(CreateProductModel createProductModel, int productId, string UserId)
        {
            List<ProductImage> productImages = new List<ProductImage>();
            if (createProductModel.Files != null)
            {
                foreach (var file in createProductModel.Files)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //change this path to the one in assets/productImages in angular project
                    //var path = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Máy tính\\code\\source code\\api/bidding_web\\Auction_web_backend\\Chat.Service\\Images\\ProductImages", fileName);
                    var path = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Máy tính\\code\\source code\\api\\bidding_web\\Auction_web\\src\\productImages", fileName);

                    using (var stream = File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ProductImage productImage = new ProductImage
                    {
                        Image = fileName
                    };
                    productImages.Add(productImage);
                }
            }

            var oldProduct = await _dbcontext.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == productId);

            if (oldProduct != null)
            {
                _dbcontext.ProductImages.RemoveRange(oldProduct.Images);

                oldProduct.Name = createProductModel.Name;
                oldProduct.Description = createProductModel.Description;
                oldProduct.InitialPrice = createProductModel.InitialPrice;
                oldProduct.MinimumStep = createProductModel.MinimumStep;
                oldProduct.Images = productImages;
            }
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Product> { IsSuccess = true, Message = "Create product successfully", Response = oldProduct, StatusCode = 201 };
            }
            return new ApiResponse<Product> { IsSuccess = false, Message = "Create product failed", Response = oldProduct, StatusCode = 400 };
        }

        public async Task<ApiResponse<Product>> ContinueBidding(int ProductId)
        {
            var oldProduct = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ProductInStatuses).FirstOrDefaultAsync(p => p.ProductId == ProductId);
            if (oldProduct.ProductInStatuses.ElementAt(oldProduct.ProductInStatuses.Count - 1).ProductStatusId == 2)
            {
                oldProduct.ProductInStatuses.Add(new ProductInStatus { ProductStatusId = 1 });
            }
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Product> { IsSuccess = true, Message = "Create product successfully", Response = oldProduct, StatusCode = 201 };
            }
            return new ApiResponse<Product> { IsSuccess = false, Message = "Create product failed", Response = oldProduct, StatusCode = 400 };
        }

        public async Task<ApiResponse<List<Product>>> GetProductsWithStatus(int statusId)
        {
            var products = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ChatRoomProducts).ThenInclude(crp => crp.ChatRoom).Include(p => p.ProductInStatuses).ThenInclude(p => p.ProductStatus).ToListAsync();

            var productsWithLatestStatus = await _dbcontext.Products
                .Select(p => new
                {
                    Product = p,
                    LatestProductStatus = p.ProductInStatuses.OrderByDescending(ps => ps.Timestamp)
                                                            .FirstOrDefault()
                })
                .Where(x => x.LatestProductStatus != null && x.LatestProductStatus.ProductStatusId == statusId)
                .Select(x => x.Product)
                .ToListAsync();
            var a = products[0];
            if (products == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product by current time successfully", Response = productsWithLatestStatus, StatusCode = 200 };
        }

    }
}
