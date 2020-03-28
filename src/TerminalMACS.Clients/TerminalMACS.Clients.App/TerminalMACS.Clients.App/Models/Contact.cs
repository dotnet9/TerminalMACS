using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalMACS.Clients.App.Models
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// 获取或者设置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或者设置头像
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 获取或者设置邮箱地址
        /// </summary>
        public string[] Emails { get; set; }
        /// <summary>
        /// 获取或者设置手机号码
        /// </summary>
        public string[] PhoneNumbers { get; set; }
    }
}
