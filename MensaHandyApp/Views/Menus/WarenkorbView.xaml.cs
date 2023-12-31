using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views.Menus;

public partial class WarenkorbView : ContentPage
{
    private WarenkorbViewModel _vm = new WarenkorbViewModel();

    public WarenkorbView()
	{
        this.BindingContext = this._vm;

        InitializeComponent();	
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reload data or perform any other actions when the page appears
        // For example, you can call a method in your view model to refresh data
        ((WarenkorbViewModel)BindingContext).ReloadData();
    }
}