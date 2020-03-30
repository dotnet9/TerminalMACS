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

namespace TerminalMACS.Clients.App.Droid.Services
{
    /// <summary>
    /// 通讯录获取服务
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
        //权限请求状态码
        public const int RequestContacts = 1239;
        /// <summary>
        /// 获取通讯录需要的请求权限
        /// </summary>
        static string[] PermissionsContact = {
            Manifest.Permission.ReadContacts
        };

        public event EventHandler<ContactEventArgs> OnContactLoaded;
        /// <summary>
        /// 异步请求通讯录权限
        /// </summary>
        async void RequestContactsPermissions()
        {
            //检查是否可以弹出申请读、写通讯录权限
            if (ActivityCompat.ShouldShowRequestPermissionRationale(CrossCurrentActivity.Current.Activity, Manifest.Permission.ReadContacts)
                || ActivityCompat.ShouldShowRequestPermissionRationale(CrossCurrentActivity.Current.Activity, Manifest.Permission.WriteContacts))
            {
                // 如果未授予许可，请向用户提供其他理由用户将从使用权限的附加上下文中受益。
                // 例如，如果请求先前被拒绝。
                await UserDialogs.Instance.AlertAsync("通讯录权限", "此操作需要“通讯录”权限", "确定");
            }
            else
            {
                // 尚未授予通讯录权限。直接请求这些权限。
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, PermissionsContact, RequestContacts);
            }
        }

        /// <summary>
        /// 收到用户响应请求权限操作后的结果
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestContacts)
            {
                // 我们请求了多个通讯录权限，因此需要检查相关的所有权限
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // 已授予所有必需的权限，显示联系人片段。
                    contactPermissionTcs.TrySetResult(true);
                }
                else
                {
                    contactPermissionTcs.TrySetResult(false);
                }

            }
        }

        /// <summary>
        /// 异步请求权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RequestPermissionAsync()
        {
            contactPermissionTcs = new TaskCompletionSource<bool>();

            // 验证是否已授予所有必需的通讯录权限。
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.ReadContacts) != (int)Permission.Granted
                || Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.WriteContacts) != (int)Permission.Granted)
            {
                // 尚未授予通讯录权限。
                RequestContactsPermissions();
            }
            else
            {
                // 已授予通讯录权限。
                contactPermissionTcs.TrySetResult(true);
            }

            return await contactPermissionTcs.Task;
        }

        /// <summary>
        /// 异步请求通讯录，此方法由界面真正调用
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? cancelToken = null)
        {
            stopLoad = false;

            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;

            // 我们创建了一个十进制的TaskCompletionSource
            var taskCompletionSource = new TaskCompletionSource<IList<Contact>>();

            // 在cancellationToken中注册lambda
            cancelToken.Value.Register(() =>
            {
                // 我们收到一条取消消息，取消TaskCompletionSource.Task
                stopLoad = true;
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
            if (!hasPermission)
            {
                return contacts;
            }

            var uri = ContactsContract.Contacts.ContentUri;
            var ctx = Application.Context;
            await Task.Run(() =>
            {
                // 暂时只请求通讯录Id、DisplayName、PhotoThumbnailUri，可以扩展
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
                            // 读取出一条，即通知界面展示
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
        /// 读取一条通讯录数据
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
        /// 读取联系人电话号码
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
        /// 读取联系人邮箱地址
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