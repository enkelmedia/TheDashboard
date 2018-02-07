using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Web.Strategies.Migrations;

namespace TheDashboard.Install.PackageActions.Helpers
{
    /// <summary>
    /// Kudos to the Merchello guys for this approach of merging lang files back into the main
    /// Umbraco lang files
    /// https://github.com/Merchello/Merchello/blob/1.7.1/src/Merchello.Web/PackageActions/AddLocalizationAreas.cs
    /// </summary>
    public class TranslationHelper
    {
        // Set the path of the language files directory
        private const string UmbracoLangPath = "~/umbraco/config/lang/";
        private const string PackageLangPath = "~/App_Plugins/TheDashboard/Lang/";

        public static bool ShouldRun()
        {
            // If older than Umbraco 7.3 (where lang-feature where added) copy over the lang-entries.
            if (UmbracoVersion.Current.Major == 7 && UmbracoVersion.Current.Minor < 3)
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<FileInfo> GetUmbracoLanguageFiles()
        {
            var umbPath = HostingEnvironment.MapPath(UmbracoLangPath);
            var di = new DirectoryInfo(umbPath);
            var files = di.GetFiles("*.xml");
            return files;
        }

        public static IEnumerable<FileInfo> GetPackageLanguageFiles()
        {
            var packageLangPath = HostingEnvironment.MapPath(PackageLangPath);
            var di = new DirectoryInfo(packageLangPath);
            var files = di.GetFiles("*.xml");
            return files;
        }



        public static IEnumerable<FileInfo> GetUmbracoLanguageFilesToInsertLocalizationData()
        {

            var umbFiles = GetUmbracoLanguageFiles();
            var packageFiles = GetPackageLanguageFiles();

            var res = umbFiles.Where(x => packageFiles.Any(packageFile => IsFileNameMatch(x, packageFile)));

            return res;
        }

        public static bool IsFileNameMatch(FileInfo umbracoFile, FileInfo packageFile)
        {
            var umbLang = Path.GetFileNameWithoutExtension(umbracoFile.Name);
            var packgLang = Path.GetFileNameWithoutExtension(packageFile.Name);

            // process Umbraco file - replace _ with since older version of Umbraco uses _ for lang files.
            umbLang = umbLang.Replace("_", "-");

            var bRes = packgLang.ToLower().Contains(umbLang.ToLower());

            return bRes;
        }

        public static void AddTranslations()
        {
            if (!TranslationHelper.ShouldRun())
                return;

            var packageLanguageFiles = GetPackageLanguageFiles();
            LogHelper.Info<TranslationHelper>(string.Format("{0} Dashboard language files to be merged into Umbraco language files", packageLanguageFiles.Count()));

            //Convert to an array
            var packageFileArray = packageLanguageFiles as FileInfo[] ?? packageLanguageFiles.ToArray();

            //Check which language filenames that we have match up
            var existingLangs = GetUmbracoLanguageFilesToInsertLocalizationData();
            LogHelper.Info<TranslationHelper>(string.Format("{0} Umbraco language files that match up with our Dashboard language files", existingLangs.Count()));

            //For each umbraco language file...
            foreach (var lang in existingLangs)
            {
                var package = new XmlDocument() { PreserveWhitespace = true };
                var umb = new XmlDocument() { PreserveWhitespace = true };

                try
                {
                    //From our package language file/s - try & find a file with the same name
                    var match = packageFileArray.FirstOrDefault(packageLang => IsFileNameMatch(lang, packageLang));

                    //Ensure we have a match & not null
                    if (match != null)
                    {
                        //Load the two XML files
                        package.LoadXml(File.ReadAllText(match.FullName));
                        umb.LoadXml(File.ReadAllText(lang.FullName));

                        //Get all of the <area>'s from package lang XML file & their child elements
                        var areas = package.DocumentElement.SelectNodes("//area");

                        //For each <area> in our XML...
                        foreach (XmlNode area in areas)
                        {
                            //Get the current area in this loop from our package translation file - alias attribute
                            var aliasToTryFind = area.Attributes["alias"];

                            //Try and find <area> with same alias in the umbraco file
                            var findAreaInUmbracoLang = umb.SelectSingleNode(string.Format("//area [@alias='{0}']", aliasToTryFind.Value));

                            //Can not find <area> to import/merge in Umbraco lang file
                            if (findAreaInUmbracoLang == null)
                            {
                                //So let's just import the area and child keys
                                var import = umb.ImportNode(area, true);
                                umb.DocumentElement.AppendChild(import);
                            }
                            else
                            {
                                //We have found the <area> so don't just overwrite from what we have
                                //Ensure to go through each key and check we have it or not
                                foreach (XmlNode areaKey in area.ChildNodes)
                                {
                                    if(areaKey==null)
                                        continue;

                                    //Added as area.childNodes contained 3 items for one element - with 2 being WhiteSpace elements
                                    if (areaKey.NodeType == XmlNodeType.Element)
                                    {
                                        //Get the current area in this loop from our package translation file - alias attribute
                                        var keyAliasToTryFind = areaKey.Attributes["alias"];

                                        //Try and find <key> is in the Umbraco XML lang doc
                                        var findKeyInUmbracoLang = findAreaInUmbracoLang.SelectSingleNode(string.Format("./key [@alias='{0}']", keyAliasToTryFind.Value));

                                        //Can not find <key> in Umbraco lang file - let's add it
                                        //And DO NOTHING if we do find it - don't want to overwrite it
                                        if (findKeyInUmbracoLang == null)
                                        {
                                            var keyImport = umb.ImportNode(areaKey, true);
                                            findAreaInUmbracoLang.AppendChild(keyImport);
                                        }
                                    }


                                }
                            }
                        }

                        //Save the umb lang file with the merged contents
                        umb.Save(lang.FullName);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<TranslationHelper>("Failed to add Dashboard localization values to language file", ex);
                }

            }
        }

        public static void RemoveTranslations()
        {
            if (!TranslationHelper.ShouldRun())
                return;

            var packageFiles = GetPackageLanguageFiles();
            LogHelper.Info<TranslationHelper>(string.Format("{0} Dashboard Plugin language files to be removed into Umbraco language files", packageFiles.Count()));

            //Convert to an array
            var packageFileArray = packageFiles as FileInfo[] ?? packageFiles.ToArray();

            //Check which language filenames that we have match up
            var existingLangs = GetUmbracoLanguageFilesToInsertLocalizationData();
            LogHelper.Info<TranslationHelper>(string.Format("{0} Umbraco language files that match up with our Dashboard language files", existingLangs.Count()));

            //For each umbraco language file...
            foreach (var lang in existingLangs)
            {
                var package = new XmlDocument() { PreserveWhitespace = true };
                var umb = new XmlDocument() { PreserveWhitespace = true };

                try
                {
                    //From our package language file/s - try & find a file with the same name
                    var match = packageFileArray.FirstOrDefault(packageLang => IsFileNameMatch(lang, packageLang));

                    //Ensure we have a match & not null
                    if (match != null)
                    {
                        //Load the two XML files
                        package.LoadXml(File.ReadAllText(match.FullName));
                        umb.LoadXml(File.ReadAllText(lang.FullName));

                        //Get all of the <area>'s from packages XML file & their child elements
                        var areas = package.DocumentElement.SelectNodes("//area");

                        //For each <area> in our XML...
                        foreach (XmlNode area in areas)
                        {
                            //Get the current area in this loop from our package translation file - alias attribute
                            var aliasToTryFind = area.Attributes["alias"];

                            //Try and find <area> with same alias in the umbraco file
                            var areaInUmbracoLang = umb.SelectSingleNode(string.Format("//area [@alias='{0}']", aliasToTryFind.Value));

                            //Found <area> with alias to remove from Umbraco lang file
                            if (areaInUmbracoLang != null)
                            {
                                //We have found the <area> so don't just REMOVE it entirely from what we have
                                //As may be 'treeHeaders' or 'sections' as the area which is core Umbraco
                                //Ensure to go through each key and check we have it or not
                                foreach (XmlNode areaKey in area.ChildNodes)
                                {
                                    if(areaKey == null || areaKey.Attributes == null)
                                        continue;
                                    
                                    //Get the current area in this loop from our package translation file - alias attribute
                                    var keyAliasToTryFind = areaKey.Attributes["alias"];

                                    for (int i = 0; i < areaInUmbracoLang.ChildNodes.Count; i++)
                                    {
                                        if (areaInUmbracoLang.ChildNodes[i] != null && areaInUmbracoLang.ChildNodes[i].Attributes != null && areaInUmbracoLang.ChildNodes[i].Name=="key")
                                        {
                                            if (areaInUmbracoLang.ChildNodes[i].Attributes["alias"].Value == keyAliasToTryFind.Value)
                                            {
                                                areaInUmbracoLang.RemoveChild(areaInUmbracoLang.ChildNodes[i]);
                                            }
                                        }
                                    }
                                    
                                }

                                // After looping through all - lets check we have no <key> left in them. Since there is "whitespace" nodes we can't just use 
                                // ChildNodes.Count as it would count the empty objects.

                                var validChildsCount = 0;
                                for (int i = 0; i < areaInUmbracoLang.ChildNodes.Count; i++)
                                {
                                    if (areaInUmbracoLang.ChildNodes[i] != null && areaInUmbracoLang.ChildNodes[i].Attributes != null)
                                    {
                                        validChildsCount++;
                                    }
                                }

                                if (validChildsCount == 0)
                                {
                                    //No child nodes - so is our custom package areas as opposed to Umbraco core ones 'treeHeaders' & 'sections'
                                    //Remove the area itself as it's empty

                                    var parent = areaInUmbracoLang.ParentNode;
                                    parent.RemoveChild(areaInUmbracoLang);

                                }
                            }
                        }

                        //Save the umb lang file with the merged contents
                        umb.Save(lang.FullName);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<TranslationHelper>("Failed to add Dashboard localization values to language file", ex);
                }
            }

        }
    }

    
}


