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

        public static void AddPluginUpdate(Action<TextMeshPro, VRRig> updateFunction, float nametagOffset = 0f, bool useAccentColor = true)
        {
            // if (!PluginsEnabled) throw new Exception("Plugins are not currently enabled.");


            BingusNametagsPlugin assignedPluginManager = new BingusNametagsPlugin();
            loadedPlugins.Add(assignedPluginManager);

            assignedPluginManager.UpdateTag += updateFunction;
            assignedPluginManager.tagOffset = nametagOffset != 0f ? nametagOffset : (1.2f + (loadedPlugins.IndexOf(assignedPluginManager) * 0.2f));

            Main.UpdateTags += assignedPluginManager.Update;
        }
    }

    internal class BingusNametagsPlugin
    {
        internal Dictionary<VRRig, GameObject> tags = new Dictionary<VRRig, GameObject>();

        internal void Update()
        {
            if (GorillaParent.instance != null)
            {
                List<VRRig> list = new List<VRRig>();

                foreach (KeyValuePair<VRRig, GameObject> keyValuePair in tags)
                {
                    if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key))
                    {
                        GameObject.Destroy(keyValuePair.Value);
                        list.Add(keyValuePair.Key);
                    }
                }

                foreach (VRRig key in list)
                    tags.Remove(key);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        UpdateTagLocal(vrrig);
            }
        }

        internal event Action<TextMeshPro, VRRig> UpdateTag = delegate { };
        internal float tagOffset = 1.2f;

        internal void UpdateTagLocal(VRRig rig)
        {
            if (!tags.ContainsKey(rig))
                tags[rig] = NametagCreator.CreateTag(rig, Configuration.accentColor, tagOffset, "PluginTextObject");

            TextMeshPro component = tags[rig].GetComponent<TextMeshPro>();

            UpdateTag(component, rig);

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            tags[rig].transform.position = transform.position + new Vector3(0f, tagOffset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                tags[rig].transform.rotation = Quaternion.LookRotation(forward);
            }
        }
    }
}
