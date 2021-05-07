using Data.Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Data.BackgrooundServices
{
    public class BackupJob : IBackupJob
    {
        private readonly ILogger<BackupService> _logger;
        private readonly IHubContext<BackupHub, IBackupHub> _backupHub;
        private readonly IConfiguration _configuration;

        private string _jobId;
        private BackgroundWorker _backgroundWorker;
        private string _dirPath;
        private string _fileName;

        public BackupJob(ILogger<BackupService> logger, IHubContext<BackupHub, IBackupHub> backupHub,
            IConfiguration configuration)
        {
            _logger = logger;
            _backupHub = backupHub;
            _configuration = configuration;
            _dirPath = @"C:\KHM_DB_Backup";
        }

        public void StartBackupDatabase(string jobId)
        {
            _jobId = jobId;
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += DoBackup;
            //_backgroundWorker.RunWorkerCompleted += DoWorkCompleted;
            _backgroundWorker.RunWorkerAsync(jobId);
            _logger.LogDebug($"Start backup job {jobId}");
        }

        public void StopBackupDatabase(string jobId)
        {
            if (_backgroundWorker != null)
            {
                _backgroundWorker.CancelAsync();
            }
        }

        private void DoBackup(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_jobId)) return;

            string constring = _configuration.GetConnectionString("MariaDb");

            // Important Additional Connection Options
            //constring += "charset=utf8;convertzerodatetime=true;";

            if (!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
            }
            _fileName = DateTime.Now.ToString("dd-MM-yyyy") + ".sql";
            var filePath = _dirPath + "\\" + _fileName;

            using var conn = new MySqlConnection(constring);
            using var cmd = new MySqlCommand();
            using MySqlBackup mb = new MySqlBackup(cmd);
            mb.ExportCompleted += BackupCompleted;
            try
            {
                cmd.Connection = conn;
                conn.Open();
                mb.ExportToFile(filePath);
                conn.Close();
            }catch(Exception ex)
            {
                _backupHub.Clients.Group(_jobId).BackupFailed(ex.Message);
                _logger.LogError($"Backup error: {ex.Message}");
            }
        }

        private void BackupCompleted(object sender, ExportCompleteArgs e)
        {
            if(_backgroundWorker.CancellationPending)
            {
                _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, 0);
                _logger.LogDebug($"Finish backup job {_jobId}");

            } else
            {
                var filePath = _dirPath + "\\" + _fileName;
                _backupHub.Clients.Group(_jobId).BackupCompleted(filePath);
                _logger.LogDebug($"Finish backup job {_jobId}");
            }
        }

        private void DoWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, 100);
            _logger.LogDebug($"Finish backup job {_jobId}");
        }
    }
}
