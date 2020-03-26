using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Castle.Core.Internal;
using Plugin.CurrentActivity;
using TerminalMACS.Clients.App.Models;
using TerminalMACS.Clients.App.Services;

namespace TerminalMACS.Clients.App.Droid.Services
{
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

        public const int RequestContacts = 1239;
        static string[] PermissionsContact = {
            Manifest.Permission.ReadContacts
        };

        public event EventHandler<ContactEventArgs> OnContactLoaded;

        async void RequestContactsPermissions()
        {
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
        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == ContactsService.RequestContacts)
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
        async Task<IList<Contact>> LoadContactsAsync()
        {
            IList<Contact> contacts = new List<Contact>();
            var hasPermission = await RequestPermissionAsync();
            if (hasPermission)
            {
                var uri = ContactsContract.Contacts.ContentUri;
                var ctx = Application.Context;
                await Task.Run(() =>
                {
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
                                OnContactLoaded?.Invoke(this, new ContactEventArgs(contact));
                                contacts.Add(contact);
                            }

                            if (stopLoad)
                                break;
                        }
                    }
                });

            }
            return contacts;

        }

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