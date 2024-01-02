using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views.Orders;

public partial class OrderHistoryView : ContentPage
{
    private OrderHistoryViewModel _vm = new OrderHistoryViewModel();

    public OrderHistoryView()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ((OrderHistoryViewModel)BindingContext).ReloadData();
    }
}