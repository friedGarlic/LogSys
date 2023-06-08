using System.Configuration;

namespace LogAppLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection DataConnections { get; private set; }

        public static void In_Connection(DatabaseType databaseType)
        {
            if(databaseType == DatabaseType.Sql)
            {
                SqlConnector sqlConnector = new SqlConnector();
                DataConnections = sqlConnector;
            }
        }

        public static string ConnectString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
