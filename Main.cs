using BepInEx;
using HarmonyLib;
using System;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.2.0")]
public class Main: BaseUnityPlugin
{
    public void Awake()
    {
        new Harmony(Info.Metadata.GUID).PatchAll();
        Configuration.UpdateConfig();
    }

    public static event Action UpdateTags = delegate { };
    public void Update() => UpdateTags();
}
