using BepInEx;
using HarmonyLib;
using System;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.2.0")]
public class Main: BaseUnityPlugin
{
    public void Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { ["bingus.nametags"] = Info.Metadata.Version });
        new Harmony(Info.Metadata.GUID).PatchAll();
        Configuration.UpdateConfig();
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}
