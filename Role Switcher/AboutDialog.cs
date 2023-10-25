using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Role_Switcher
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();


            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionString = version.ToString();

            versionLabel.Text = versionString;

            createdByLink.Links.Add(0, 50, "https://joepitts.co.uk/");
            githubLink.Links.Add(0, 100, "https://github.com/JoePittsy/Dynamics-Bulk-Role-Updater");

        }

        private void createdByLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());

        }
    }
}
