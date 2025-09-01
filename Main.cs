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
    public static FontStyles activeTMPFontStyle = FontStyles.Bold;
    private const string GorillaInfoURL = "https://raw.githubusercontent.com/HanSolo1000Falcon/GorillaInfo/main/";

    public static Dictionary<string, string> KnownMods = new Dictionary<string, string>();
    public static Dictionary<string, string> KnownCheats = new Dictionary<string, string>();
    public static List<string> KnownPeople = new List<string>();

    public void Awake()
    {
        new Harmony(Info.Metadata.GUID).PatchAll();

        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage knownModsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownMods.txt").Result;
            HttpResponseMessage knownCheatsResponse = httpClient.GetAsync(GorillaInfoURL + "KnownCheats.txt").Result;
            HttpResponseMessage knownPeopleResponse = httpClient.GetAsync("https://raw.githubusercontent.com/Not-A-Bird-07/GorillaFriends/refs/heads/main/gorillas.verified").Result;

            knownModsResponse.EnsureSuccessStatusCode();
            knownCheatsResponse.EnsureSuccessStatusCode();
            knownPeopleResponse.EnsureSuccessStatusCode();

            using (Stream stream = knownModsResponse.Content.ReadAsStreamAsync().Result)
            using (StreamReader reader = new StreamReader(stream))
                KnownMods = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());

            using (Stream stream = knownCheatsResponse.Content.ReadAsStreamAsync().Result)
            using (StreamReader reader = new StreamReader(stream))
                KnownCheats = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());

            using (Stream stream = knownPeopleResponse.Content.ReadAsStreamAsync().Result)
            using (StreamReader reader = new StreamReader(stream)) {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    KnownPeople.Add(line);
                }
            }
        }
    }

    public void Update()
    {
        Name.Update();
        Platform.Update();
    }

    public class TMPLookAt : MonoBehaviour
    {
        private void Update()
        {
            if (Camera.main != null && text != null)
            {
                Vector3 forward = Camera.main.transform.forward;

                forward.y = 0f;
                forward.Normalize();

                transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        public VRRig who;
        public TextMeshPro text;
    }
}