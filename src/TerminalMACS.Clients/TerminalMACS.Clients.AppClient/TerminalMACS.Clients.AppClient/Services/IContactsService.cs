using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;

namespace TerminalMACS.Clients.App.Services
{
    /// <summary>
    /// Read a contact record notification event parameter.
    /// </summary>
    public class ContactEventArgs:EventArgs
    {
        public Contact Contact { get; }
        public ContactEventArgs(Contact contact)
        {
            Contact = contact;
        }
    }

    /// <summary>
    /// Contact service interface, which is required for Android and iOS terminal specific 
    ///  contact acquisition service needs to implement this interface.
    /// </summary>
    public interface IContactsService
    {
        /// <summary>
        /// Read a contact record and notify the shared library through this event.
        /// </summary>
        event EventHandler<ContactEventArgs> OnContactLoaded;
        /// <summary>
        /// Loading or not
        /// </summary>
        bool IsLoading { get; }
        /// <summary>
        /// Try to get all contact information
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? token = null);
    }
}
