using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;
using TerminalMACS.Clients.App.Services;

[assembly: Xamarin.Forms.Dependency(typeof(TerminalMACS.Clients.App.Droid.Services.ContactsService))]
namespace TerminalMACS.Clients.App.Droid.Services
{
    /// <summary>
    /// Contact service.
    /// </summary>
    public class ContactsService : IContactsService
    {
        const string ThumbnailPrefix = "thumb";
        bool stopLoad = false;
        static TaskCompletionSource<bool> contactPermissionTcs;
        public string TAG
        {
            get
            {
                return "MainActivity";
            }
        }
        bool _isLoading = false;
        public bool IsLoading => _isLoading;
        // Request contact permission status code
        public const int RequestContacts = 1239;
        /// <summary>
        /// Get the request permission required by the contact
        /// </summary>
        static string[] PermissionsContact = {
            Manifest.Permission.ReadContacts
        };

        public event EventHandler<ContactEventArgs> OnContactLoaded;
        /// <summary>
        /// Provide an additional rationale to the user if the permission was not granted
        //  and the user would benefit from additional context for the use of the permission.
        //  For example, if the request has been denied previously.
        /// </summary>
        async void RequestContactsPermissions()
        {
            // Check whether you can pop up to apply for permission to read and write address book
            if (ActivityCompat.ShouldShowRequestPermissionRationale(CrossCurrentActivity.Current.Activity, Manifest.Permission.ReadContacts)
                || ActivityCompat.ShouldShowRequestPermissionRationale(CrossCurrentActivity.Current.Activity, Manifest.Permission.WriteContacts))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example, if the request has been denied previously.
                await UserDialogs.Instance.AlertAsync("Contacts Permission", "This action requires contacts permission", "Ok");
            }
            else
            {
                // Contact permissions have not been granted yet. Request them directly.
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, PermissionsContact, RequestContacts);
            }
        }

        /// <summary>
        /// Results after receiving the user response request permission Operation
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestContacts)
            {
                // We have requested multiple permissions for contacts, so all of them need to be
                // checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display contacts fragment.
                    contactPermissionTcs.TrySetResult(true);
                }
                else
                {
                    contactPermissionTcs.TrySetResult(false);
                }

            }
        }

        /// <summary>
        /// Asynchronous request permission
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RequestPermissionAsync()
        {
            contactPermissionTcs = new TaskCompletionSource<bool>();

            // Verify that all required contact permissions have been granted.
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.ReadContacts) != (int)Permission.Granted
                || Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.WriteContacts) != (int)Permission.Granted)
            {
                // Contacts permissions have not been granted.
                RequestContactsPermissions();
            }
            else
            {
                // Contact permissions have been granted. 
                contactPermissionTcs.TrySetResult(true);
            }

            return await contactPermissionTcs.Task;
        }

        /// <summary>
        /// Request contact asynchronously. This method is called by the interface.
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? cancelToken = null)
        {
            stopLoad = false;

            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;

            // We create a TaskCompletionSource of decimal
            var taskCompletionSource = new TaskCompletionSource<IList<Contact>>();

            // Registering a lambda into the cancellationToken
            cancelToken.Value.Register(() =>
            {
                // We received a cancellation message, cancel the TaskCompletionSource.Task
                stopLoad = true;
                taskCompletionSource.TrySetCanceled();
            });

            _isLoading = true;

            var task = LoadContactsAsync();

            // Wait for the first task to finish among the two
            var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);
            _isLoading = false;

            return await completedTask;
        }

        /// <summary>
        /// Load contacts asynchronously, fact reading method of address book.
        /// </summary>
        /// <returns></returns>
        async Task<IList<Contact>> LoadContactsAsync()
        {
            IList<Contact> contacts = new List<Contact>();
            var hasPermission = await RequestPermissionAsync();
            if (!hasPermission)
            {
                return contacts;
            }

            var uri = ContactsContract.Contacts.ContentUri;
            var ctx = Application.Context;
            await Task.Run(() =>
            {
                // Only Contact ID, DisplayName and PhotoThumbnailUri are requested temporarily, which can be extended
                var cursor = ctx.ApplicationContext.ContentResolver.Query(uri, new string[]
                {
                        ContactsContract.Contacts.InterfaceConsts.Id,
                        ContactsContract.Contacts.InterfaceConsts.DisplayName,
                        ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri
                }, null, null, $"{ContactsContract.Contacts.InterfaceConsts.DisplayName} ASC");
                if (cursor.Count > 0)
                {
                    while (cursor.MoveToNext())
                    {
                        var contact = CreateContact(cursor, ctx);

                        if (!string.IsNullOrWhiteSpace(contact.Name))
                        {
                            // Read out a contact record, notify the shared library.
                            OnContactLoaded?.Invoke(this, new ContactEventArgs(contact));
                            contacts.Add(contact);
                        }

                        if (stopLoad)
                            break;
                    }
                }
            });

            return contacts;

        }

        /// <summary>
        /// Read a contact record
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        Contact CreateContact(ICursor cursor, Context ctx)
        {
            var contactId = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.Id);

            var numbers = GetNumbers(ctx, contactId);
            var emails = GetEmails(ctx, contactId);

            var uri = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri);
            string path = null;
            if (!string.IsNullOrEmpty(uri))
            {
                try
                {
                    using (var stream = Android.App.Application.Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(uri)))
                    {
                        path = Path.Combine(Path.GetTempPath(), $"{ThumbnailPrefix}-{Guid.NewGuid()}");
                        using (var fstream = new FileStream(path, FileMode.Create))
                        {
                            stream.CopyTo(fstream);
                            fstream.Close();
                        }

                        stream.Close();
                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

            }
            var contact = new Contact
            {
                Name = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.DisplayName),
                Emails = emails,
                Image = path,
                PhoneNumbers = numbers,
            };

            return contact;
        }

        /// <summary>
        /// Get contact numbers
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        string[] GetNumbers(Context ctx, string contactId)
        {
            var key = ContactsContract.CommonDataKinds.Phone.Number;

            var cursor = ctx.ApplicationContext.ContentResolver.Query(
                ContactsContract.CommonDataKinds.Phone.ContentUri,
                null,
                ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                new[] { contactId },
                null
            );

            return ReadCursorItems(cursor, key)?.ToArray();
        }

        /// <summary>
        /// Get cotnact emails
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        string[] GetEmails(Context ctx, string contactId)
        {
            var key = ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data;

            var cursor = ctx.ApplicationContext.ContentResolver.Query(
                ContactsContract.CommonDataKinds.Email.ContentUri,
                null,
                ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = ?",
                new[] { contactId },
                null);

            return ReadCursorItems(cursor, key)?.ToArray();
        }

        IEnumerable<string> ReadCursorItems(ICursor cursor, string key)
        {
            while (cursor.MoveToNext())
            {
                var value = GetString(cursor, key);
                yield return value;
            }

            cursor.Close();
        }

        string GetString(ICursor cursor, string key)
        {
            return cursor.GetString(cursor.GetColumnIndex(key));
        }

    }
}