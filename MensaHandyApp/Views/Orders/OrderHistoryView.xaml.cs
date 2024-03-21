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

        //Calling the Method ReloadData() every time the ORderHistoryView is opened   
        await ((OrderHistoryViewModel)BindingContext).ReloadData();
    }
}