using SQLite;


namespace LoginManager.Shared
{
    class DataService : IDataStorageService
    {
        private static string dbPath = PlatformSpecifics.GetPathToDb();

        public DataService()
        {

        }

        public void Update(Record record)
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                record.DateOnlyCreated = DateOnly.FromDateTime(DateTime.Now);
                record.DateCreated = record.MakeDateForDB(record.DateOnlyCreated);
                record.DateToDisplay = record.MakeDateToDisplay(record.DateOnlyCreated);
                bool result = EncryptRecord(ref record);
                if (!result)
                    throw new Exception("Error during decrypting DB");

                db.Update(record);
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

        public void Create(Record record)
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                record.DateOnlyCreated = DateOnly.FromDateTime(DateTime.Now);
                record.DateCreated = record.MakeDateForDB(record.DateOnlyCreated);
                record.DateToDisplay = record.MakeDateToDisplay(record.DateOnlyCreated);
                bool result = EncryptRecord(ref record);
                if (!result)
                    throw new Exception("Error during decrypting DB");

                db.Insert(record);
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

        public void Delete(int id)
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                db.Delete<Record>(id);
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

        public Record Get(int id)
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                Record record = db.Get<Record>(id);
                bool result = DecryptRecord(ref record);
                if (!result)
                    throw new Exception("Error during decrypting DB");
                record.DateOnlyCreated = record.MakeDateFromDB(record.DateCreated);
                record.DateToDisplay = record.MakeDateToDisplay(record.DateCreated);
                return record;
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

        public IList<Record> GetAll()
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                bool result;
                List<Record> listOfRecords = new List<Record>();
                List<Record> records = db.Table<Record>().ToList();
                for (int i = 0; i < records.Count; i++)
                {
                    Record record = records[i];
                    result = DecryptRecord(ref record);
                    if (!result)
                        throw new Exception("Error during decrypting DB");

                    record.DateOnlyCreated = record.MakeDateFromDB(record.DateCreated);
                    record.DateToDisplay = record.MakeDateToDisplay(record.DateCreated);

                    listOfRecords.Add(record);
                    //if (i == 10)
                    //    break;
                }
                return listOfRecords;
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

        public void CreateTable()
        {
            using SQLiteConnection db = new SQLiteConnection(dbPath);
            try
            {
                db.CreateTable<Record>();
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

        private static bool EncryptRecord(ref Record record)
        {
            try
            {
                record.Password = Crypto.Encrypt(record.Password);
                record.Username = Crypto.Encrypt(record.Username);
                record.Location = Crypto.Encrypt(record.Location);
                record.Notes = Crypto.Encrypt(record.Notes);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool DecryptRecord(ref Record record)
        {
            try 
            {
                record.Password = Crypto.Decrypt(record.Password);
                record.Username = Crypto.Decrypt(record.Username);
                record.Location = Crypto.Decrypt(record.Location);
                record.Notes = Crypto.Decrypt(record.Notes);
                return true;
            }
            catch (Exception ex) 
            {
                return false; 
            }
        }
    }
}
