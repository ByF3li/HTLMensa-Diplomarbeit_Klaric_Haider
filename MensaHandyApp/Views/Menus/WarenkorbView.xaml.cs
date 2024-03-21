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
        //Calling the Method ReloadData() every time the WarenkorbView is opened   
        ((WarenkorbViewModel)BindingContext).ReloadData();
    }
}