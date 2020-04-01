using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using TerminalMACS.Clients.App.Droid.Services;
using TerminalMACS.Clients.App.Services;

namespace TerminalMACS.Clients.App.Droid
{
    [Activity(Label = "TerminalMACS.Clients.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        IContactsService contactsService = new ContactsService();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(() => this);

            // Send the contact service instance to the shared library, which uses the read contact interface
            LoadApplication(new App(contactsService));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            // The contact service processes the result of the permission request.
            ContactsService.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}