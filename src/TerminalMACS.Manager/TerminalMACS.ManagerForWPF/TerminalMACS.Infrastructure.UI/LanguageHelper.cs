using System;
using System.Linq;
using System.Windows;

namespace TerminalMACS.Infrastructure.UI
{
    public class LanguageHelper
    {
        private const string KEY_OF_LANGUAGE = "language";
        public static void SetLanguage(string language = "")
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = ConfigHelper.ReadKey(KEY_OF_LANGUAGE);
                if (string.IsNullOrWhiteSpace(language))
                {
                    language = System.Globalization.CultureInfo.CurrentCulture.ToString();
                }
            }

            string languagePath = $@"I18nResources\{language}.xaml";
            try
            {
                var lanRd = Application.LoadComponent(new Uri(languagePath, UriKind.Relative)) as ResourceDictionary;
                var old = Application.Current.Resources.MergedDictionaries.FirstOrDefault(o => o.Contains("AppTitle"));
                if (old != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(old);
                }
                Application.Current.Resources.MergedDictionaries.Add(lanRd);
                ConfigHelper.SetKey(KEY_OF_LANGUAGE, language);

                var culture = new System.Globalization.CultureInfo(language);
                System.Globalization.CultureInfo.CurrentCulture = culture;
                System.Globalization.CultureInfo.CurrentUICulture = culture;
            }
            catch { }
        }
    }
}
