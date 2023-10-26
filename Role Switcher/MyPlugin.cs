using XrmToolBox.Extensibility.Interfaces;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using XrmToolBox.Extensibility;
using System.Reflection;
using System.Linq;
using System.IO;
using System;

namespace Role_Switcher
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Dynamics Bulk Role Updater"),
        ExportMetadata("Description", "Create sets of roles and apply them to users with 1 click!"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAANHAAADRwF0rlF2AAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAApZJREFUWIXtl81rE0EYh5830RV6lgo9KJK2Cj3owYO14KEQL0JSlIKlOQrq3c/2Uk/9D6y19WI/"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Yellow"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new RoleSwitcher();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
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