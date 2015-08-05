using System.Collections.Generic;
using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.packager;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TheDashboard.Install.PackageActions
{
    /// <summary>
    /// This action is used for upgrades. It will remove the old icon from the developer-section so that we don't get 
    /// a long list of "The Dashboard"-packages in the list of installed packages.
    /// </summary>
    public class RemoveOldInstallation : IPackageAction
    {
        
        public string Alias()
        {
            return "RemoveOldInstallation";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {

            List<InstalledPackage> list = InstalledPackage.GetAllInstalledPackages().Where(x => x.Data.Name.Equals("The Dashboard")).ToList();

            if (list.Count > 1)
            {
                list.RemoveAt(list.Count - 1);
                foreach (InstalledPackage package in list)
                {
                    package.Delete();
                }
            }
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" alias=\"{0}\"></Action>", this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return true;
        }
    }



}