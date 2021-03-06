﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleXmlPerferencePage.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.SampleXmlAddin.Views
{
  partial class PerferencePage
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
      this.SampleTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.lblIsModified = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // SampleTextBox
      // 
      this.SampleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SampleTextBox.Location = new System.Drawing.Point(12, 80);
      this.SampleTextBox.Multiline = true;
      this.SampleTextBox.Name = "SampleTextBox";
      this.SampleTextBox.Size = new System.Drawing.Size(296, 101);
      this.SampleTextBox.TabIndex = 5;
      this.SampleTextBox.TextChanged += new System.EventHandler(this.SampleTextBox_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 64);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(58, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "IsModified:";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(107, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "Fill-out Demo";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // lblIsModified
      // 
      this.lblIsModified.AutoSize = true;
      this.lblIsModified.Location = new System.Drawing.Point(76, 64);
      this.lblIsModified.Name = "lblIsModified";
      this.lblIsModified.Size = new System.Drawing.Size(32, 13);
      this.lblIsModified.TabIndex = 6;
      this.lblIsModified.Text = "False";
      // 
      // PerferencePage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(320, 193);
      this.Controls.Add(this.lblIsModified);
      this.Controls.Add(this.SampleTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.Name = "PerferencePage";
      this.Text = "Sample Add-in (External XML)";
      this.Load += new System.EventHandler(this.SampleXmlPerferencePage_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox SampleTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label lblIsModified;
  }
}