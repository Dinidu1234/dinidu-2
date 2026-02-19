using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Services;

namespace KickBlastStudentUI.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private string _username = "rashii";
    private string _password = "123456";
    private string _errorMessage = string.Empty;

    public event Action? LoginSucceeded;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new RelayCommand(_ => Login());
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public RelayCommand LoginCommand { get; }

    private void Login()
    {
        if (_authService.Login(Username.Trim(), Password))
        {
            ErrorMessage = string.Empty;
            LoginSucceeded?.Invoke();
            return;
        }

        ErrorMessage = "Invalid username or password.";
    }
}
