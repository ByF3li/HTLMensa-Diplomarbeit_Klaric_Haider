using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;


public partial class ShowDailyMenuView : ContentPage
{
    private ShowDailyMenuViewModel _vm = new ShowDailyMenuViewModel();
    public ShowDailyMenuView()
	{  
		this.BindingContext = this._vm;
		InitializeComponent();
	}

    
}