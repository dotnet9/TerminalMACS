using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;

namespace TerminalMACS.Clients.App.Services
{
    /// <summary>
    /// 通讯录事件参数
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
    /// 通讯录服务接口，android和iOS终端具体的通讯录获取服务需要继承此接口
    /// </summary>
    public interface IContactsService
    {
        /// <summary>
        /// 读取一条数据通知
        /// </summary>
        event EventHandler<ContactEventArgs> OnContactLoaded;
        /// <summary>
        /// 是否正在加载
        /// </summary>
        bool IsLoading { get; }
        /// <summary>
        /// 尝试获取所有通讯录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? token = null);
    }
}
