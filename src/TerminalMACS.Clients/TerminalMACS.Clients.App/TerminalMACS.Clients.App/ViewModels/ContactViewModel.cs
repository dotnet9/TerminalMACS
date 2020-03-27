using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;
using TerminalMACS.Clients.App.Services;

namespace TerminalMACS.Clients.App.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        IContactsService _contactService;

        public new string Title => "通讯录";

        public string SearchText { get; set; }
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<Contact> FilteredContacts
        {
            get
            {
                return string.IsNullOrEmpty(SearchText) ? Contacts 
                                                        : new ObservableCollection<Contact>(Contacts?.ToList()
                                                        ?.Where(s => !string.IsNullOrEmpty(s.Name) && s.Name.ToLower().Contains(SearchText.ToLower())));
            }
        }
        public ContactViewModel(IContactsService contactService)
        {
            _contactService = contactService;
            Contacts = new ObservableCollection<Contact>();
            Xamarin.Forms.BindingBase.EnableCollectionSynchronization(Contacts, null, ObservableCollectionCallback);
            _contactService.OnContactLoaded += OnContactLoaded;
            LoadContacts();
        }


        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            // `lock` ensures that only one thread access the collection at a time
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        private void OnContactLoaded(object sender, ContactEventArgs e)
        {
            Contacts.Add(e.Contact);
        }
        async Task LoadContacts()
        {
            try
            {
                await _contactService.RetrieveContactsAsync();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("任务已经取消");
            }
        }
    }
}
