using BepInEx.Configuration;
using BingusNametags.Plugins;
using BingusNametags.Tags;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Configuration
{
    public static string AccentColor = "#1c32ef";
    public static string GFriendsVerifiedColor = "#7fff7f";
    public static string GFriendsRecentColor = "#ffa0a0";
    public static string GFriendsFriendColor = "#cc7fe5";
    public static TMP_FontAsset CustomFont;

    private static string AssemblyDirectory
    {
        get
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            
            return Path.GetDirectoryName(path);
        }
    }

    public static void UpdateConfig()
    {
        var cfg = new ConfigFile(Path.Combine(AssemblyDirectory, "BingusNametags.cfg"), true);

        Name.Instance.Enabled = (cfg.Bind("Name", "Enabled", true, "A default nametag containing the name of the player.").Value);
        Name.GFriends = cfg.Bind("Name", "GFriendsIntegration", true, "Use GorillaFriends to get colors for names").Value;
        
        Platform.Instance.Enabled = (cfg.Bind("Platform", "Enabled", true, "A tag that displays the user's current platform.").Value);
        Platform.UseOculusName = cfg.Bind("Platform", "OculusNamingScheme", true, "Use \"Oculus Rift\" and \"Oculus Quest\" instead of \"Oculus PCVR\" and \"Meta\".").Value;

        // Colors
        AccentColor = cfg.Bind("Color", "AccentColor", "#1c32ef", "Sets the accent color for the non-essential nametag things").Value;

        GFriendsVerifiedColor = cfg.Bind("Color", "GFriends_VerifiedColor", "#7fff7f", "Hex code for verified players").Value;
        GFriendsRecentColor = cfg.Bind("Color", "GFriends_RecentColor", "#ffa0a0", "Hex code for recently played with players").Value;
        GFriendsFriendColor = cfg.Bind("Color", "GFriends_FriendColor", "#cc7fe5", "Hex code for friended players").Value;

        // Font loading
        string fontFile = Directory.EnumerateFiles(AssemblyDirectory, "*.ttf", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();

        if (fontFile != null) // font found
            CustomFont = TMP_FontAsset.CreateFontAsset(new Font(fontFile));
    }
}
