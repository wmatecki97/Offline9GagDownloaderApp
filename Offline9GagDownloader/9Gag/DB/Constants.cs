namespace Offline9GagDownloader._9Gag.DB
{
    public static class Constants
    {
        public const string DatabaseFilename = "Posts.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.Current.CacheDirectory, DatabaseFilename);
    }
}
