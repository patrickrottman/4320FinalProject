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
        List<StudentFile> studentFileList = new List<StudentFile>();

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
                fileLocation.Text = "Absolute Zip File Path: " + ZipFilesLocation;
            }
        }

        private void startUnzip_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait; // set the cursor to loading spinner
            if ((ZipFilesLocation != null && ZipFilesLocation != "") || ExportFolderLocation != null && ExportFolderLocation != "")
            {
                ZipFile.ExtractToDirectory(ZipFilesLocation, ExportFolderLocation);
                DirectoryInfo d = new DirectoryInfo(ExportFolderLocation);
                FileInfo[] Files = d.GetFiles("*.zip");
                String customFolder;
                foreach (FileInfo file in Files)
                {
                    try
                    {
                        customFolder = ExportFolderLocation + @"\" + file.Name.Substring(0, file.Name.Length - 4);
                        Directory.CreateDirectory(customFolder);
                        ZipFile.ExtractToDirectory(file.FullName, customFolder);
                        File.Delete(file.FullName);
                    }catch(Exception error)
                    {
                        System.Windows.MessageBox.Show("Unable to extract " + file.Name + " due to: " + error.Message);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please choose file directory first.");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow; // set the cursor back to arrow
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow; // set the cursor back to arrow
        }

        private void FileExportButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                ExportFolderLocation = dialog.SelectedPath;
                fileExportLocation.Text = "Absolute Export File Path: " + ExportFolderLocation;
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait; // set the cursor to loading spinner
            WalkDirectoryTree(new DirectoryInfo(ExportFolderLocation));

            foreach(StudentFile student in studentFileList)
            {
                foreach(StudentFile innerStudent in studentFileList)
                {
                    if(student.fileName != innerStudent.fileName)
                    {
                        double similarity = SimilarityinPercentage(student.fileContents, innerStudent.fileContents);
                        if(similarity > 60)
                        {
                            SimilarStudentFileNames.Items.Add(student.fileName + " and " + innerStudent.fileName + " are " + similarity.ToString("0.00") + "% similar.");
                        }
                    }
                }
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow; // set the cursor back to arrow
        }

        private void WalkDirectoryTree(DirectoryInfo dr)
        {
            String studentFileName;
            foreach (FileInfo file in FindFiles(dr, "*.xaml.cs"))
            {
                // process file
                if (file.Name != "App.xaml.cs")
                {
                    studentFileName = file.FullName.Replace(ExportFolderLocation, "");
                    studentFileName = studentFileName.Substring(1, studentFileName.Length-1);
                    studentFileName = studentFileName.Substring(0, studentFileName.IndexOf(@"\"));
                    studentFileName = studentFileName.Substring(0, studentFileName.IndexOf("_"));
                    //file.Name.Substring(0, file.Name.Length - 4);


                    StudentFile tempStudent = new StudentFile(studentFileName, System.IO.File.ReadAllText(file.FullName));
                    studentFileList.Add(tempStudent);
                    FileNames.Items.Add(tempStudent.fileName);
                }
                
                
            }
        }
        public IEnumerable<FileInfo> FindFiles(DirectoryInfo startDirectory, string pattern)
        {
            return startDirectory.EnumerateFiles(pattern, SearchOption.AllDirectories);
        }

        public double SimilarityinPercentage(String original, String comparison)
        {
            original = original.Replace("\r", "");
            comparison = comparison.Replace("\r", "");

            original = original.Replace("\"", "");
            comparison = comparison.Replace("\"", "");

            original = original.Replace("{", "");
            comparison = comparison.Replace("{", "");

            original = original.Replace("}", "");
            comparison = comparison.Replace("}", "");


            
            IQueryable<String> splitString1 = original.Split('\n').AsQueryable();
            IQueryable<String> splitString2 = comparison.Split('\n').AsQueryable();

            splitString1 = splitString1.Where(s => !string.IsNullOrWhiteSpace(s)).AsQueryable();
            splitString2 = splitString2.Where(s => !string.IsNullOrWhiteSpace(s)).AsQueryable();

            splitString1 = splitString1.Where(x => !x.Contains("using")).AsQueryable();
            splitString2 = splitString2.Where(x => !x.Contains("using")).AsQueryable();

            splitString1 = splitString1.Where(x => !x.Contains("///")).AsQueryable();
            splitString2 = splitString2.Where(x => !x.Contains("///")).AsQueryable();



            // code from http://www.dotnetworld.in/2013/05/c-find-similarity-between-two-strings.html
            var strCommon = splitString1.Intersect(splitString2);
            //Formula : Similarity (%) = 100 * (CommonItems * 2) / (Length of String1 + Length of String2)
            double Similarity = (double)(100 * (strCommon.Count() * 2)) / (splitString1.Count() + splitString2.Count());
            Console.WriteLine("Strings are {0}% similar", Similarity.ToString("0.00"));

            int oldCount = int.Parse(counter.Text);
            counter.Text = "Lines of code compared: " + splitString1.Count() + splitString2.Count() + oldCount;

            return Similarity;
        }
    }
}
