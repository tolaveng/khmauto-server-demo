using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.SignalR
{
    public interface IBackupHub
    {
        public Task JobUpdate(string jobId, int processPercent);
        public Task JobFailed(string error);
        public Task JobCompleted(string filePath);
        public Task StartBackupJob(string jobId);
        public Task StopBackupJob(string jobId);
        public Task StartRestoreJob(string jobId, string selectedFileName);
        public Task StopRestoreJob(string jobId);
    }
}
