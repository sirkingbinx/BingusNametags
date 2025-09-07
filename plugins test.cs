using BingusNametags;
using BingusNametags.Plugins;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace MyNametag
{
    // BingusNametagPlugin(float offset)
    [BingusNametagPlugin(1f)]
    public class Nametag
    {
        public Dictionary<VRRig, GameObject> nametags = new Dictionary<VRRig, GameObject>();
        public BingusNametagPlugin myPluginManager = null;

        private void UpdateTag(VRRig rig)
        {
            // grab the plugin manager, which lets us create nametags without a lot of work
            if (myPluginManager == null)
                myPluginManager = this.GetType().GetCustomAttribute<BingusNametagPlugin>();

            if (!nametags.ContainsKey(rig))
            {
                //  obj    ,     text
                (GameObject, TextMeshPro) tag = myPluginManager.CreateNametag(rig);
                nametags[rig] = tag.Item1;
            }

            TextMeshPro textMeshPro = nametags[rig].GetComponent<TextMeshPro>();
            textMeshPro.text = rig.OwningNetPlayer.NickName;

            myPluginManager.UpdatePositionAndRotation(rig, nametags[rig]);
        }

        public void Update()
        {
            if (GorillaParent.instance != null)
            {
                List<VRRig> players = new List<VRRig>();

                // Delete nametags of players not in the current lobby
                foreach (KeyValuePair<VRRig, GameObject> keyValuePair in nametags)
                {
                    if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key))
                    {
                        GameObject.Destroy(keyValuePair.Value);
                        players.Add(keyValuePair.Key);
                    }
                }

                // Delete the player from the nametags list
                foreach (VRRig key in players)
                    nametags.Remove(key);

                // Update all the nametags
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        // not the current player, update it
                        UpdateTag(vrrig);
            }
        }
    }
}