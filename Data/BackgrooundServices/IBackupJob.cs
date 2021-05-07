using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.BackgrooundServices
{
    public interface IBackupJob
    {
        void StartBackupDatabase(string jobId);
        void StopBackupDatabase(string jobId);
    }
}
