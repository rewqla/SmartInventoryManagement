﻿using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

//todo: inherit base repo
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly InventoryContext _dbContext;

    public UserRepository(InventoryContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == emailOrPhone || u.PhoneNumber == emailOrPhone);
    }

    public async Task<User?> GetByIdWithRoles(Guid id)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}