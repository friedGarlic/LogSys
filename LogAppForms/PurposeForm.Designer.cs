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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.userControl11 = new LogAppForms.UserControl1();
            this.userControl21 = new LogAppForms.UserControl2();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(77, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Purpose";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(60, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(41, 136);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(154, 24);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Laboratory Usage";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(41, 183);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(165, 24);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Equipment Lending";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // userControl11
            // 
            this.userControl11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControl11.Location = new System.Drawing.Point(245, 12);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(560, 357);
            this.userControl11.TabIndex = 6;
            // 
            // userControl21
            // 
            this.userControl21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControl21.Location = new System.Drawing.Point(245, 12);
            this.userControl21.Name = "userControl21";
            this.userControl21.Size = new System.Drawing.Size(560, 357);
            this.userControl21.TabIndex = 7;
            // 
            // PurposeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 381);
            this.Controls.Add(this.userControl21);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "PurposeForm";
            this.Text = "PurposeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private UserControl1 userControl11;
        private UserControl2 userControl21;
    }
}