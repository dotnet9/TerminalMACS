namespace TerminalMACS.Clients.App.Models
{
    /// <summary>
    /// 主界面左上角的抽屉式菜单
    /// </summary>
    public enum MenuItemType
    {
        Contacts,       //联系人
        About           //关于
    }
    public class HomeMenuItem
    {
        /// <summary>
        /// 获取或者设置 菜单ID
        /// </summary>
        public MenuItemType Id { get; set; }

        /// <summary>
        /// 获取或者设置 菜单名称
        /// </summary>

        public string Title { get; set; }
    }
}
