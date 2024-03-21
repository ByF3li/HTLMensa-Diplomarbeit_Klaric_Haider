using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views.Login;

public partial class LoginView : ContentPage
{
    private LoginViewModel _vm = new LoginViewModel();

    public LoginView()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}
}