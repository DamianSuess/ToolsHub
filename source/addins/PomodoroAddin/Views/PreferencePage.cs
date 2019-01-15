﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferencePage.cs
 * Description:
 *  Preferences page
 */

namespace PomodoroAddin.Views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services;
  
  public partial class PreferencePage : Form, IPreferencePageForm
  {
    public PreferencePage()
    {
      InitializeComponent();

      TxtDuration.Text = SettingsService.GetInt(Constants.AddinId, Constants.KeyDuration, 25).ToString();
      TxtBreakShort.Text = SettingsService.GetInt(Constants.AddinId, Constants.KeyShortBreak, 5).ToString();
      TxtBreakLong.Text = SettingsService.GetInt(Constants.AddinId, Constants.KeyLongBreak, 10).ToString();

      IsModified = false;
    }

    public bool IsModified { get; set; }

    public bool OnSave()
    {
      SettingsService.SetValue(Constants.AddinId, Constants.KeyDuration, TxtDuration.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyShortBreak, TxtBreakShort.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyLongBreak, TxtBreakLong.Text);

      return true;
    }

    private void PreferencePage_Load(object sender, EventArgs e)
    {
    }

    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) &&
          !char.IsDigit(e.KeyChar) &&
          (e.KeyChar != '.'))
        e.Handled = true;
      else
        IsModified = true;

      // Only allow one decimal point
      if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
        e.Handled = true;
    }
  }
}