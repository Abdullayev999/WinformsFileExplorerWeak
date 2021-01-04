using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsFileExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //   Directory / DirectoryInfo
            //   File      / FileInfo
            //   Path

            var rootDirectories = Directory.EnumerateDirectories(@"C:\");

            foreach (var directoryPath in rootDirectories)
            {
                TreeNode treeNode = new TreeNode(Path.GetFileName(directoryPath));
                treeNode.Tag = directoryPath;
                folderTreeView1.Nodes.Add(treeNode);

                try
                {
                    var childDirectories = Directory.EnumerateDirectories(directoryPath);


                    foreach (var path in childDirectories)
                    {
                        TreeNode childTreeNode = new TreeNode(Path.GetFileName(path));
                        childTreeNode.Tag = path;
                        treeNode.Nodes.Add(childTreeNode);
                    }
                }
                catch (Exception e)
                {
                }
            }


        }

        private void folderTreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            foreach (TreeNode item in e.Node.Nodes)
            {

                if (item.Nodes.Count == 0)
                {
                    try
                    {
                        var dirictories = Directory.EnumerateDirectories(item.Tag as string);
                        foreach (var path in dirictories)
                        {
                            TreeNode treeNode = new TreeNode(Path.GetFileName(path));

                            treeNode.Tag = path;
                            item.Nodes.Add(treeNode);
                        }
                    }
                    catch (Exception exception) { }
                }

            }
        }

        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var path = e.Node.Tag as string;
            OpenFolder(path);
            
        }

        private void OpenFolder(string path)
        {
            pathTextBox.Text = path;
           
            try
            {
                var fileCount = Directory.EnumerateFileSystemEntries(path).Count();
                fileToolStripStatusLabel1.Text = $"Files : {fileCount}";


                 var entries = Directory.EnumerateFileSystemEntries(path);
                fileListView.Clear();

                foreach (var entry in entries)
                {
                    ListViewItem item;

                    if (Path.HasExtension(entry))
                        item = new ListViewItem(Path.GetFileName(entry), 1);
                    else
                        item = new ListViewItem(Path.GetFileName(entry), 0);

                    item.Tag = entry;
                    fileListView.Items.Add(item);
                }
            }
            catch (Exception exception)
            {
            }
        }
        private void fileListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            fileListView.View = View.LargeIcon;
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileListView.View = View.SmallIcon;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileListView.View = View.List;
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileListView.View = View.Tile;
        }

        private void fileListView_ItemActivate(object sender, EventArgs e)
        {
            var selection = fileListView.SelectedItems[0];
            var path = selection.Tag as string;
            if (Path.HasExtension(path))
            {
                Process.Start(path);
            }
            else
            {
                OpenFolder(path);
            }
        }

        private void backButton1_Click(object sender, EventArgs e)
        {
            if (pathTextBox.Text.Length>=4) 
            {
                var lastSymbolIndexOf = pathTextBox.Text.LastIndexOf(@"\");
                var path = pathTextBox.Text.Substring(0, lastSymbolIndexOf);
                OpenFolder(path);
            }
        }
    }
}
