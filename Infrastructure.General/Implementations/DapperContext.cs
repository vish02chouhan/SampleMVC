using Domain.Entities;
using Infrastructure.General.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Infrastructure.General.Implementation.Helpers;

namespace Infrastructure.General.Implementation
{
    public class DapperContext : IDapperContext
    {
        protected string ConnectionString;

        private IDbConnection _connection;

        public AppSettings Appsettings { get; }

        public int TimeoutPeriod { get; set; }

        public DapperContext(AppSettings appSettings)
        {
            Appsettings = appSettings;
            ConnectionString = Appsettings.ConnectionString;
            ConnectionString += Appsettings.ConnectionStringPassword.Decrypt();
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(ConnectionString);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        //public DapperContext()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json");

        //    IConfiguration Configuration = builder.Build();
        //    //var encrypt = "ElasticSmart".Encrypt();
        //    ConnectionString = Configuration.GetSection("AppSettings")["ConnectionStrings:GXCCDDatabase"];
        //    if (ConnectionString == "" || ConnectionString == null)
        //    {
        //        ConnectionString = Configuration.GetSection("AppSettings")["ConnectionStrings:GXCSearchDatabase"];
        //    }
        //    ConnectionString += Configuration.GetSection("AppSettings")["ConnectionStrings:Password"];
        //}

        public DapperContext()
        {
            //var encrypt = "ElasticSmart".Encrypt();
        
        }

        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                //test comment
                // SqlConnection's dispose method will close the connection for us
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync(); // Asynchronously open a connection to the database
                    return await getData(connection);
                    // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
                }
            }
            catch (TimeoutException ex)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
