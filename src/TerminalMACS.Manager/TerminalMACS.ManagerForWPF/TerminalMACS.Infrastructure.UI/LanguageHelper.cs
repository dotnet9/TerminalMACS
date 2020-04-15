using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TerminalMACS.Infrastructure.UI
{
    public class LanguageHelper
    {
        public static void SetLanguage(string language = "")
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = ConfigHelper.ReadKey("language");
                if (string.IsNullOrWhiteSpace(language))
                {
                    language = System.Globalization.CultureInfo.CurrentCulture.ToString();
                }
            }

            string languagePath = $@"I18nResources\{language}.xaml";
            var lanRd = Application.LoadComponent(new Uri(languagePath, UriKind.Relative)) as ResourceDictionary;
            var old = Application.Current.Resources.MergedDictionaries.FirstOrDefault(o => o.Contains("AppTitle"));
            if (old != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(old);
            }
            Application.Current.Resources.MergedDictionaries.Add(lanRd);
        }
    }
}
