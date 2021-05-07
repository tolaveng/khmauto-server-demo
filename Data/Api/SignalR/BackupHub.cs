using Data.BackgrooundServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Api.SignalR
{
    public class BackupHub : Hub<IBackupHub>
    {
        private readonly IBackupJob _backupJob;
        public BackupHub(IBackupJob backupJob)
        {
            _backupJob = backupJob;
        }

        public async Task SendJobUpdate(string jobId, int processPercent)
        {
            // Old way:
            // await Clients.Group(jobId).SendAsync("JobUpdate", processPercent);
            //await Clients.Group(jobId).JobUpdate(jobId, processPercent);
        }

        public void StartBackupJob(string jobId)
        {
            _backupJob.StartBackupDatabase(jobId);
        }

        public void StopBackupJob(string jobId)
        {
           _backupJob.StopBackupDatabase(jobId);
        }

        

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var jobId = httpContext.Request.Query["JobId"];
            if (!string.IsNullOrWhiteSpace(jobId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
                //await Clients.Caller.SendAsync("InitJob", jobId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _backupJob.StopBackupDatabase("0");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
