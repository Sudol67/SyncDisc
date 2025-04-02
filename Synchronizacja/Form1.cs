using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synchronizacja
{
    public partial class Form1 : Form
    {
        private string FolderSourcePath;
        private string FolderTargetPath;
        private CancellationTokenSource cancellationTokenSource;
        private bool isCancelled;

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
            button10.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList2.Images[0];
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Przycisk wyłączania
            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zakończyć działanie programu?", "Komunikat", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Ustawienia
            MessageBox.Show("Ustawienia nie są jeszcze dostępne", "Komunikat");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Pomoc
        }
        //---------- Wczytywanie ścieżek ---------------------------------------

        private void button6_Click(object sender, EventArgs e)
        {
            //Wczytywanie źródła
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Wybierz folder źródłowy";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    FolderSourcePath = folderBrowser.SelectedPath;
                    richTextBox1.Text = FolderSourcePath;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Czyaszczenie źródła
            richTextBox1.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Wczytywanie docelowego
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Wybierz folder docelowy";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    FolderTargetPath = folderBrowser.SelectedPath;
                    richTextBox2.Text = FolderTargetPath;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Czyszczenie docelowego
            richTextBox2.Text = "";
        }

        //---------Sprawdzanie zgodności------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            //Check

            if (Directory.Exists(FolderSourcePath) && Directory.Exists(FolderTargetPath))
            {
                listView1.Items.Clear();

                var sourceFiles = Directory.GetFiles(FolderSourcePath, "*", SearchOption.AllDirectories);
                var sourceDirectories = Directory.GetDirectories(FolderSourcePath, "*", SearchOption.AllDirectories);

                var destinationFiles = Directory.GetFiles(FolderTargetPath, "*", SearchOption.AllDirectories);
                var destinationDirectories = Directory.GetDirectories(FolderTargetPath, "*", SearchOption.AllDirectories);

                foreach (var file in sourceFiles)
                {
                    string destFilePath = file.Replace(FolderSourcePath, FolderTargetPath);

                    if (!File.Exists(destFilePath))
                    {
                        string fileName = Path.GetFileName(file);
                        listView1.Items.Add(new ListViewItem(new[] { "Plik", fileName, "Brak", file }));
                    }
                    else
                    {
                        FileInfo sourceFileInfo = new FileInfo(file);
                        FileInfo destFileInfo = new FileInfo(destFilePath);

                        if (sourceFileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                        {
                            string fileName = Path.GetFileName(file);
                            listView1.Items.Add(new ListViewItem(new[] { "Plik", fileName, "Nowsza wersja", file  }));
                        }
                    }
                }

                foreach (var dir in sourceDirectories)
                {
                    string destDirPath = dir.Replace(FolderSourcePath, FolderTargetPath);

                    if (!Directory.Exists(destDirPath))
                    {
                        string folderName = Path.GetFileName(dir);
                        listView1.Items.Add(new ListViewItem(new[] { "Folder", folderName, "Brak", dir  }));
                    }
                }
                if (listView1.Items.Count > 0)
                {
                    button10.Enabled = true;
                    button1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Brak plików do synchronizacji");
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else
            {
                MessageBox.Show("Podane ścieżki nie są prawidłowe");
            }
        }
        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Rodzaj", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Nazwa", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Status", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Ścieżka", 400, HorizontalAlignment.Left);
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            // Rozpocznij synchronizację
            if (Directory.Exists(FolderSourcePath) && Directory.Exists(FolderTargetPath))
            {
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                isCancelled = false;

                try
                {
                    await Task.Run(() => SynchronizeFilesAndFolders(token), token);
                    if (!isCancelled)
                    {
                        MessageBox.Show("Synchronization complete!");
                    }
                }
                catch (OperationCanceledException)
                {
                    isCancelled = true;
                    MessageBox.Show("Synchronization was cancelled.");
                }
                finally
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                    button1.Enabled = true;
                    button10.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please provide valid source and destination paths.");
            }
        }


        private void UpdateListViewItem(ListViewItem item)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(() => UpdateListViewItem(item)));
            }
            else
            {
                item.BackColor = Color.LightGreen;
            }
        }
        private void SynchronizeFilesAndFolders(CancellationToken token)
        {
            List<ListViewItem> itemsToProcess = new List<ListViewItem>();

            listView1.Invoke((MethodInvoker)delegate
            {
                listView1.ListViewItemSorter = new ListViewItemComparer(0);
                foreach (ListViewItem item in listView1.Items)
                {
                    itemsToProcess.Add(item);
                }
            });

            try
            {
                foreach (ListViewItem item in itemsToProcess)
                {
                    token.ThrowIfCancellationRequested();

                    string itemType = item.SubItems[0].Text;
                    string itemPath = item.SubItems[3].Text;
                    string destPath = itemPath.Replace(FolderSourcePath, FolderTargetPath);

                    if (itemType == "Folder")
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    else if (itemType == "Plik")
                    {
                        string destDirectory = Path.GetDirectoryName(destPath);
                        if (!Directory.Exists(destDirectory))
                        {
                            Directory.CreateDirectory(destDirectory);
                        }

                        if (item.SubItems[2].Text == "Brak" || item.SubItems[2].Text == "Nowsza wersja")
                        {
                            File.Copy(itemPath, destPath, true);
                        }
                    }

                    UpdateListViewItem(item);
                }
            }
            catch (OperationCanceledException)
            {
                isCancelled = true;
            }
        }

        public class ListViewItemComparer : System.Collections.IComparer
        {
            private int col;
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (itemX.SubItems[col].Text == "Folder" && itemY.SubItems[col].Text == "Plik")
                    return -1;
                if (itemX.SubItems[col].Text == "Plik" && itemY.SubItems[col].Text == "Folder")
                    return 1;

                return string.Compare(itemX.SubItems[col].Text, itemY.SubItems[col].Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Stop
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }
}