using KickBlastStudentUI.Helpers;

namespace KickBlastStudentUI.Services;

public class ToastService : ObservableObject
{
    private string _message = "Ready";

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public void Success(string text) => Message = $"✅ {text}";
    public void Error(string text) => Message = $"❌ {text}";
}
