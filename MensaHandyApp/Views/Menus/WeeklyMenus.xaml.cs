using MensaAppKlassenBibliothek;
using MensaHandyApp.ViewModels;
using System.Globalization;

namespace MensaHandyApp.Views.Menus;

public partial class WeeklyMenus : ContentPage
{
    private WeeklyMenusViewModel _vm = new WeeklyMenusViewModel();
    public WeeklyMenus()
	{
        this.BindingContext = this._vm;

        InitializeComponent();
    }

}