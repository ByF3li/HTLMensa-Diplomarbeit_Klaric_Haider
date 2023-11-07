using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;

public partial class Orders : ContentPage
{
    private OrdersViewModel _vm = new OrdersViewModel();

    public Orders()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}
}