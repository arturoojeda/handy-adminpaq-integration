using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HandySyncService
{
    public static class ConfigProxy
    {
        static System.Configuration.Configuration config;

        public static void OpenConfigFile()
        {
            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string configFile = System.IO.Path.Combine(appPath, appName + ".exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }
        
        public static string HandyUser
        {
            get
            {
                return config.AppSettings.Settings["HandyUser"] != null ?
                    config.AppSettings.Settings["HandyUser"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["HandyUser"].Value = value;
                config.Save();
            }
        }

        public static string HandyPassword
        {
            get
            {
                return config.AppSettings.Settings["HandyPass"] != null ?
                    config.AppSettings.Settings["HandyPass"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["HandyPass"].Value = value;
                config.Save();
            }
        }

        public static string PriceId
        {
            get
            {
                return config.AppSettings.Settings["PriceId"] != null ?
                    config.AppSettings.Settings["PriceId"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["PriceId"].Value = value;
                config.Save();
            }
        }

        public static string HandyURL
        {
            get
            {
                return config.AppSettings.Settings["HandyURL"] != null ?
                    config.AppSettings.Settings["HandyURL"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["HandyURL"].Value = value;
                config.Save();
            }
        }
        public static string OrderConceptCode
        {
            get
            {
                return config.AppSettings.Settings["OrderConceptCode"] != null ?
                    config.AppSettings.Settings["OrderConceptCode"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["OrderConceptCode"].Value = value;
                config.Save();
            }
        }
        
        public static string WarehouseCode
        {
            get
            {
                return config.AppSettings.Settings["WarehouseCode"] != null ?
                    config.AppSettings.Settings["WarehouseCode"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["WarehouseCode"].Value = value;
                config.Save();
            }
        }

        public static string AdminPaqPath
        {
            get
            {
                return config.AppSettings.Settings["AdminPAQPath"] != null ?
                    config.AppSettings.Settings["AdminPAQPath"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["AdminPAQPath"].Value = value;
                config.Save();
            }
        }

        public static string AdminPaqCompanyPath 
        { 
            get
            {
                return config.AppSettings.Settings["AdminPAQCompanyPath"] != null ?
                    config.AppSettings.Settings["AdminPAQCompanyPath"].Value : string.Empty;
            }
            set
            {
                config.AppSettings.Settings["AdminPAQCompanyPath"].Value = value;
                config.Save();
            }
        }

        public static string LastSync
        {
            get
            {
                return config.AppSettings.Settings["LastSync"] != null ?
                    config.AppSettings.Settings["LastSync"].Value : string.Empty;
            }
            set
            {
                
                config.AppSettings.Settings["LastSync"].Value = value;
                config.Save(); 
            }
        }
        
        /// <summary>
        /// Indicates if it is the First Sync between Handy and AdminPaq
        /// </summary>
        public static bool FirstSync
        {
            get
            {
                if (config.AppSettings.Settings["FirstSync"] != null)
                    return config.AppSettings.Settings["FirstSync"].Value.ToUpper() == "TRUE" ?
                    true : false;
                else
                    return false;
            }
            set
            {
                config.AppSettings.Settings["FirstSync"].Value = value ?
                    "True" : "False";
                config.Save();
            }
        }

        public static bool SyncProducts
        {
            get
            {
                if (config.AppSettings.Settings["SyncProducts"] != null)
                    return config.AppSettings.Settings["SyncProducts"].Value.ToUpper() == "TRUE" ?
                    true : false;
                else
                    return false;
            }
            set
            {
                config.AppSettings.Settings["SyncProducts"].Value = value ?
                    "True" : "False";
                config.Save();
            }
        }

        public static bool SyncClients
        {
            get
            {
                if (config.AppSettings.Settings["SyncClients"] != null)
                    return config.AppSettings.Settings["SyncClients"].Value.ToUpper() == "TRUE" ?
                    true : false;
                else
                    return false;
            }
            set
            {
                config.AppSettings.Settings["SyncClients"].Value = value ?
                    "True" : "False";
                config.Save();
            }
        }

        public static bool SyncOrders
        {
            get
            {
                if (config.AppSettings.Settings["SyncOrders"] != null)
                    return config.AppSettings.Settings["SyncOrders"].Value.ToUpper() == "TRUE" ?
                    true : false;
                else
                    return false;
            }
            set
            {
                config.AppSettings.Settings["SyncOrders"].Value = value ?
                    "True" : "False";
                config.Save();
            }
        }

        public static int SyncPeriodMinutes
        {
            get 
            {
                return Convert.ToInt32(config.AppSettings.Settings["SyncPeriodMinutes"].Value);
            }
            set 
            {
                config.AppSettings.Settings["SyncPeriodMinutes"].Value = value.ToString();
                config.Save();
            }
        }
    }
}
