using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;

public partial class Orders : ContentPage
{
    private CarusellViewModel _vm = new CarusellViewModel();

    public Orders()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}
}