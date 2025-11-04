using BepInEx;
using BingusNametags.Plugins;
using BingusNametags.Tags;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

[BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("bingus.nametags", "BingusNametags", "1.0.0")]
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
