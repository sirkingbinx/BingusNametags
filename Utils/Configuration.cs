using BepInEx.Configuration;
using BingusNametags;
using BingusNametags.Plugins;
using BingusNametags.Tags;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Configuration
{
    public static TMP_FontAsset customFont;

    public static string accentColor = "#1c32ef";
    public static string GFriendsVerifiedColor = "#7fff7f";
    public static string GFriendsRecentColor = "#ffa0a0";
    public static string GFriendsFriendColor = "#cc7fe5";

    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    public static void UpdateConfig()
    {
        ConfigFile cfg = new ConfigFile(Path.Combine(AssemblyDirectory, "BingusNametags.cfg"), true);

        // built-in-stuff enabled
        if (cfg.Bind("Name", "Enabled", true, "Show the deafult nametag").Value)
            PluginManager.AddPluginUpdate(Name.UpdateNametag, 1f, false);

        Name.GFriends = cfg.Bind("Name", "GFriendsIntegration", true, "Use GorillaFriends to get colors for names").Value;

        if (cfg.Bind("Platform", "Enabled", true, "Show the platform tag").Value)
            PluginManager.AddPluginUpdate(Platform.UpdateNametag, 0.8f, true);

        Platform.UseOculusName = cfg.Bind("Platform", "OculusNamingScheme", true, "Use \"Oculus Rift\" and \"Oculus Quest\" instead of \"Oculus PCVR\" and \"Meta\".").Value;
    
        // Plugins
        PluginManager.PluginsEnabled = cfg.Bind("Plugins", "Enabled", true, "Allow plugins to load").Value;

        // Colors
        accentColor = cfg.Bind("Color", "AccentColor", "#1c32ef", "Sets the accent color for the non-essential nametag things").Value;

        GFriendsVerifiedColor = cfg.Bind("Color", "GFriends_VerifiedColor", "#7fff7f", "Hex code for verified players").Value;
        GFriendsRecentColor = cfg.Bind("Color", "GFriends_RecentColor", "#ffa0a0", "Hex code for recently played with players").Value;
        GFriendsFriendColor = cfg.Bind("Color", "GFriends_FriendColor", "#cc7fe5", "Hex code for friended players").Value;

        // Font loading
        if (File.Exists(Path.Combine(AssemblyDirectory, @"BingusNametagsFont.ttf")))
            customFont = TMP_FontAsset.CreateFontAsset(new Font(Path.Combine(AssemblyDirectory, @"BingusNametagsFont.ttf")));
    }
}
