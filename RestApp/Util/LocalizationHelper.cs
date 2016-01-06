using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RestApp.Properties;
using WPFLocalizeExtension.Extensions;

namespace RestApp.Util
{
    public class LocalizationHelper
    {
        public static T Get<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":Resources:" + key);
        }
        public static string GetString(string key)
        {
            return LocExtension.GetLocalizedValue<string>(Assembly.GetCallingAssembly().GetName().Name + ":Resources:" + key);
        }
    }
}
