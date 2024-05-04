using SQLite;

namespace LoginManager.Shared
{
    public interface IDataStorageService
    {
        static SQLiteConnection db;
        static readonly string dbPath;
        void Update(Record record);
        void Create(Record record);
        void Delete(int id);
        Record Get(int id);
        IList<Record> GetAll();
        void CreateTable();
    }
}
