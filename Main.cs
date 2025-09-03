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
    // public static List<string> KnownPeople = new List<string>();

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

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage knownModsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownMods.txt").Result;
                HttpResponseMessage knownCheatsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownCheats.txt").Result;
                // HttpResponseMessage knownPeopleResponse = httpClient.GetAsync("https://raw.githubusercontent.com/Not-A-Bird-07/GorillaFriends/refs/heads/main/gorillas.verified").Result;

                knownModsResponse.EnsureSuccessStatusCode();
                knownCheatsResponse.EnsureSuccessStatusCode();
                // knownPeopleResponse.EnsureSuccessStatusCode();

                using (Stream stream = knownModsResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    KnownMods = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());

                using (Stream stream = knownCheatsResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    KnownCheats = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                /*
                using (Stream stream = knownPeopleResponse.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream)) {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        KnownPeople.Add(line);
                    }
                }
                */
            }
        }
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}