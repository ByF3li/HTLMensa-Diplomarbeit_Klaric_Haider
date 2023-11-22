using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views.Orders;

public partial class OrderHistory : ContentPage
{
    private OrderHistoryViewModel _vm = new OrderHistoryViewModel();

    public OrderHistory()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}
}