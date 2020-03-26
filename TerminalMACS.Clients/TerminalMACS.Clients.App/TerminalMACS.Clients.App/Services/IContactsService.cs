using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;

namespace TerminalMACS.Clients.App.Services
{
    public class ContactEventArgs:EventArgs
    {
        public Contact Contact { get; }
        public ContactEventArgs(Contact contact)
        {
            Contact = contact;
        }
    }
    public interface IContactsService
    {
        event EventHandler<ContactEventArgs> OnContactLoaded;
        bool IsLoading { get; }
        Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? token = null);
    }
}
