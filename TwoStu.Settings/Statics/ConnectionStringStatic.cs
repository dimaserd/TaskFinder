using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoStu.Settings.Enumerations;

namespace TwoStu.Settings.Statics
{
    /// <summary>
    /// Возвращает строку подключения для контекстов
    /// </summary>
    public static class ConnectionStringStatic
    {
        public static string ConnectionString
        {
            get
            {

                string result = string.Empty;
                ConnectionType conType = Settings.Connection;
                switch (conType)
                {
                    case ConnectionType.Local:
                        result = "LocalTestConnection";
                        break;

                    case ConnectionType.Production:
                        result = "DefaultConnection";
                        break;

                }
                return result;
            }
        }
    }
}
