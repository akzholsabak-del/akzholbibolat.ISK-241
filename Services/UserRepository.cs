using ViewModelValidation.Models;

namespace ViewModelValidation.Services;

/// <summary>Жадтағы пайдаланушылар репозиторийі.</summary>
public class UserRepository
{
    private readonly object _lock = new();
    private readonly List<AppUser> _users = new();
    private int _nextId = 1;

    public bool UserNameExists(string userName)
    {
        lock (_lock)
        {
            return _users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public bool EmailExists(string email)
    {
        lock (_lock)
        {
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }

    public AppUser Add(AppUser user)
    {
        lock (_lock)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return user;
        }
    }

    public AppUser? GetById(int id)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}
