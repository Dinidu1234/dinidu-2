using System.Windows.Controls;

namespace KickBlastStudentUI.Services;

public class NavigationService
{
    public UserControl CurrentView { get; private set; } = new UserControl();

    public void Navigate(UserControl view)
    {
        CurrentView = view;
    }
}
