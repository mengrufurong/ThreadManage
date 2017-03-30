using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadMan;

namespace ThreadConfigForm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                    var serviceInfos=ThreadInfoDto.Current.ServiceInfos;
                    if (serviceInfos.Any())
                    {
                       var serviceInfo= serviceInfos.Where(s => s.ServiceName.Equals(service.ServiceName));
                        if (serviceInfo.Any())
                        {
                            string[] serviceArr = new string[] { service.ServiceName, service.DisplayName,"是" };
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

        private void button1_Click(object sender, EventArgs e)
        {
            var items=listView2.Items;
            if (items.Count>0)
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
            this.FindForm().Close();
        }
    }
}
