﻿using System;
using System.Windows.Forms;
using v2rayN.Handler;
using v2rayN.Mode;

namespace v2rayN.Forms
{
    public partial class AddServer4Form : BaseForm
    {
        public int EditIndex { get; set; }
        VmessItem vmessItem = null;

        public AddServer4Form()
        {
            InitializeComponent();
        }

        private void AddServer4Form_Load(object sender, EventArgs e)
        {
            if (EditIndex >= 0)
            {
                vmessItem = config.vmess[EditIndex];
                BindingServer();
            }
            else
            {
                vmessItem = new VmessItem();
                ClearServer();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindingServer()
        {
            txtAddress.Text = vmessItem.address;
            txtPort.Text = vmessItem.port.ToString();
            txtRemarks.Text = vmessItem.remarks;
        }


        /// <summary>
        /// 清除设置
        /// </summary>
        private void ClearServer()
        {
            txtAddress.Text = "";
            txtPort.Text = "";
            txtRemarks.Text = "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text;
            string port = txtPort.Text;
            string remarks = txtRemarks.Text;

            if (Utils.IsNullOrEmpty(address))
            {
                UI.Show(UIRes.I18N("FillServerAddress"));
                return;
            }
            if (Utils.IsNullOrEmpty(port) || !Utils.IsNumberic(port))
            {
                UI.Show(UIRes.I18N("FillCorrectServerPort"));
                return;
            }

            vmessItem.address = address;
            vmessItem.port = Utils.ToInt(port);
            vmessItem.remarks = remarks;

            if (ConfigHandler.AddSocksServer(ref config, vmessItem, EditIndex) == 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                UI.Show(UIRes.I18N("OperationFailed"));
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        #region 导入配置
         
        /// <summary>
        /// 从剪贴板导入URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemImportClipboard_Click(object sender, EventArgs e)
        {
            ImportConfig();
        }

        private void ImportConfig()
        {
            ClearServer();

            string msg;
            VmessItem vmessItem = V2rayConfigHandler.ImportFromClipboardConfig(Utils.GetClipboardData(), out msg);
            if (vmessItem == null)
            {
                UI.Show(msg);
                return;
            }

            txtAddress.Text = vmessItem.address;
            txtPort.Text = vmessItem.port.ToString();
            txtRemarks.Text = vmessItem.remarks;
        }        

        #endregion
         

    }
}
