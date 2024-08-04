using Techshop.Models.Entities;

namespace Techshop.Repository;

public class UnitOfWork
{
    private readonly AppDbContext _context;

    public readonly Repository<CartItem> CartItemRepository;
    public readonly Repository<Category> CategoryRepository;
    public readonly Repository<Image> ImageRepository;
    public readonly Repository<Order> OrderRepository;
    public readonly Repository<OrderItem> OrderItemRepository;
    public readonly Repository<Product> ProductRepository;
    public readonly Repository<Role> RoleRepository;
    public readonly Repository<User> UserRepository;

    public UnitOfWork()
    {
        _context = AppDbContext.Init();
        CartItemRepository = new Repository<CartItem>(_context);
        CategoryRepository = new Repository<Category>(_context);
        ImageRepository = new Repository<Image>(_context);
        OrderRepository = new Repository<Order>(_context);
        OrderItemRepository = new Repository<OrderItem>(_context);
        ProductRepository = new Repository<Product>(_context);
        UserRepository = new Repository<User>(_context);
        RoleRepository = new Repository<Role>(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}