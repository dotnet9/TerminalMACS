using Contacts;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;
using TerminalMACS.Clients.App.Services;

namespace TerminalMACS.Clients.App.iOS.Services
{
    /// <summary>
    /// 通讯录获取服务
    /// </summary>
    public class ContactsService : NSObject, IContactsService
    {
        const string ThumbnailPrefix = "thumb";

        bool requestStop = false;

        public event EventHandler<ContactEventArgs> OnContactLoaded;

        bool _isLoading = false;
        public bool IsLoading => _isLoading;

        /// <summary>
        /// 异步请求权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RequestPermissionAsync()
        {
            var status = CNContactStore.GetAuthorizationStatus(CNEntityType.Contacts);

            Tuple<bool, NSError> authotization = new Tuple<bool, NSError>(status == CNAuthorizationStatus.Authorized, null);

            if (status == CNAuthorizationStatus.NotDetermined)
            {
                using (var store = new CNContactStore())
                {
                    authotization = await store.RequestAccessAsync(CNEntityType.Contacts);
                }
            }
            return authotization.Item1;

        }

        /// <summary>
        /// 异步请求通讯录，此方法由界面真正调用
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? cancelToken = null)
        {
            requestStop = false;

            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;

            // 我们创建了一个十进制的TaskCompletionSource
            var taskCompletionSource = new TaskCompletionSource<IList<Contact>>();

            // 在cancellationToken中注册lambda
            cancelToken.Value.Register(() =>
            {
                // 我们收到一条取消消息，取消TaskCompletionSource.Task
                requestStop = true;
                taskCompletionSource.TrySetCanceled();
            });

            _isLoading = true;

            var task = LoadContactsAsync();

            // 等待两个任务中的第一个任务完成
            var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);
            _isLoading = false;

            return await completedTask;

        }

        /// <summary>
        /// 异步加载通讯录，具体的通讯录读取方法
        /// </summary>
        /// <returns></returns>
        async Task<IList<Contact>> LoadContactsAsync()
        {
            IList<Contact> contacts = new List<Contact>();
            var hasPermission = await RequestPermissionAsync();
            if (hasPermission)
            {

                NSError error = null;
                var keysToFetch = new[] { CNContactKey.PhoneNumbers, CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses, CNContactKey.ImageDataAvailable, CNContactKey.ThumbnailImageData };

                var request = new CNContactFetchRequest(keysToFetch: keysToFetch);
                request.SortOrder = CNContactSortOrder.GivenName;

                using (var store = new CNContactStore())
                {
                    var result = store.EnumerateContacts(request, out error, new CNContactStoreListContactsHandler((CNContact c, ref bool stop) =>
                    {

                        string path = null;
                        if (c.ImageDataAvailable)
                        {
                            path = path = Path.Combine(Path.GetTempPath(), $"{ThumbnailPrefix}-{Guid.NewGuid()}");

                            if (!File.Exists(path))
                            {
                                var imageData = c.ThumbnailImageData;
                                imageData?.Save(path, true);


                            }
                        }

                        var contact = new Contact()
                        {
                            Name = string.IsNullOrEmpty(c.FamilyName) ? c.GivenName : $"{c.GivenName} {c.FamilyName}",
                            Image = path,
                            PhoneNumbers = c.PhoneNumbers?.Select(p => p?.Value?.StringValue).ToArray(),
                            Emails = c.EmailAddresses?.Select(p => p?.Value?.ToString()).ToArray(),

                        };

                        if (!string.IsNullOrWhiteSpace(contact.Name))
                        {
                            OnContactLoaded?.Invoke(this, new ContactEventArgs(contact));

                            contacts.Add(contact);
                        }

                        stop = requestStop;

                    }));
                }
            }

            return contacts;
        }


    }
}
