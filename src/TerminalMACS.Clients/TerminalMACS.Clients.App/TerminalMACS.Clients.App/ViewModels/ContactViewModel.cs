using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TerminalMACS.Clients.App.Models;
using TerminalMACS.Clients.App.Resx;
using TerminalMACS.Clients.App.Services;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App.ViewModels
{
    /// <summary>
    /// Contact page ViewModel
    /// </summary>
    public class ContactViewModel : BaseViewModel
    {
        /// <summary>
        /// Contact service interface
        /// </summary>
        IContactsService _contactService;
        private string _SearchText;
        /// <summary>
        /// Gets or sets the search text of the contact list.
        /// </summary>
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                SetProperty(ref _SearchText, value);
            }
        }
        /// <summary>
        /// The search contact command.
        /// </summary>
        public ICommand RaiseSearchCommand { get; }
        /// <summary>
        /// The contact list.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }
        private List<Contact> _FilteredContacts;
        /// <summary>
        /// Contact filter list.
        /// </summary>
        public List<Contact> FilteredContacts

        {
            get { return _FilteredContacts; }
            set
            {
                SetProperty(ref _FilteredContacts, value);
            }
        }
        public ContactViewModel()
        {
            _contactService = DependencyService.Get<IContactsService>();
            Contacts = new ObservableCollection<Contact>();
            Xamarin.Forms.BindingBase.EnableCollectionSynchronization(Contacts, null, ObservableCollectionCallback);
            _contactService.OnContactLoaded += OnContactLoaded;
            LoadContacts();
            RaiseSearchCommand = new Command(RaiseSearchHandle);
        }

        /// <summary>
        /// Filter contact list
        /// </summary>
        void RaiseSearchHandle()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredContacts = Contacts.ToList();
                return;
            }

            Func<Contact, bool> checkContact = (s) =>
            {
                if (!string.IsNullOrWhiteSpace(s.Name) && s.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    return true;
                }
                else if (s.PhoneNumbers.Length > 0 && s.PhoneNumbers.ToList().Exists(cu => cu.ToString().Contains(SearchText)))
                {
                    return true;
                }
                return false;
            };
            FilteredContacts = Contacts.ToList().Where(checkContact).ToList();
        }

        /// <summary>
        /// BindingBase.EnableCollectionSynchronization
        ///     Enable cross thread updates for collections
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="context"></param>
        /// <param name="accessMethod"></param>
        /// <param name="writeAccess"></param>
        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            // `lock` ensures that only one thread access the collection at a time
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        /// <summary>
        /// Received a event notification that a contact information was successfully read.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContactLoaded(object sender, ContactEventArgs e)
        {
            Contacts.Add(e.Contact);
            RaiseSearchHandle();
        }

        /// <summary>
        /// Read contact information asynchronously
        /// </summary>
        /// <returns></returns>
        async Task LoadContacts()
        {
            try
            {
                await _contactService.RetrieveContactsAsync();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine(AppResource.TaskCancelled);
            }
        }
    }
}
