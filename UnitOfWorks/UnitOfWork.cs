using Blink_API.Models;
using Blink_API.Repositories;
using Blink_API.Repositories;

using Blink_API.Repositories.BrandRepository;

using Blink_API.Repositories.BranchRepos;
 
using Blink_API.Repositories.CartRepos;
using Blink_API.Repositories.DiscountRepos;
using Blink_API.Repositories.InventoryRepos;
<<<<<<< HEAD

using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Blink_API.Repositories.Order;
using AutoMapper;

using Blink_API.Repositories.BiDataRepos;
using Blink_API.Repositories.Payment;

=======
<<<<<<< HEAD
using Blink_API.Repositories.BiDataRepos;
=======
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Blink_API.Repositories.Order;
using AutoMapper;
<<<<<<< HEAD
>>>>>>> c646337 (resolve Secret key2)
<<<<<<< HEAD
>>>>>>> 7c1b2dc (create PAyment f)
=======
=======

using Blink_API.Repositories.BiDataRepos;
using Blink_API.Repositories.Payment;

>>>>>>> 4815246 (testt)
>>>>>>> 256852f (Create PAymeeent)

namespace Blink_API
{
    public class UnitOfWork
    {
        private readonly BlinkDbContext db;
<<<<<<< HEAD


        

        internal object cart_DiminsionRepos;

=======
<<<<<<< HEAD
        internal object cart_DiminsionRepos;
=======

<<<<<<< HEAD
        public DbContext Context => db;
>>>>>>> c646337 (resolve Secret key2)
<<<<<<< HEAD
>>>>>>> 7c1b2dc (create PAyment f)
=======
=======
        

        internal object cart_DiminsionRepos;

>>>>>>> 4815246 (testt)
>>>>>>> 256852f (Create PAymeeent)
        BrandRepos brandRepo;
        ProductRepo productRepo;
        CategoryRepo categoryRepo;
        DiscountRepo discountRepo;
        CartRepo cartRepo;
        CartDetailsRepo cartDetailsRepo;
        BranchRepos branchRepos;
        InventoryRepo inventoryRepo;
        orderRepo orderRepo;
<<<<<<< HEAD
<<<<<<< HEAD
        PaymentRepository paymentRepository;
=======
>>>>>>> 7c1b2dc (create PAyment f)
=======
        PaymentRepository paymentRepository;
>>>>>>> 256852f (Create PAymeeent)

        // **** for bi ***
        BiDataRepos biDataRepos;
        Review_DimensionRepos reviewDiminsionRepo;
        paymentDiminsionRepos paymentDiminsionRepo;
        UserRoles_DimensionRepos userRoleRepo;
        Role_DiminsionRepos identityRoleRepo;
        User_DiminsionRepos userDiminsionRepos;
        Product_DiscountRepos productDiscountRepo;
        Inventory_transactionRepo inventoryTransactionRepo;
        cart_DiminsionRepos _cartDiminsionRepos;
        order_FactRepos orderFactRepos;
        Discount_DiminsionRepo DiminsionRepo;
        Branch_InventoryRepos branchInventoryRepos;
        InventoryTransaction_factRepos inventoryTransactionfactRepo;
        Product_DiminsionRepos _productDiminsionRepos;
        public UnitOfWork(BlinkDbContext _db)
        {
            db = _db;
        }

       

