using KickBlastStudentUI.Data;

namespace KickBlastStudentUI.Services;

public class AuthService
{
    public bool Login(string username, string password)
    {
        using var db = new AppDbContext();
        return db.Users.Any(u => u.Username == username && u.PasswordPlain == password);
    }
}
