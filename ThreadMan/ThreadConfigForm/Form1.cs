using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewLife.Log;
using ThreadMan;

namespace ThreadConfigForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        public int processId = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "exe|*.exe|bat|*.bat|所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                showProPathTextBox.Text = file;
                //MessageBox.Show("已选择文件:" + file, "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addProcessButton_Click(object sender, EventArgs e)
        {
            string[] paths = showProPathTextBox.Text.Split("\\");
            string fileName = paths.LastOrDefault();
            string[] fileNames = fileName.Split(".");
            //后缀名
            int length = fileNames.Length;
            ProcessInfo proInfo = new ProcessInfo();
            proInfo.PathFile = showProPathTextBox.Text;
            proInfo.ProcessName = fileName;
            proInfo.ProcessType = fileNames[length - 1];
            ThreadInfoDto.Current.ProcessInfos.Add(proInfo);
            ThreadInfoDto.Current.Save();
            string[] subitem = new[] { proInfo.ProcessName, proInfo.ProcessType, proInfo.PathFile };
            var item = new ListViewItem(subitem);
            item.Tag = proInfo;
            listView1.Items.Add(item);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            processStartInfo.FileName = "ThreadMan.exe";
            processStartInfo.UseShellExecute = false;
            //processStartInfo.CreateNoWindow = true;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            timeSpanTextBox.Text = ((ThreadInfoDto.Current.TimeSpan) / 60).ToString();
            ShuaXinListView();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteMenuItem1_Click(object sender, EventArgs e)
        {
            var items = listView1.SelectedItems;
            if (items.Count > 0)
            {
                foreach (ListViewItem item in items)
                {
                    var processInfo = item.Tag as ProcessInfo;
                    if (processInfo != null)
                    {
                        ThreadInfoDto.Current.ProcessInfos.RemoveAll(s => s.PathFile.Equals(processInfo.PathFile));
                    }

                    listView1.Items.Remove(item);
                }
                ThreadInfoDto.Current.Save();
            }

        }

        private void ShuaXinListView()
        {
            foreach (var pro in ThreadInfoDto.Current.ProcessInfos)
            {

                string[] subitem = new[] { pro.ProcessName, pro.ProcessType, pro.PathFile };
                var item = new ListViewItem(subitem);
                listView1.Items.Add(item);
            }
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                var serviceInternet = services.Where(e => e.ServiceType.ToString().Equals("Win32OwnProcess"));
                foreach (ServiceController service in serviceInternet)
                {
                    var serviceInfos = ThreadInfoDto.Current.ServiceInfos;
                    if (serviceInfos.Any())
                    {
                        var serviceInfo = serviceInfos.Where(s => s.ServiceName.Equals(service.ServiceName));
                        if (serviceInfo.Any())
                        {
                            string[] serviceArr = new string[] { service.ServiceName, service.DisplayName, "是" };
                            var item = new ListViewItem(serviceArr);
                            item.Checked = true;
                            listView2.Items.Add(item);
                            continue;
                        }
                    }
                    string[] serviceArr2 = new string[] { service.ServiceName, service.DisplayName, "否" };
                    var item2 = new ListViewItem(serviceArr2);
                    listView2.Items.Add(item2);
                }
            }
            catch
            {
                MessageBox.Show("服务查询失败!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void timeSpanButton_Click(object sender, EventArgs e)
        {
            var timeMiniter = timeSpanTextBox.Text.ToInt();
            ThreadInfoDto.Current.TimeSpan = timeMiniter * 60;
            ThreadInfoDto.Current.Save();
            MessageBox.Show("设置成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var items = listView2.Items;
            if (items.Count > 0)
            {
                foreach (ListViewItem item in items)
                {
                    if (item.Checked)
                    {
                        ServiceInfo serviceInfo = new ServiceInfo();
                        serviceInfo.ServiceName = item.Text;
                        ThreadInfoDto.Current.ServiceInfos.Add(serviceInfo);
                    }
                }
                ThreadInfoDto.Current.Save();
            }
            MessageBox.Show("添加成功！");
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timeSpanTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void showProPathTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //var sssss = Process.GetProcessesByName("ThreadMan");
            //if (sssss.Any())
            //{
            //    foreach (var process in sssss)
            //    {
            //        process.Kill();
            //    }
            //}
            var processOfThreadMans = Process.GetProcessesByName("ThreadMan");
            if (!processOfThreadMans.Any())
            {
                processStartInfo.Arguments = @"-run";
                Process.Start(processStartInfo);
            }
            //processId = StartProcess.Id; var StartProcess =
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //  processStartInfo.FileName = "ThreadMan.exe";
            //processStartInfo.Arguments = @"-step";
            // Process.GetCurrentProcess().StartInfo.FileName = "ThreadMan.exe";
            // Process.GetCurrentProcess().StartInfo.Arguments= @"-step";

            //var sssss = Process.GetProcessById(processId);
            //sssss.Kill();

            var processOfThreadMan = Process.GetProcessesByName("ThreadMan");
            if (processOfThreadMan.Any())
            {
                foreach (var process in processOfThreadMan)
                {
                    process.Kill();
                }
            }
         //   Process.Start("ThreadMan.exe", @"-stop");
            // Process.Start(processStartInfo);
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            //processStartInfo.FileName = "ThreadMan.exe";
            processStartInfo.Arguments = @"-i";
            Process.Start(processStartInfo);
        }

        private void UninstallButton_Click(object sender, EventArgs e)
        {
            //     processStartInfo.FileName = "ThreadMan.exe";
            processStartInfo.Arguments = @"-u";
            var p = Process.Start(processStartInfo);
            
        }
    }
}
