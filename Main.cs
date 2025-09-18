using BepInEx;
using BingusNametags.Plugins;
using BingusNametags.Tags;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using TMPro;
using UnityEngine;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.0.0")]
public class Main: BaseUnityPlugin
{
    private const string GorillaInfoURL = "https://raw.githubusercontent.com/HanSolo1000Falcon/GorillaInfo/main/";

    public static Dictionary<string, string> KnownMods = new Dictionary<string, string>();
    public static Color accentColor = Color.blue;
    public static TMP_FontAsset customFont;

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

    public void Awake()
    {
        new Harmony(Info.Metadata.GUID).PatchAll();

        // enable things
        bool NametagsE = Config.Bind("Name", "Enabled", true, "Shows name of players").Value;
        bool PlatformE = Config.Bind("Platform", "Enabled", true, "Checks platform of players").Value;

        // specific settings
        Platform.UseOculusName = Config.Bind("Platform", "UseOculusName", false, "Replaces \"Rift PCVR\" and \"Meta\" with \"Oculus Rift\" and \"Oculus Quest\".").Value;
        Name.GFriends = Config.Bind("Name", "GFriendsIntegration", true, "Use GorillaFriends to get colors for names").Value;

        // color
        accentColor = Config.Bind("Global", "AccentColor", Color.blue, "Sets the accent color for the non-essential nametag things").Value;

        // load plugins
        PluginManager.PluginsEnabled = Config.Bind("Plugins", "Enabled", true, "Enable plugin support for mod developers to add additional functionality to BingusNametags").Value;

        string fontPath = Path.Combine(AssemblyDirectory, @"BingusNametagsFont.ttf");

        if (File.Exists(fontPath))
            customFont = TMP_FontAsset.CreateFontAsset(new Font(fontPath));

        if (NametagsE)
            UpdateTags += Name.Update;
        
        if (PlatformE)
            UpdateTags += Platform.Update;
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}