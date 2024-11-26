using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class ResourceManagerServiceImpl : IResourceManagerService
{
    private const string PrefixDirectoryPath = "files";

    private static string MakeResourcePathById<TEntity>(string resourceId)
        => Path.Combine(PrefixDirectoryPath, typeof(TEntity).FullName + resourceId);

    Stream IResourceManagerService.Resource<TEntity>(string resourceId, bool readOnly)
    {
        if (!Directory.Exists(PrefixDirectoryPath))
            Directory.CreateDirectory(PrefixDirectoryPath);
        var path = MakeResourcePathById<TEntity>(resourceId);
        return new FileStream(path,
            readOnly ? FileMode.Open : FileMode.Create,
            readOnly ? FileAccess.Read : FileAccess.Write
        );
    }

    bool IResourceManagerService.RemoveResource<TEntity>(string resourceId)
    {
        if (Directory.Exists(PrefixDirectoryPath))
        {
            var path = MakeResourcePathById<TEntity>(resourceId);
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
        }
        return false;
    }
}