using BepInEx;
using HarmonyLib;
using System;

namespace BingusNametags
{
    [BepInDependency("net.rusjj.gorillafriends", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("bingus.nametags", "BingusNametags", "1.2.0")]
    public class Main : BaseUnityPlugin
    {
        public static Version Version = new Version("1.0.0");

        public void Awake()
        {
            Version = Info.Metadata.Version;

            GorillaTagger.OnPlayerSpawned(delegate
            {
                Plugins.Plugins.PluginStart();
                Configuration.UpdateConfig();
            });

            new Harmony(Info.Metadata.GUID).PatchAll();
        }

        public static event Action UpdateTags = delegate { };

        private int _run;

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
}