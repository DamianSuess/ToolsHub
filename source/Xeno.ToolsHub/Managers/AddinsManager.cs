﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    AddinManager.cs
 * Description:
 *  Manager for system wide add-ins
 */

using System;
using System.Collections.Generic;
using Mono.Addins;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.Managers
{
  public class AddinsManager
  {
    //private static readonly Log _log = myLogger.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    ///   Key = TypeExtensionNode.Id
    /// </summary>
    private Dictionary<string, UtilityAddin> _appAddins;

    public AddinsManager() : this("")
    {
    }

    /// <summary>Constructor providing alternative add-in directory</summary>
    /// <param name="configDir">Add-ins configuration directory</param>
    public AddinsManager(string configDir)
    {
      _appAddins = new Dictionary<string, UtilityAddin>();

      InitAddins();
    }

    public event EventHandler OnApplicationAddinListChanged;

    public PreferenceAddin[] GetPreferenceAddins()
    {
      //TODO: Currently not in use
      PreferenceAddin[] addins;

      try
      {
        addins = (PreferenceAddin[])Mono.Addins.AddinManager.GetExtensionObjects(
          ExtensionPaths.PreferencePath, typeof(PreferenceAddin));
      }
      catch (Exception ex)
      {
        Log.Warn($"No perferenceAddins found '{ex.Message}'");
        addins = new PreferenceAddin[0];
      }

      return addins;
    }

    /// <summary>Load utility add-ins</summary>
    public void LoadUtilityAddins()
    {
      Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPaths.UtilityPath);
      foreach (Mono.Addins.ExtensionNode node in nodes)
      {
        Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;

        try
        {
          UtilityAddin utility = typeNode.CreateInstance() as UtilityAddin;

          LoadUtilityAddin(utility, typeNode.Id, typeNode.TypeName);
        }
        catch (Exception ex)
        {
          Log.Error("Couldn't create UtilityAddin: " + ex.Message);
        }
      }
    }

    /// <summary>
    ///   Load UtilityAddin and store in memory
    /// </summary>
    /// <param name="addin"></param>
    private void LoadUtilityAddin(UtilityAddin addin, string addinId, string addinTypeName)
    {
      try
      {
        addin.Initialize();
      }
      catch (Exception ex)
      {
        Log.Error($"Error while attempting to initialize UtilityAddin, Id: '{addinId}', TypeName: '{addinTypeName}': {ex.Message}");
      }

      // TODO: Add add-in to managed list for later callings
      if (addin.IsInitialized)
        addin.Execute();
    }

    private void InitAddins()
    {
      Log.Info("Initializing Mono.Addins");

      Mono.Addins.AddinManager.AddinLoaded += OnAddinLoaded;
      Mono.Addins.AddinManager.AddinUnloaded += OnAddinUnloaded;

      Mono.Addins.AddinManager.Initialize(".");

      // Rebuild registry when debugging
      if (Helpers.IsDebugging)
        Mono.Addins.AddinManager.Registry.Rebuild(null);
      else
        Mono.Addins.AddinManager.Registry.Update();

      try
      {
        // EventHandlers for ExtensionNodes
        Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPaths.OnStartupPath, OnStartupAddins_ExtensionHandler);
        Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPaths.SystemTrayPath, OnUtilityAddins_ExtensionHandler);
        Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPaths.UtilityPath, OnUtilityAddins_ExtensionHandler);
      }
      catch (Exception ex)
      {
        Log.Error("Could not register one or more ExtensionPoints. Possibly could not find XML manifest.");
        Log.Error("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);

        throw new Exception("Unable to add extension handler.", ex);
      }
    }

    private void OnAddinLoaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Mono.Addins.Addin addin = Mono.Addins.AddinManager.Registry.GetAddin(args.AddinId);
      Log.Debug($"=============================");
      Log.Debug($"OnAddinLoaded: {args.AddinId}");
      Log.Debug($"         Name: '{addin.Name}'");
      Log.Debug($"  Description: '{addin.Description.Description}'");
      Log.Debug($"    Namespace: '{addin.Namespace}'");
      Log.Debug($"      Enabled: '{addin.Enabled}'");
      Log.Debug($"         File: '{addin.AddinFile}'");
      Log.Debug("= = = = = = = = = = = = =");
    }

    private void OnAddinUnloaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Log.Debug($"Add-in Id: {args.AddinId}");
    }

    private void OnDisableAddin(string extensionNodeId)
    {
      throw new NotImplementedException();
    }

    private void OnStartupAddins_ExtensionHandler(object sender, Mono.Addins.ExtensionNodeEventArgs args)
    {
      Log.Debug("Entering");

      Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
      PrintInfo(ExtensionPaths.OnStartupPath, args, extNode);

      // Execute via class interface definition of extension path
      // IOnStartupExtension ext = (IOnStartupExtension)args.ExtensionObject;
      IOnStartupExtension addin = extNode.GetInstance(typeof(IOnStartupExtension)) as IOnStartupExtension;
      addin.Execute();

      // Push event changed out to listeners
      OnApplicationAddinListChanged?.Invoke(sender, args);
    }

    private void OnUtilityAddins_ExtensionHandler(object sender, ExtensionNodeEventArgs args)
    {
      if (args.Change == Mono.Addins.ExtensionChange.Add)
        OnUtilityAddinEnable(args);
      else
        OnUtilityAddinDisable(args);
    }

    private void OnUtilityAddinDisable(Mono.Addins.ExtensionNodeEventArgs args)
    {
      Mono.Addins.TypeExtensionNode node = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
      try
      {
        OnDisableAddin(node.Id);
      }
      catch (Exception ex)
      {
        Log.Error("Error unloading add-in: " + ex.Message);
      }
    }

    private void OnUtilityAddinEnable(Mono.Addins.ExtensionNodeEventArgs args)
    {
      Log.Debug("Entering (not used)");

      // Cycle through and attach to in-memory
    }

    private void OnUtilityHandle_Example()
    {
      //Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
      //PrintInfo(ExtensionPaths.UtilityPath, args, extNode);
      //
      //UtilityAddin addin;
      //if (args.Change == Mono.Addins.ExtensionChange.Add)
      //{
      //  // Load add-in
      //  addin = extNode.GetInstance(typeof(UtilityAddin)) as UtilityAddin;
      //  if (addin != null)
      //  {
      //    try
      //    {
      //      addin.Initialize();
      //      _appAddins[extNode.Id] = addin;
      //      addin.Execute();
      //    }
      //    catch (Exception loadEx)
      //    {
      //      Log.Error($"Issue initializing add-in, '{addin.GetType().ToString()}' " +
      //                $"with exception: {loadEx.Message}{Environment.NewLine}StackTrace: {loadEx.StackTrace}");
      //    }
      //  }
      //}
      //else
      //{
      //  // Remove add-in from app cache
      //  if (_appAddins.ContainsKey(extNode.Id))
      //  {
      //    addin = _appAddins[extNode.Id];
      //    try
      //    {
      //      addin.Shutdown();
      //    }
      //    catch (Exception unloadEx)
      //    {
      //      Log.Error($"Issue shutting down add-in, '{addin.GetType().ToString()}' " +
      //                $"with exception: {unloadEx.Message}{Environment.NewLine}StackTrace: {unloadEx.StackTrace}");
      //    }
      //    finally
      //    {
      //      _appAddins.Remove(extNode.Id);
      //    }
      //  }
      //
      //// Push event changed out to listeners
      //OnApplicationAddinListChanged?.Invoke(sender, args);
      //}
    }

    private void PrintInfo(string extPoint, ExtensionNodeEventArgs args, Mono.Addins.TypeExtensionNode extNode)
    {
      Log.Debug("###########################");
      Log.Debug($"'{extPoint}'");
      Log.Debug($"  Id      - '{args.ExtensionNode.Id}'");
      Log.Debug($"  Path    - '{args.Path}'");
      Log.Debug($"  Node    - '{args.ExtensionNode}'");
      Log.Debug($"  Object  - '{args.ExtensionObject}'");
      Log.Debug($"  Changed - '{args.Change.ToString()}'");
      Log.Debug("   --[ ExtensionNode ]------");
      Log.Debug($"  Id      - '{extNode.Id}'");
      Log.Debug($"  ToString- '{extNode.ToString()}'");
      Log.Debug($"  TypeName- '{extNode.TypeName}'");
    }
  }
}
