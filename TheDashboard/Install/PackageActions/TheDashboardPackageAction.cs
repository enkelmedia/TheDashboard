using System.Xml;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.IO;
using umbraco.interfaces;

namespace TheDashboard.Install.PackageActions
{
    public class TheDashboardPackageAction : IPackageAction
    {
        public string Alias()
        {
            return "TheDashboard";
        }

        public XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"TheDashboard\"/>";
            return umbraco.cms.businesslogic.packager.standardPackageActions.helper.parseStringToXmlNode(sample);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            //If not there, add the dashboards to dashboard.config

            this.AddSectionDashboard("TheDashboard", "content", "Welcome", "/app_plugins/TheDashboard/TheDashboard.html");
            this.AddSectionDashboard("TheDevDashboard", "developer", "Developer dashboard", "/app_plugins/TheDashboard/TheDevDashboard.html");
            this.AddSectionDashboard("TheUserDashboard", "users", "User session dashboard", "/app_plugins/TheDashboard/TheUserDashboard.html");

            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            // Remove the dashboards from Dashboard.config
            this.RemoveDashboardTab("TheDashboard");
            this.RemoveDashboardTab("TheDevDashboard");
            this.RemoveDashboardTab("TheUserDashboard");

            return true;
        }

        /// <summary>
        /// Adds the required XML to the dashboard.config file
        /// </summary>
        /// <param name="sectionAlias">Alias of the section. a-z, A-z 0-1 and no blank spaces</param>
        /// <param name="area">Enter the area where you want to show the dashboard</param>
        /// <param name="tabCaption">Enter the caption to be shown in the dashboard tab</param>
        /// <param name="src"></param>
        public void AddSectionDashboard(string sectionAlias,string area, string tabCaption, string src)
        {
            bool saveFile = false;

            //Path to the file resolved
            var dashboardFilePath = IOHelper.MapPath(SystemFiles.DashboardConfig);

            Log.Add(LogTypes.Notify,0,"Adding dashboard section " + sectionAlias + " in: " + dashboardFilePath);

            //Load settings.config XML file
            XmlDocument dashboardXml = new XmlDocument();
            dashboardXml.Load(dashboardFilePath);

            // Section Node
            XmlNode findSection = dashboardXml.SelectSingleNode("//section [@alias='" + sectionAlias + "']");

            //Couldn't find it
            if (findSection == null)
            {
                //Let's add the xml
                var xmlToAdd = "<section alias='" + sectionAlias + "'>" +
                                    "<areas>" +
                                        "<area>"+ area +"</area>" +
                                    "</areas>" +
                                    "<tab caption='" + tabCaption + "'>" +
                                        "<control addPanel='true' panelCaption=''>" + src + "</control>" +
                                    "</tab>" +
                               "</section>";

                //Get the main root <dashboard> node
                XmlNode dashboardNode = dashboardXml.SelectSingleNode("//dashBoard");

                if (dashboardNode != null)
                {
                    //Load in the XML string above
                    XmlDocument xmlNodeToAdd = new XmlDocument();
                    xmlNodeToAdd.LoadXml(xmlToAdd);

                    var toAdd = xmlNodeToAdd.SelectSingleNode("*");

                    //Prepend the xml above to the dashboard node - so that it will be the first dashboards to show in the backoffice.
                    dashboardNode.PrependChild(dashboardNode.OwnerDocument.ImportNode(toAdd, true));

                    //Save the file flag to true
                    saveFile = true;
                }
            }

            //If saveFile flag is true then save the file
            if (saveFile)
            {
                //Save the XML file
                dashboardXml.Save(dashboardFilePath);
            }
        }

        /// <summary>
        /// Removes a tab from the dashboard configuration.
        /// </summary>
        /// <param name="sectionAlias"></param>
        public void RemoveDashboardTab(string sectionAlias)
        {
            
            string dbConfig = IOHelper.MapPath(SystemFiles.DashboardConfig);
            XmlDocument dashboardFile = XmlHelper.OpenAsXmlDocument(dbConfig);

            XmlNode section = dashboardFile.SelectSingleNode("//section [@alias = '" + sectionAlias + "']");

            if (section != null)
            {
                dashboardFile.SelectSingleNode("/dashBoard").RemoveChild(section);
                dashboardFile.Save(dbConfig);
            }

        }
    }
}