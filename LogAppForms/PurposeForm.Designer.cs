namespace LogAppForms
{
    partial class PurposeForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.userControl21 = new LogAppForms.UserControl2();
            this.toggle_Switch1 = new LogAppForms.Toggle_Switch();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toggle_Switch2 = new LogAppForms.Toggle_Switch();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(56, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // userControl21
            // 
            this.userControl21.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.userControl21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControl21.Location = new System.Drawing.Point(214, 70);
            this.userControl21.Name = "userControl21";
            this.userControl21.Size = new System.Drawing.Size(347, 363);
            this.userControl21.TabIndex = 7;
            // 
            // toggle_Switch1
            // 
            this.toggle_Switch1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.toggle_Switch1.Location = new System.Drawing.Point(65, 154);
            this.toggle_Switch1.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggle_Switch1.Name = "toggle_Switch1";
            this.toggle_Switch1.OffBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.toggle_Switch1.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggle_Switch1.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.toggle_Switch1.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggle_Switch1.Size = new System.Drawing.Size(70, 27);
            this.toggle_Switch1.TabIndex = 8;
            this.toggle_Switch1.UseVisualStyleBackColor = false;
            this.toggle_Switch1.CheckedChanged += new System.EventHandler(this.toggle_Switch1_CheckedChanged);
            this.toggle_Switch1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.toggle_Switch1_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Timed-In";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 64);
            this.panel1.TabIndex = 10;
            // 
            // toggle_Switch2
            // 
            this.toggle_Switch2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.toggle_Switch2.Location = new System.Drawing.Point(65, 235);
            this.toggle_Switch2.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggle_Switch2.Name = "toggle_Switch2";
            this.toggle_Switch2.OffBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.toggle_Switch2.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggle_Switch2.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.toggle_Switch2.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggle_Switch2.Size = new System.Drawing.Size(70, 27);
            this.toggle_Switch2.TabIndex = 11;
            this.toggle_Switch2.UseVisualStyleBackColor = false;
            this.toggle_Switch2.CheckedChanged += new System.EventHandler(this.toggle_Switch2_CheckedChanged);
            // 
            // PurposeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 445);
            this.Controls.Add(this.toggle_Switch2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toggle_Switch1);
            this.Controls.Add(this.userControl21);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "PurposeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PurposeForm";
            this.Load += new System.EventHandler(this.PurposeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private UserControl2 userControl21;
        private Toggle_Switch toggle_Switch1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private Toggle_Switch toggle_Switch2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}