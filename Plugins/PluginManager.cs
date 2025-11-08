using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Plugins
{
    public class PluginManager
    {
        public static bool PluginsEnabled = true;
        internal static List<BingusNametagsPlugin> loadedPlugins = new List<BingusNametagsPlugin>();

        public static void AddPluginUpdate(Action<TextMeshPro, VRRig> updateFunction, string name = "Plugin Name", float nametagOffset = 0f, bool useAccentColor = true)
        {
            if (!PluginsEnabled) throw new Exception("Plugins are not currently enabled.");

            BingusNametagsPlugin assignedPluginManager = new BingusNametagsPlugin();
            loadedPlugins.Add(assignedPluginManager);

            assignedPluginManager.UpdateTag += updateFunction;
            assignedPluginManager.TagOffset = nametagOffset != 0f ? nametagOffset : (1.2f + (loadedPlugins.IndexOf(assignedPluginManager) * 0.2f));
            assignedPluginManager.UseAccent = useAccentColor;
            assignedPluginManager.Name = name;

            Main.UpdateTags += assignedPluginManager.Update;
        }
    }

    internal class BingusNametagsPlugin
    {
        internal bool UseAccent = true;
        internal bool Enabled = true;
        internal string Name = "Plugin";
        internal event Action<TextMeshPro, VRRig> UpdateTag = delegate { };
        internal float TagOffset = 1.2f;
        
        private Dictionary<VRRig, GameObject> _tags = new Dictionary<VRRig, GameObject>();

        internal void Update()
        {
            if (GorillaParent.hasInstance)
            {
                List<VRRig> list = new List<VRRig>();

                foreach (KeyValuePair<VRRig, GameObject> keyValuePair in _tags)
                {
                    if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key) || !Enabled )
                    {
                        GameObject.Destroy(keyValuePair.Value);
                        list.Add(keyValuePair.Key);
                    }
                }

                foreach (var key in list)
                    _tags.Remove(key);

                foreach (var rig in GorillaParent.instance.vrrigs)
                    if (rig != GorillaTagger.Instance.offlineVRRig)
                        UpdateTagLocal(rig);
            }
        }

        private void UpdateTagLocal(VRRig rig)
        {
            if (!_tags.ContainsKey(rig))
                _tags[rig] = NametagCreator.CreateTag(rig, TagOffset, "PluginTextObject");

            TextMeshPro component = _tags[rig].GetComponent<TextMeshPro>();

            UpdateTag(component, rig);

            component.text = $"<color={(UseAccent ? Configuration.accentColor : "#ffffff")}>{component.text}</color>";

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            _tags[rig].transform.position = transform.position + new Vector3(0f, TagOffset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                _tags[rig].transform.rotation = Quaternion.LookRotation(forward);
            }
        }
    }
}
