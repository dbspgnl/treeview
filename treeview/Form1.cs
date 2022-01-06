
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace treeview
{
    public partial class Form1 : Form
    {
        // Log Level을 지정 할 Enum
        enum enLogLevel
        {
            Info,
            Warning,
            Error,
        }
       
        public Form1()
        {
            InitializeComponent();
        }

        // TreeView 폴더 기준으로 노드 경로를 가져옴
        private void btnTreeLoad_Click(object sender, EventArgs e)
        {
            TreeViewLoadByPath(tviewLocation, tboxSource.Text);
        }

        private void TreeViewLoadByPath(TreeView treeView, String path)
        {
            tviewLocation.Nodes.Clear();
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeNode directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories()) // 재귀함수 : 폴더 경로 찾음
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }

            //foreach (var file in directoryInfo.GetFiles()) // 파일명 찾기
            //    directoryNode.Nodes.Add(new TreeNode(file.Name));

            return directoryNode;
        }

        private void btnExtend_Click(object sender, EventArgs e)
        {
            if (tviewLocation.SelectedNode != null)
            {
                tviewLocation.SelectedNode.ExpandAll();   // 선택한 Node 부터 하위 노드를 펼침
            }
        }

        private void btnCoolapse_Click(object sender, EventArgs e)
        {
            tviewLocation.CollapseAll();
        }

        private void tviewLocation_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strSelectPath = tviewLocation.SelectedNode.FullPath;

            if (lboxCommand.Items.Contains(strSelectPath))
            {
                Log(enLogLevel.Warning, "선택 한 Folder는 이미 Command 항목에 등록 되어 있습니다.");
                return;
            }

            lboxCommand.Items.Add(strSelectPath);
        }


        #region Log OverLoading
        private void Log(enLogLevel eLevel, string LogDesc)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"{dTime:yyyy-MM-dd hh:mm:ss.fff} [{eLevel.ToString()}] {LogDesc}";
            lboxLog.Items.Insert(0, LogInfo);
        }
        private void Log(DateTime dTime, enLogLevel eLevel, string LogDesc)
        {
            string LogInfo = $"{dTime:yyyy-MM-dd hh:mm:ss.fff} [{eLevel.ToString()}] {LogDesc}";
            lboxLog.Items.Insert(0, LogInfo);
        }

        private string SourcePath()
        {
            string path = tboxSource.Text;
            var lastFolder = Path.GetDirectoryName(path);
            string strpath = lboxCommand.SelectedItem.ToString();   //dc[lboxCommand.SelectedItem.ToString()];
            string dirPath = $@"{lastFolder}\{strpath}";  // TextBox에 적어놓은 Local 경로와 TreeNode에서 가져온 하위 경오를 합쳐서 복사 할 폴더가 있는 경로를 만듬

            return dirPath;
        }

        #endregion

    }
}
