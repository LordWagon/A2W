using SQLite;
using System.Runtime.CompilerServices;

namespace LoginManager.Shared
{
    public class Setting
    {
        [PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }

        public const string EMTPY_MAIN_PASSWORD = "Abrakadabra42";
        public static string dbPath = PlatformSpecifics.GetPathToDb();
        public static string dateFormat;
        public static string colorScheme;
        public static string mainPassword;

        public Setting()
        { }

        public Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }


        public static async Task<bool> RequestStoragePermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        return status == PermissionStatus.Granted;
    }


    public static void Create(Setting setting)
        {
            if (!File.Exists(dbPath))
                throw new Exception("Database file not found");

            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            try
            {
                SQLiteConnection db = new SQLiteConnection(connStr);
                try
                {
                    if (setting.Key == null)
                        throw new Exception("Key is null");
                    if (setting.Value == null)
                        setting.Value = "";
                    if (setting.Key == "MainPassword")
                    {
                        if (setting.Value.Trim().Length == 0)
                            setting.Value = EMTPY_MAIN_PASSWORD;
                        else
                            setting.Value = Crypto.Encrypt(setting.Value);
                    }
                    Delete(setting.Key);
                    db.Insert(setting);
                    switch (setting.Key) 
                    {
                        case "DateFormat":
                            dateFormat = setting.Value;
                            break;
                        case "ColorScheme":
                            colorScheme = setting.Value;
                            break;
                        case "MainPassword":
                            mainPassword = Crypto.Decrypt(setting.Value);
                            break;
                    }
                }
                finally
                {
                    if (db != null)
                    {
                        db.Close();
                        db.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                string x = e.Message;
            }
        }

        public static void Delete(string key)
        {
            if (!File.Exists(dbPath))
                throw new Exception("Database file not found");

            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            try
            {
                SQLiteConnection db = new SQLiteConnection(connStr);
                try
                {
                    db.Delete<Setting>(key);
                }
                finally
                {
                    if (db != null)
                    {
                        db.Close();
                        db.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

        }

        public static string Get(string key)
        {
            bool hasPermission = RequestStoragePermissionAsync().Result;
            if (!hasPermission)
                return null;

            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);



            if (!File.Exists(dbPath))
                throw new Exception("Database file not found");
            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            try
            {
                SQLiteConnection db = new SQLiteConnection(connStr);
                try
                {

                    Setting result = db.Get<Setting>(key);
                    if (result.Key == "MainPassword")
                    {
                        if (result.Value == null || result.Value.Trim().Length == 0)
                        {
                            result.Value = null;
                        }
                        else if (result.Value == EMTPY_MAIN_PASSWORD)
                        {

                            result.Value = "";
                        }
                        else
                        {
                            result.Value = Crypto.Decrypt(result.Value);
                        }
                    }
                    return result.Value;
                }
                finally
                {
                    if (db != null)
                    {
                        db.Close();
                        db.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                string x = e.Message;
            }
            return null;
        }


        public static List<Setting> GetAll()
        {
            if (!File.Exists(dbPath))
                throw new Exception("Database file not found");

            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            try
            {
                SQLiteConnection db = new SQLiteConnection(connStr);
                try
                {
                    List<Setting> result = new List<Setting>();
                    List<Setting> beforeResult = db.Table<Setting>().ToList();

                    foreach (Setting setting in beforeResult)
                    {
                        string outResult = Get(setting.Key);
                        result.Add(setting);
                        if (setting.Key == "DateFormat")
                            Setting.dateFormat = outResult;
                        if (setting.Key == "ColorScheme")
                            Setting.colorScheme = outResult;
                        if (setting.Key == "MainPassword")
                            Setting.mainPassword = outResult;
                    }
                    return result;
                }
                finally
                {
                    if (db != null)
                    {
                        db.Close();
                        db.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                string x = e.Message;
            }
            return null;
        } 
    }
}