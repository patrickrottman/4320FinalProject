using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace GradeChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String ZipFilesLocation;
        String ExportFolderLocation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileLocationButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                ZipFilesLocation = dialog.FileName;
                fileLocation.Text = "Absolute File Path: " + ZipFilesLocation;
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if(ZipFilesLocation != null && ZipFilesLocation != "")
            {
                var permissionSet = new PermissionSet(PermissionState.None);
                var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, ZipFilesLocation);
                permissionSet.AddPermission(writePermission);

                try
                {
                    FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, ZipFilesLocation);
                    fileIOPermission.Demand();
                }
                catch (SecurityException se)
                {
                    
                }

                if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    string extractPath = @"C:\Users\PatrickRottman\Desktop\submissions\unzipped";

                    //System.IO.Directory.CreateDirectory(extractPath);

                    //ZipFile.CreateFromDirectory(startPath, zipPath);

                    ZipFile.ExtractToDirectory(ZipFilesLocation, extractPath);
                }
                
            }
            else
            {
                System.Windows.MessageBox.Show("Please choose file directory first.");
            }
        }

        private void FileExportButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                ExportFolderLocation = dialog.FileName;
                fileExportLocation.Text = "Absolute Export File Path: " + ZipFilesLocation;
            }
        }
    }
}
