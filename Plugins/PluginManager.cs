using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BingusNametags.Plugins
{
    internal class BingusNametagsPlugin
    {
        public Dictionary<VRRig, GameObject> tags = new Dictionary<VRRig, GameObject>();

        public static void Update()
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
                        UpdateTag(vrrig);
            }
        }

        private static void UpdateTag(GameObject tagObject, VRRig rig)
        {
            
        }
    }

    public class PluginManager
    {

        
    }
}
