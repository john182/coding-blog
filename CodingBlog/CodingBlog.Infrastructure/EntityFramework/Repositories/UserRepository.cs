namespace CodingBlog.Infrastructure.EntityFramework.Repositories;

using Domain.Entities;
using CodingBlog.Domain.Repositories;
using Configuration;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly PostgresDBContext _context;

    public UserRepository(PostgresDBContext context)
        =>  _context = context;
    
    public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
      => await _context.Users.FirstOrDefaultAsync(u => u.Email == username, cancellationToken);

    public async Task<User> Create(User user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }
}