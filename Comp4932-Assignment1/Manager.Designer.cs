﻿namespace Comp4932_Assignment1
{
    partial class Manager
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
            button1 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(61, 12);
            button1.Name = "button1";
            button1.Size = new Size(101, 51);
            button1.TabIndex = 0;
            button1.Text = "Play";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Manager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(232, 203);
            Controls.Add(button1);
            Name = "Manager";
            Text = "Manager";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
    }
}