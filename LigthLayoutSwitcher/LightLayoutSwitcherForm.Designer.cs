namespace LigthLayoutSwitcher
{
    partial class LightLayoutSwitcherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightLayoutSwitcherForm));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SwitchKeyText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ConvertKeyText = new System.Windows.Forms.TextBox();
            this.SwitchRegisterText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Ok_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.Startup_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            resources.ApplyResources(this.TrayIcon, "TrayIcon");
            this.TrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // SwitchKeyText
            // 
            resources.ApplyResources(this.SwitchKeyText, "SwitchKeyText");
            this.SwitchKeyText.Name = "SwitchKeyText";
            this.SwitchKeyText.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ConvertKeyText
            // 
            resources.ApplyResources(this.ConvertKeyText, "ConvertKeyText");
            this.ConvertKeyText.Name = "ConvertKeyText";
            this.ConvertKeyText.ReadOnly = true;
            // 
            // SwitchRegisterText
            // 
            resources.ApplyResources(this.SwitchRegisterText, "SwitchRegisterText");
            this.SwitchRegisterText.Name = "SwitchRegisterText";
            this.SwitchRegisterText.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Ok_button
            // 
            resources.ApplyResources(this.Ok_button, "Ok_button");
            this.Ok_button.Name = "Ok_button";
            this.Ok_button.UseVisualStyleBackColor = true;
            this.Ok_button.Click += new System.EventHandler(this.Ok_button_Click);
            // 
            // Cancel_button
            // 
            resources.ApplyResources(this.Cancel_button, "Cancel_button");
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // Startup_checkBox
            // 
            resources.ApplyResources(this.Startup_checkBox, "Startup_checkBox");
            this.Startup_checkBox.Name = "Startup_checkBox";
            this.Startup_checkBox.UseVisualStyleBackColor = true;
            // 
            // LightLayoutSwitcherForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Startup_checkBox);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Ok_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SwitchRegisterText);
            this.Controls.Add(this.ConvertKeyText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SwitchKeyText);
            this.MaximizeBox = false;
            this.Name = "LightLayoutSwitcherForm";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Activated += new System.EventHandler(this.LightLayoutSwitcherForm_Activated);
            this.Deactivate += new System.EventHandler(this.LightLayoutSwitcherForm_Deactivate);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.TextBox SwitchKeyText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ConvertKeyText;
        private System.Windows.Forms.TextBox SwitchRegisterText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Ok_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.CheckBox Startup_checkBox;
    }
}

