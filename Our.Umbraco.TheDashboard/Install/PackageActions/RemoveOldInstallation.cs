using System.Xml.Linq;
using Umbraco.Core.PackageActions;

namespace Our.Umbraco.TheDashboard.Install.PackageActions
{
    /// <summary>
    /// This action is used for upgrades. It will remove the old icon from the developer-section so that we don't get 
    /// a long list of "The Dashboard"-packages in the list of installed packages.
    /// </summary>
    public class RemoveOldInstallation : IPackageAction
    {
        public bool Execute(string packageName, XElement xmlData)
        {
            //List<InstalledPackage> list = InstalledPackage.GetAllInstalledPackages().Where(x => x.Data.Name.Equals("The Dashboard")).ToList();

            //if (list.Count > 1)
            //{
            //    list.RemoveAt(list.Count - 1);
            //    foreach (InstalledPackage package in list)
            //    {
            //        package.Delete();
            //    }
            //}
            return true;
        }

        public string Alias()
        {
            return "RemoveOldInstallation";
        }

        public bool Undo(string packageName, XElement xmlData)
        {
            return true;
        }

       
    }

}