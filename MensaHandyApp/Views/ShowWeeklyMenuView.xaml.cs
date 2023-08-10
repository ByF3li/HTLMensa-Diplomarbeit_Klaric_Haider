using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;

public partial class ShowWeeklyMenuView : ContentPage
{
    private ShowWeeklyMenuViewModel _vm = new ShowWeeklyMenuViewModel();
    public ShowWeeklyMenuView()
	{
        this.BindingContext = this._vm;
        InitializeComponent();
	}
}