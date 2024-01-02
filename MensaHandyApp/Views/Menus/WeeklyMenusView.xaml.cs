using MensaAppKlassenBibliothek;
using MensaHandyApp.ViewModels;
using System.Globalization;

namespace MensaHandyApp.Views.Menus;

public partial class WeeklyMenusView : ContentPage
{
    private WeeklyMenusViewModel _vm = new WeeklyMenusViewModel();
    public WeeklyMenusView()
	{
        this.BindingContext = this._vm;

        InitializeComponent();
    }

}