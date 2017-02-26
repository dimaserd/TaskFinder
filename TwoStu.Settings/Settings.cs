using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoStu.Settings.Enumerations;

namespace TwoStu.Settings
{
    public static class Settings
    {
        public static ConnectionType Connection = ConnectionType.Production;

        public static ApplicationSettingType AppSetting = ApplicationSettingType.Production;
    }
}
