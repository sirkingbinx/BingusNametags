using BepInEx;
using BingusNametags.Tags;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TMPro;
using UnityEngine;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.0.0")]
public class Main: BaseUnityPlugin
{
    private const string GorillaInfoURL = "https://raw.githubusercontent.com/HanSolo1000Falcon/GorillaInfo/main/";

    public static Dictionary<string, string> KnownMods = new Dictionary<string, string>();
    public static Dictionary<string, string> KnownCheats = new Dictionary<string, string>();

    public void Awake()
    {
        new Harmony(Info.Metadata.GUID).PatchAll();

        bool NametagsE = Config.Bind("Name", "Enabled", true, "Shows name of players").Value;
        bool PlatformE = Config.Bind("Platform", "Enabled", true, "Checks platform of players").Value;
        bool ModCheckE = Config.Bind("ModList", "Enabled", true, "Shows lists of (known) mods for players (if they have any)").Value;

        Platform.UseOculusName = Config.Bind("Platform", "UseOculusName", false, "Replaces \"Oculus\" and \"Meta\" with \"Oculus Rift\" and \"Oculus Quest\".").Value;
        Name.GFriends = Config.Bind("Name", "GFriendsIntegration", true, "Use GorillaFriends to get colors for names").Value;

        if (NametagsE)
            UpdateTags += Name.Update;
        
        if (PlatformE)
            UpdateTags += Platform.Update;
        
        if (ModCheckE) {
            UpdateTags += ModList.Update;

            //    This code is from GorillaNametags by HanSolo
            //    Used just for checking mods
            //    https://github.com/HanSolo1000Falcon/GorillaNametags

            //    Copyright (c) 2025 HanSolo1000Falcon
            //
            //    Permission is hereby granted, free of charge, to any person obtaining a copy
            //    of this software and associated documentation files (the "Software"), to deal
            //    in the Software without restriction, including without limitation the rights
            //    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
            //    copies of the Software, and to permit persons to whom the Software is
            //    furnished to do so, subject to the following conditions:
            //
            //    The above copyright notice and this permission notice shall be included in all
            //    copies or substantial portions of the Software.
            //
            //    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            //    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            //    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            //    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            //    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
            //    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
            //    SOFTWARE.

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage knownModsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownMods.txt").Result;
                HttpResponseMessage knownCheatsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownCheats.txt").Result;
                
                knownModsResponse.EnsureSuccessStatusCode();
                knownCheatsResponse.EnsureSuccessStatusCode();

                using (Stream stream = knownModsResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    KnownMods = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());

                using (Stream stream = knownCheatsResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    KnownCheats = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
            }
        }
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}