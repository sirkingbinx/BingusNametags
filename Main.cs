using BepInEx;
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
        bool ModCheckE = Config.Bind("ModList", "Enabled", true, "Shows lists of (known) mods for players (if they have any)").Value;

        // specific settings
        Platform.UseOculusName = Config.Bind("Platform", "UseOculusName", false, "Replaces \"Oculus\" and \"Meta\" with \"Oculus Rift\" and \"Oculus Quest\".").Value;
        Name.GFriends = Config.Bind("Name", "GFriendsIntegration", true, "Use GorillaFriends to get colors for names").Value;

        // color
        accentColor = Config.Bind("Global", "AccentColor", Color.blue, "Sets the accent color for the non-essential nametag things").Value;

        string fontPath = Path.Combine(AssemblyDirectory, @"BingusNametagsFont.ttf");

        if (File.Exists(fontPath))
            customFont = TMP_FontAsset.CreateFontAsset(new Font(fontPath));

        if (NametagsE)
            UpdateTags += Name.Update;
        
        if (PlatformE)
            UpdateTags += Platform.Update;
        
        if (ModCheckE) {
            UpdateTags += ModList.Update;

            //    This code is from GorillaNametags by HanSolo
            //    Used just for checking mods
            //    https://github.com/HanSolo1000Falcon/GorillaNametags

            //    check LICENSE.txt for license stuff

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage knownModsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownMods.txt").Result;
                
                knownModsResponse.EnsureSuccessStatusCode();

                using (Stream stream = knownModsResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    KnownMods = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
            }
        }
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}