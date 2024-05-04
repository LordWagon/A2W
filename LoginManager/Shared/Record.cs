using SQLite;

namespace LoginManager.Shared
{
    public class Record
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
        public string DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
        [Ignore]
        public DateOnly DateOnlyCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Ignore]
        public string DateToDisplay { get; set; }

        private readonly string dateFormat = Setting.dateFormat;

        
        public Record()
        {
        }

        public Record(string name, string location, string username, string password, string notes, DateOnly dateOnlyCreated)
        {
            Name = name;
            Location = location;
            Username = username;
            Password = password;
            Notes = notes;
            DateCreated = MakeDateForDB(dateOnlyCreated);
        }

        public string MakeDateForDB(DateOnly dateOnly) 
        { 
            DateCreated = dateOnly.Year + "-" + dateOnly.Month.ToString("00") + "-" + dateOnly.Day.ToString("00");
            return DateCreated;
        }

        public string MakeDateForDB()
        {
            DateCreated = DateOnlyCreated.Year + "-" + DateOnlyCreated.Month.ToString("00") + "-" + DateOnlyCreated.Day.ToString("00");
            return DateCreated;
        }

        public DateOnly MakeDateFromDB(string date)
        { 
            DateOnlyCreated = DateOnly.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return DateOnlyCreated;
        }

        public string MakeDateToDisplay(DateOnly dateOnly)
        {
            DateToDisplay = dateOnly.ToString(dateFormat);
            return DateToDisplay;
        }

        public string MakeDateToDisplay(string dateStringInDB)
        {
            DateToDisplay = 
                DateOnly.ParseExact(dateStringInDB, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).
                ToString(dateFormat);
            return DateToDisplay;
        }

        public string MakeDateToDisplay()
        {
            DateToDisplay = DateOnlyCreated.ToString(dateFormat);
            return DateToDisplay;
        }


    }
}
