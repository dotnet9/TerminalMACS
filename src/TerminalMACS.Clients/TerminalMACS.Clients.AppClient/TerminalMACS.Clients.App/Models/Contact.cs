namespace TerminalMACS.Clients.App.Models
{
    /// <summary>
    /// Contact information entity.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the image
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Gets or sets the emails
        /// </summary>
        public string[] Emails { get; set; }
        /// <summary>
        /// Gets or sets the phone numbers
        /// </summary>
        public string[] PhoneNumbers { get; set; }
    }
}
