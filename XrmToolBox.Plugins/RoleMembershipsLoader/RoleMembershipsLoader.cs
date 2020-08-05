using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace RoleMembershipsLoader
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Role and Field Security Profile Memberships Loader"),
        ExportMetadata("Description", "Loads memberships of security roles, fields security profiles and systemusers"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "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"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class RoleMembershipsLoader : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new RolePluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public RoleMembershipsLoader()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}