using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace SampleWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                BackupDatabase();
                await Task.Delay(10000, stoppingToken);
            }
        }

        private void BackupDatabase()
        {
            string DbName = _configuration["BackupOptions:DbName"];
            string DestinationPath = _configuration["BackupOptions:DestinationPath"];
            string ServerName = _configuration["BackupOptions:ServerName"];
            string UserName = _configuration["BackupOptions:UserName"];
            string Password = _configuration["BackupOptions:Password"];
            
            Backup backup = new Backup();

            backup.Action = BackupActionType.Database;
            backup.BackupSetDescription = "Backup of : " + DbName + "on" + DateTime.Now.ToShortDateString();
            backup.BackupSetName = "FullBackup";
            backup.Database = DbName;

            //Declare a BackupDeviceItem
            DateTime now = DateTime.Now;
            string backupFileName = $"{DbName}-{now.Year}-{now.Month}-{now.Day}-{now.Hour}-{now.Minute}-{now.Second}.bak";
            BackupDeviceItem deviceItem = new BackupDeviceItem(DestinationPath + "\\" + backupFileName, DeviceType.File);

            ServerConnection connection;
            
            if(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                connection = new ServerConnection(ServerName);
            else
                connection = new ServerConnection(ServerName, UserName, Password);

            //To Avoid Timeout Exception
            Server sqlServer = new Server(connection);
            sqlServer.ConnectionContext.StatementTimeout = 60 * 60;
            Database db = sqlServer.Databases[DbName];

            backup.Initialize = true;
            backup.Checksum = true;
            backup.ContinueAfterError = true;

            //Add the device to the backup object
            backup.Devices.Add(deviceItem);

            //Set rhe Incremental property to False to specify that this is a full database backup
            backup.Incremental = false;

            backup.ExpirationDate = DateTime.Now.AddDays(3);
            backup.LogTruncation = BackupTruncateLogType.Truncate;

            backup.FormatMedia = false;

            backup.SqlBackup(sqlServer);
            backup.Devices.Remove(deviceItem);

            _logger.LogInformation("Successful backup is created!");

        }
    }
}