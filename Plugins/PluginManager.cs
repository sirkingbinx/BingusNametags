using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BingusNametags.Plugins
{
    public class PluginManager
    {
        internal static Dictionary<string, BingusNametagsPlugin> loadedPlugins = new Dictionary<string, BingusNametagsPlugin>();

        public static AddPluginUpdate(Action updateFunction)
        {
            BingusNametagsPlugin assignedPluginManager = new BingusNametagsPlugin(updateFunction);

            Main.UpdateTags += assignedPluginManager.Update;

            loadedPlugins.Add(Assembly.GetCallingAssembly().FullName, assignedPluginManager);
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

        internal static event Action UpdateTag = delegate { };

        internal void UpdateTagLocal(VRRig rig)
        {
            if (!tags.ContainsKey(rig))
                tags[rig] = NametagCreator.CreateTag(rig, Main.accentColor, offset, "PluginTextObject");

            TextMeshPro component = tags[rig].GetComponent<TextMeshPro>();

            UpdateTag(component, rig);

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            ptags[rig].transform.position = transform.position + new Vector3(0f, offset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                ptags[rig].transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        internal BingusNametagsPlugin(Action updateFunction) => UpdateTag += updateFunction;
    }
}
