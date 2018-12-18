﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCrypt.cs
 * Description:
 *  Entry point to VeraCrypt add-in
 */

using System;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public class VeraCrypt : UtilityAddin
  {
    private bool _initialized = false;

    public override bool IsInitialized => _initialized;

    public static void SettingSave(string setting, string value)
    {
      Xeno.ToolsHub.Config.Settings.AddinSettings.Save("VeraCrypt", setting, value);
    }

    public static string SettingLoad(string setting, string defaultValue)
    {
      return Xeno.ToolsHub.Config.Settings.AddinSettings.Load("VeraCrypt", setting, defaultValue);
    }

    public override void Execute()
    {
      Log.Debug("VeraCrypt add-in initializing");
      _initialized = true;

      //TODO: Auto-mount virtual drives

      throw new NotImplementedException();
    }

    public override void Shutdown()
    {
      Log.Debug("VeraCrypt add-in shutting down");

      // Auto-dismount virtual drives

      throw new NotImplementedException();
    }
  }
}
