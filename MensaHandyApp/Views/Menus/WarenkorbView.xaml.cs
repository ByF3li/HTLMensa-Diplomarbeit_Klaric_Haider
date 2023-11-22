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
}