using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductManagement(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
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

        public async Task<ApiResponse<Product>> AssignToChatRoomAsync(List<Product> products, ChatRoom chatRoom, int duration)
        {
            var message = "";
            //chatRoom.ChatRoomProducts ??= new List<ChatRoomProduct>();
            //chatRoom.Users ??= new List<ChatRoomUser>();
            var productIds = products.Select(p => p.ProductId).ToList();
            var chatRoomProducts = chatRoom.ChatRoomProducts;
            foreach (var crp in chatRoomProducts)
            {
                //if (!productIds.Contains(crp.ProductId))
                //{
                //crp.IsDeleted = true;
                //crp.DeletedAt = DateTime.Now;
                //Product pr = await _dbcontext.Products.Include(p => p.ProductInStatuses).FirstOrDefaultAsync(p => p.ProductId == crp.ProductId);
                //pr.ProductInStatuses.Add(new ProductInStatus { ProductStatusId = 1 });
                //}
                _dbcontext.ChangeTracker.TrackGraph(crp, entity =>
                {
                    if (entity.Entry.State == EntityState.Unchanged)
                    {
                        entity.Entry.State = EntityState.Deleted;
                    }
                });
                _dbcontext.ChatRoomProducts.Remove(crp);
            }
            if (chatRoomProducts != null)
            {
                
               
            }
            
            var currentTime = chatRoom.EndDate;
            foreach (var product in products)
            {
                var vdv = DateTime.Now;
                //var existedInUpcoming = from crp in _dbcontext.ChatRoomProducts
                //                        join cr in _dbcontext.ChatRooms
                //                        on crp.ChatRoomId equals cr.ChatRoomId
                //                        where crp.ProductId == product.ProductId
                //                        && cr.EndDate > DateTime.Now
                //                        && cr.IsDeleted == false
                //                        select crp;
                //if (!existedInUpcoming.Any())
                //{
                    ChatRoomProduct chatRoomProduct = new ChatRoomProduct
                    {
                        ProductId = product.ProductId,
                        BiddingStartTime = currentTime,
                        BiddingEndTime = currentTime.AddMinutes(duration),
                    };
                    chatRoom.ChatRoomProducts.Add(chatRoomProduct);
                    product.ProductInStatuses?.Add(new ProductInStatus { ProductStatusId = 2 });
                    currentTime = currentTime.AddMinutes(duration + 5);
                    if (!chatRoom.Users.Any(u => u.UserId == product.SellerId))
                    {
                        chatRoom.Users.Add(new ChatRoomUser
                        {
                            UserId = product.SellerId,
                            ChatRoomId = chatRoom.ChatRoomId
                        });
                    }

                //}
                //else
                //{
                //    message += " some product can not be added to this chat room duo to it's assigned to another one";
                //}
            }
            var totalTime = products.Count * (duration + 5);
            chatRoom.EndDate = chatRoom.EndDate.AddMinutes(totalTime);
            chatRoom.CustomDuration = duration;
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Product> { IsSuccess = true, Message = "Assign product to chat room succesfully" + message, StatusCode = 201 };
            }

            return new ApiResponse<Product> { IsSuccess = false, Message = "Assign product to chat room failed " + message, StatusCode = 400 };
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<Product>> CreateProductAsync(CreateProductModel createProductModel, string UserId)
        {
            List<ProductImage> productImages = new List<ProductImage>();
            if (createProductModel.Files != null)
            {
                foreach (var file in createProductModel.Files)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var rDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                    var imagePath = Path.Combine(rDirectory, "Chat.Service", "Images", "ProductImages", fileName);

                    using (var stream = File.Create(imagePath))
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
            var products = await _dbcontext.Products
            .Include(p => p.Images)
            .Include(p => p.ChatRoomProducts)
            .Include(p => p.ProductInStatuses)
            .Include(p => p.Biddings)
            .Where(p => p.SellerId == UserId && p.IsDeleted == false)
            .Select(p => new Product
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                InitialPrice = p.InitialPrice,
                MinimumStep = p.MinimumStep,
                SellerId = p.SellerId,
                Images = p.Images,
                Biddings = p.Biddings,
                ProductInStatuses = p.ProductInStatuses,
                ChatRoomProducts = p.ChatRoomProducts
            })
            .ToListAsync();

            var a = products[0];
            if (products == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product by current time successfully", Response = products, StatusCode = 200 };

        }

        public async Task<ApiResponse<List<BiddingFare>>> GetListBiddingFares()
        {
            var statuses = await _dbcontext.BiddingFares.ToListAsync();
            if (statuses == null)
            {
                return new ApiResponse<List<BiddingFare>> { IsSuccess = false, Message = "No bidding fares", StatusCode = 404 };
            }
            return new ApiResponse<List<BiddingFare>> { IsSuccess = true, Message = "Get bidding fares list successfully", Response = statuses, StatusCode = 200 };
        }

        public async Task<ApiResponse<Message>> DeleteProductAsync(int ProductId)
        {
            var product = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ProductInStatuses).Include(p => p.ChatRoomProducts).FirstOrDefaultAsync(p => p.ProductId == ProductId);
            if (product == null)
            {
                return new ApiResponse<Message> { IsSuccess = false, Message = "Error", StatusCode = 404 };
            }
            _dbcontext.ChangeTracker.TrackGraph(product, entity =>
            {
                if (entity.Entry.State == EntityState.Unchanged)
                {
                    entity.Entry.State = EntityState.Deleted;
                }
            });

            _dbcontext.Products.Remove(product);

            //product.IsDeleted = true;
            //product.DeletedAt = DateTimeOffset.UtcNow;

            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
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

                    var rDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                    var imagePath = Path.Combine(rDirectory, "Chat.Service", "Images", "ProductImages", fileName);

                    using (var stream = File.Create(imagePath))
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
            var productsWithLatestStatus = await _dbcontext.Products
                .Select(p => new
                {
                    Product = new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        InitialPrice = p.InitialPrice,
                        MinimumStep = p.MinimumStep,
                        SellerId = p.SellerId,
                        Images = p.Images,
                        Biddings = p.Biddings,
                        IsDeleted = p.IsDeleted,
                        ProductInStatuses = p.ProductInStatuses,
                        ChatRoomProducts = p.ChatRoomProducts
                    },
                    LatestProductStatus = p.ProductInStatuses.OrderByDescending(ps => ps.Timestamp)
                                                            .FirstOrDefault()
                })
                .Where(x => x.LatestProductStatus != null && x.LatestProductStatus.ProductStatusId == statusId && x.Product.IsDeleted == false)
                .Select(x => x.Product)
                .ToListAsync();
            var a = productsWithLatestStatus[0];
            if (productsWithLatestStatus == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product by current time successfully", Response = productsWithLatestStatus, StatusCode = 200 };
        }

        public async Task<ApiResponse<List<string>>> GetImageNames(int ProductId)
        {
            var products = await _dbcontext.ProductImages
                .Where(p => p.ProductId == ProductId)
                .ToListAsync();
            List<string> imgNames = new List<string>();

            foreach (var product in products)
            {
                imgNames.Add(product.Image);
            }

            if (imgNames == null)
            {
                return new ApiResponse<List<string>> { IsSuccess = false, Message = "No image with that product id", StatusCode = 404 };
            }
            return new ApiResponse<List<string>> { IsSuccess = true, Message = "Find images by productid successfully", Response = imgNames, StatusCode = 200 };
        }

        public async Task<ApiResponse<List<Product>>> GetBiddingProductsFromUser(string UserId)
        {

            var productsHadBidding = await _dbcontext.Products.Include(p => p.Images).Include(p => p.ChatRoomProducts).Include(p => p.Biddings)
                .Where(p => p.Biddings.Any(b => b.BiddingUserId == UserId) &&
                            p.SellerId != UserId &&
                            !p.IsDeleted)
                .ToListAsync();
            var a = productsHadBidding[0];
            if (productsHadBidding == null)
            {
                return new ApiResponse<List<Product>> { IsSuccess = false, Message = "No product with that user id", StatusCode = 404 };
            }
            return new ApiResponse<List<Product>> { IsSuccess = true, Message = "Find product by user id successfully", Response = productsHadBidding, StatusCode = 200 };
        }
    }
}
