namespace Naos.FileStorage.Rest
{
    using System.Configuration;
    using System.Web.Http;
    using FileStorage.Repositories;
    using Microsoft.Practices.Unity;
    using Storage;
    using Unity.WebApi;

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IFileStorageProvider, UfsFileStorageProvider>();

            var repositoryConnectionString = ConfigurationManager.ConnectionStrings["FileStorageService"].ConnectionString;
            container.RegisterType<IFileMetaDataRepository, FileMetaDataRepository>(new InjectionConstructor(repositoryConnectionString));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}