namespace TerminalMACS.Clients.App.Models
{
    /// <summary>
    /// Drawer menu in the upper left corner of the main page
    /// </summary>
    public enum MenuItemType
    {
        ClientInfo,       // Basic information of mobile phone
        Contacts,         // Contact information
        About             // About
    }
    /// <summary>
    /// Drawer menu item
    /// </summary>
    public class HomeMenuItem
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public MenuItemType Id { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>

        public string Title { get; set; }
    }
}
