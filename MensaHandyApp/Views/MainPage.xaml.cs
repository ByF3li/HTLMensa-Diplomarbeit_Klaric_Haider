using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm = new MainPageViewModel();

    public MainPage()
	{
        this.BindingContext = this._vm;
		InitializeComponent();
	}
}