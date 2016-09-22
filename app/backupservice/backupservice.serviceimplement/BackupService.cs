using BackupService.ServiceInterface;
using Petecat.IoC.Attributes;

namespace BackupService.ServiceImplement
{
    [AutoResolvable(typeof(IBackupService))]
    public class BackupService : IBackupService
    {
        public void Backup()
        {
            throw new System.NotImplementedException();
        }
    }
}
