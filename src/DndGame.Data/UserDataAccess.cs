
using DndGame.Data;
using DndGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace DndGame.Domain;

public class UserDataAccess
{

    private readonly DndGameContext _db;

  
    public UserDataAccess( DndGameContext db)
    {
        _db = db;
    }


    public async Task<User> GetUserAsync(string firebaseId, string email)
    {
        var user = await _db.Users.Include(u => u.Character)
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
        var needsCharacterCreation = string.IsNullOrWhiteSpace(user.DisplayName) || user.Character == null;
        return user;
    }

}
