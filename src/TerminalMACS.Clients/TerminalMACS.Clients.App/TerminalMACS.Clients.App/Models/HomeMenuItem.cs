using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalMACS.Clients.App.Models
{
    public enum MenuItemType
    {
        Contacts,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
