namespace LogAppForms
{
    partial class PrintedBy
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
            this.label1 = new System.Windows.Forms.Label();
            this.printby = new LogAppForms.UnderlinedTextBox();
            this.register_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Printed By:";
            // 
            // printby
            // 
            this.printby.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(49)))), ((int)(((byte)(61)))));
            this.printby.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.printby.BorderFocusColor = System.Drawing.Color.HotPink;
            this.printby.BorderSize = 1;
            this.printby.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printby.IsUnderlinedStyle = true;
            this.printby.Location = new System.Drawing.Point(103, -10);
            this.printby.Multiline = false;
            this.printby.Name = "printby";
            this.printby.Padding = new System.Windows.Forms.Padding(7);
            this.printby.PasswordChar = false;
            this.printby.Size = new System.Drawing.Size(250, 39);
            this.printby.TabIndex = 21;
            this.printby.Texts = "";
            this.printby.UnderlinedStyle = true;
            this.printby.Load += new System.EventHandler(this.printby_Load);
            // 
            // register_button
            // 
            this.register_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_button.Location = new System.Drawing.Point(68, 42);
            this.register_button.Name = "register_button";
            this.register_button.Size = new System.Drawing.Size(242, 35);
            this.register_button.TabIndex = 22;
            this.register_button.Text = "Submit";
            this.register_button.UseVisualStyleBackColor = true;
            this.register_button.Click += new System.EventHandler(this.register_button_Click);
            // 
            // PrintedBy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(49)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(365, 89);
            this.Controls.Add(this.register_button);
            this.Controls.Add(this.printby);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrintedBy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintedBy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public UnderlinedTextBox printby;
        private System.Windows.Forms.Button register_button;
    }
}