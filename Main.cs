using BepInEx;
using HarmonyLib;
using System;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.2.0")]
public class Main: BaseUnityPlugin
{
    public static Version Version = new Version("1.0.0");
    
    public void Awake()
    {
        Version = Info.Metadata.Version;
        new Harmony(Info.Metadata.GUID).PatchAll();
        Configuration.UpdateConfig();
    }

    public static event Action UpdateTags = delegate { };
    
    private int _run = 0;
    private void Update()
    {
        // reduce amount of frames we are updating because we really don't need all that
        _run++;
        if (_run < 4)
            return;

        UpdateTags();
        _run = 0;
    }
}
