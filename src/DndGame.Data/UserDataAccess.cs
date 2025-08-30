
using DndGame.Data.Entities;
using DndGame.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace DndGame.Data;

public class UserDataAccess : IUserDataAccess
{
    private readonly DndGameContext _db;

    public UserDataAccess(DndGameContext db)
    {
        _db = db;
    }

    public async Task<string> GetUserAsync(string firebaseId, string email)
    {
        var user = await _db.Users
            .Include(u => u.Character)
            .FirstOrDefaultAsync(u => u.UserId == firebaseId);

        if (user == null)
        {
            user = new User
            {
                UserId = firebaseId,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        return user.UserId;
    }
}
