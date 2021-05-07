using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.SignalR
{
    public interface IBackupHub
    {
        public Task JobUpdate(string jobId, int processPercent);
        public Task StartBackupJob(string jobId);
        public Task StopBackupJob(string jobId);
        public Task BackupFailed(string error);
        public Task BackupCompleted(string filePath);
    }
}
