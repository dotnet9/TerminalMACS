using Prism.Mvvm;

namespace TerminalMACS.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private string _title = "Login";
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    SetProperty(ref _title, value);
                }
            }
        }

        public LoginViewModel()
        {
            this.Title = App.Current.FindResource("AppTitle").ToString();
        }
    }
}
