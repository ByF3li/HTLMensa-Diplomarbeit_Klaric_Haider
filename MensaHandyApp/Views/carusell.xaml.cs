using MensaAppKlassenBibliothek;
using MensaHandyApp.ViewModels;

namespace MensaHandyApp.Views;

public partial class Carusell : ContentPage
{
    private CarusellViewModel _vm = new CarusellViewModel();
    public Carusell()
	{
        this.BindingContext = this._vm;

        InitializeComponent();
    }

}