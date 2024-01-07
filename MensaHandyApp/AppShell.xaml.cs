using MensaHandyApp.ViewModels;
using MensaHandyApp.Views;

namespace MensaHandyApp
{
    public partial class AppShell : Shell
    {
        private AppShellViewModel _vm = new AppShellViewModel();
       
        public AppShell()
        {
            this.BindingContext = this._vm;

            InitializeComponent();
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            
            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccess", (sender) =>
            {
                ShowLoggedInViews();
            });

            MessagingCenter.Subscribe<MainPage>(this, "LogoutSuccess", (sender) =>
            {
                ShowLoggedOutViews();
            });
        }

        private void ShowLoggedInViews()
        {
            _vm.IsHomepageVisible = true;
            _vm.IsWeeklyMenusVisible = true;
            _vm.IsWarenkorbVisible = true;
            _vm.IsOrderHistoryVisible = true;

            _vm.IsLoginVisible = false;
        }

        private void ShowLoggedOutViews()
        {
            _vm.IsHomepageVisible = true;
            _vm.IsLoginVisible = true;

            _vm.IsWeeklyMenusVisible = false;
            _vm.IsWarenkorbVisible = false;
            _vm.IsOrderHistoryVisible = false;
        }
    }
}