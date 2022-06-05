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
        private string _selectedFileName;
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
                _backupHub.Clients.Group(_jobId).JobFailed(ex.Message);
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
                _backupHub.Clients.Group(_jobId).JobCompleted(filePath);
                _logger.LogDebug($"Finish backup job {_jobId}");
            }
        }

        private void DoWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, 100);
            _logger.LogDebug($"Finish backup job {_jobId}");
        }

        public void StartRestoreDatabase(string jobId, string selectedFileName)
        {
            _jobId = jobId;
            _selectedFileName = selectedFileName;
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += DoRestore;
            _backgroundWorker.RunWorkerAsync(jobId);
            _logger.LogDebug($"Start restore job {jobId}");
        }

        public void StopRestoreDatabase(string jobId)
        {
            if (_backgroundWorker != null)
            {
                _backgroundWorker.CancelAsync();
            }
        }

        private void DoRestore(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_jobId) || string.IsNullOrWhiteSpace(_selectedFileName)) return;
            if (!File.Exists(_selectedFileName))
            {
                _backupHub.Clients.Group(_jobId).JobFailed($"File is not found in server. {_selectedFileName}");
                return;
            }

            string constring = _configuration.GetConnectionString("MariaDb");

            using var conn = new MySqlConnection(constring);
            using var cmd = new MySqlCommand();
            using MySqlBackup mb = new MySqlBackup(cmd);
            mb.ImportProgressChanged += RestoreProgress;
            mb.ImportCompleted += RestoreCompleted;
            try
            {
                cmd.Connection = conn;
                conn.Open();
                mb.ImportFromFile(_selectedFileName);
                conn.Close();
            }
            catch (Exception ex)
            {
                _backupHub.Clients.Group(_jobId).JobFailed(ex.Message);
                _logger.LogError($"Restore error: {ex.Message}");
            }
        }

        private void RestoreProgress(object sender, ImportProgressArgs e)
        {
            if (_backgroundWorker.CancellationPending) return;
            _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, e.PercentageCompleted);
        }

        private void RestoreCompleted(object sender, ImportCompleteArgs e)
        {
            if (_backgroundWorker.CancellationPending)
            {
                _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, 0);
                _logger.LogDebug($"Cancel restore job {_jobId}");
            }
            else
            {
                _backupHub.Clients.Group(_jobId).JobUpdate(_jobId, 100);
                _backupHub.Clients.Group(_jobId).JobCompleted(_selectedFileName);
                _logger.LogDebug($"Finish restore job {_jobId}");
            }
        }
    }
}