        public orderRepo OrderRepo
        {
            get
            {
                if (orderRepo == null)
                {
                    orderRepo = new orderRepo(db);
                }
                return orderRepo;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
  
=======
>>>>>>> 7c1b2dc (create PAyment f)
=======
  
>>>>>>> 256852f (Create PAymeeent)
        public ProductRepo ProductRepo
        {
            get
            {
                if (productRepo == null)
                {
                    productRepo = new ProductRepo(db);
                }
                return productRepo;
            }
        }

        public InventoryRepo InventoryRepo
        {
            get
            {
                if (inventoryRepo == null)
                {
                    inventoryRepo = new InventoryRepo(db);
                }
                return inventoryRepo;
            }
        }

        public CategoryRepo CategoryRepo
        {
            get
            {
                if (categoryRepo == null)
                {
                    categoryRepo = new CategoryRepo(db);
                }
                return categoryRepo;
            }
        }

        public DiscountRepo DiscountRepo
        {
            get
            {
                if (discountRepo == null)
                {
                    discountRepo = new DiscountRepo(db);
                }
                return discountRepo;
            }
        }


        public CartRepo CartRepo
        {
            get
            {
                if (cartRepo == null)
                {
                    cartRepo = new CartRepo(db);
                }
                return cartRepo;
            }
        }

        public CartDetailsRepo CartDetailsRepo
        {
            get
            {
                if (cartDetailsRepo == null)
                {
                    cartDetailsRepo = new CartDetailsRepo(db);
                }
                return cartDetailsRepo;
            }
        }


        public BrandRepos BrandRepos
        {
            get
            {
                if (brandRepo == null)
                {
                    brandRepo = new BrandRepos(db);
                }
                return brandRepo;
            }
        }



        public BranchRepos BranchRepos
        {
            get
            {
                if (branchRepos == null)
                {
                    branchRepos = new BranchRepos(db);
                }
                return branchRepos;
            }
        }
<<<<<<< HEAD


        public async Task<int> CompleteAsync()
         => await db.SaveChangesAsync();

=======
<<<<<<< HEAD
>>>>>>> 7c1b2dc (create PAyment f)
        // *************************************  for bidatarepos ************************************:
        public BiDataRepos BiDataRepos
        {
            get
            {
                if (biDataRepos == null)
                {
                    biDataRepos = new BiDataRepos(db);
                }
                return biDataRepos;
            }
        }

        // review_diminsion :
        public Review_DimensionRepos Review_DimensionRepos
        {
            get
            {
                if (reviewDiminsionRepo == null)
                {
                    reviewDiminsionRepo = new Review_DimensionRepos(db);
                }
                return reviewDiminsionRepo;
            }
        }

        // payment diminsion :
        public paymentDiminsionRepos Payment_DimensionRepos
        {
            get
            {
                if (paymentDiminsionRepo == null)
                {
                    paymentDiminsionRepo = new paymentDiminsionRepos(db);
                }
                return paymentDiminsionRepo;
            }
        }
        // user role :
        public UserRoles_DimensionRepos UserRoleRepo
        {
            get
            {
                if (userRoleRepo == null)
                {
                    userRoleRepo = new UserRoles_DimensionRepos(db);
                }
                return userRoleRepo;
            }
        }

        // identity roles :
        public Role_DiminsionRepos IdentityRoleRepo
        {
            get
            {
                if (identityRoleRepo == null)
                {
                    identityRoleRepo = new Role_DiminsionRepos(db);
                }
                return identityRoleRepo;
            }
        }

        // user :
        public User_DiminsionRepos UserDiminsionRepos  
        {
            get
            {
                if (userDiminsionRepos == null)
                {
                    userDiminsionRepos = new User_DiminsionRepos(db);
                }
                return userDiminsionRepos;
            }
        }

        // product dicount :
        public Product_DiscountRepos ProductDiscountRepo
        {
            get
            {
                if (productDiscountRepo == null)
                {
                    productDiscountRepo = new Product_DiscountRepos(db);
                }
                return productDiscountRepo;
            }
        }

        
        public Inventory_transactionRepo InventoryTransactionRepo
        {
            get
            {
                if (inventoryTransactionRepo == null)
                {
                    inventoryTransactionRepo = new Inventory_transactionRepo(db);
                }
                return inventoryTransactionRepo;
            }
            private set  
            {
                inventoryTransactionRepo = value;
            }
        }

        // cart diminsion:

        public cart_DiminsionRepos CartDiminsionRepos
        {
            get
            {
                if (_cartDiminsionRepos == null)
                {
                    _cartDiminsionRepos = new cart_DiminsionRepos(db);
                }
                return _cartDiminsionRepos;
            }
        }
        
        // order facts :
        public order_FactRepos OrderFactRepos
        {
            get
            {
                if (orderFactRepos == null)
                {
                    orderFactRepos = new order_FactRepos(db);
                }
                return orderFactRepos;
            }
        }

        // discount :
       
        public Discount_DiminsionRepo DiscountDiminsionRepo
        {
            get
            {
                if (DiminsionRepo == null)
                {
                    DiminsionRepo = new Discount_DiminsionRepo(db);
                }
                return DiminsionRepo;
            }
        }

        
        public Branch_InventoryRepos BranchInventoryRepos
        {
            get
            {
                if (branchInventoryRepos == null)
                {
                    branchInventoryRepos = new Branch_InventoryRepos(db);
                }
                return branchInventoryRepos;
            }
        }

        // inventory transaction fact :
        public InventoryTransaction_factRepos InventoryTransactionFactRepos
        {
            get
            {
                if (inventoryTransactionfactRepo == null)
                {
                    inventoryTransactionfactRepo = new InventoryTransaction_factRepos(db);
                }
                return inventoryTransactionfactRepo;
            }
        }


        // product dimintion :


        public Product_DiminsionRepos ProductDiminsionRepos
        {
            get
            {
                if (_productDiminsionRepos == null)
                {
                    _productDiminsionRepos = new Product_DiminsionRepos(db);
                }
                return _productDiminsionRepos;
            }
        }

<<<<<<< HEAD

=======
>>>>>>> 11de893 (complete bi)
    }
    
=======

        public async Task<int> CompleteAsync()
         => await db.SaveChangesAsync();

    }
>>>>>>> c646337 (resolve Secret key2)
}
