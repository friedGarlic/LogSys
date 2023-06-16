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
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.userControl21 = new LogAppForms.UserControl2();
            this.toggle_Switch1 = new LogAppForms.Toggle_Switch();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(54, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(30, 232);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(165, 24);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Equipment Lending";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // userControl21
            // 
            this.userControl21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControl21.Location = new System.Drawing.Point(233, 12);
            this.userControl21.Name = "userControl21";
            this.userControl21.Size = new System.Drawing.Size(347, 357);
            this.userControl21.TabIndex = 7;
            // 
            // toggle_Switch1
            // 
            this.toggle_Switch1.Location = new System.Drawing.Point(65, 154);
            this.toggle_Switch1.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggle_Switch1.Name = "toggle_Switch1";
            this.toggle_Switch1.OffBackColor = System.Drawing.Color.Gray;
            this.toggle_Switch1.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggle_Switch1.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.toggle_Switch1.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggle_Switch1.Size = new System.Drawing.Size(70, 27);
            this.toggle_Switch1.TabIndex = 8;
            this.toggle_Switch1.UseVisualStyleBackColor = true;
            this.toggle_Switch1.CheckedChanged += new System.EventHandler(this.toggle_Switch1_CheckedChanged);
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
            // PurposeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 381);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toggle_Switch1);
            this.Controls.Add(this.userControl21);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
        private System.Windows.Forms.RadioButton radioButton2;
        private UserControl2 userControl21;
        private Toggle_Switch toggle_Switch1;
        private System.Windows.Forms.Label label2;
    }
}