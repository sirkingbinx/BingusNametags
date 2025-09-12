# BingusNametags ![download badge thing (its not a virus i promise)](https://img.shields.io/github/downloads/sirkingbinx/BingusNametags/total)
BingusNametags is a customizable name mod for Gorilla Tag. It's easily extendable by anyone with a little bit of C# knowledge.

<img width=350 height=350 src="https://github.com/user-attachments/assets/ad530b19-5795-40d8-95cb-3a697340e041">
<img width=350 height=350 src="https://github.com/user-attachments/assets/cb126c2f-3cf4-4b40-a405-a0288445a7c6">

## Configuration
- You can choose what and how to show nametags in `BepInEx/config/bingus.nametags.cfg`.
- You can place any `.ttf (font file)` in your plugins folder named `BingusNametagsFont.ttf` to use that font instead of the default. I recommend [JetBrains Mono](https://www.jetbrains.com/lp/mono/).

## For Developers
### Properties
You can choose to show or hide the recognized mods nametags for the player by setting `bingusnametags_ignoreme` in your player properties.
```cs
// Mod checker only shows [Private] on top of your rig
PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
    { { "bingusnametags_ignoreme", "showPrivate" } });

// Mod checker just doesn't show anything
// This can be anything BUT "showPrivate"
PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
    { { "bingusnametags_ignoreme", "abaisuhghauyyus" } });
```
### Plugin Creation (WIP)
> [!WARNING]
> I'm still working on this and the way to create plugins will probably be changing rapidly.

You can create your own nametags using `BingusNametagPlugin`.

**Example:**
```cs
using BingusNametags.Plugins;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using BepInEx;

namespace MyNametag
{
    // BingusNametagPlugin(float offset)
    // The offsets 0.6f, 0.8f, and 1f are already used by the mod
    [BingusNametagPlugin(1.2f)]
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

    [BepInDependency("bingus.nametags", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("myname.mynametagbootstrapper", "MyNametag", "1.0.0")]
    public class Bootstrap : BaseUnityPlugin
    {
        // Remove this if you already have an Awake() function in your BaseUnityPlugin
        // It's only needed to make BepInEx wake up
        public void Awake() => Debug.Log("wake up bepinex")
    }
}
```

## Dependencies
- .NET
- [GorillaFriends](https://github.com/not-a-bird-09/GorillaFriends) (only needed for building)

## Credits
- Mod checker list from [GorillaNametags](https://github.com/HanSolo1000Falcon/GorillaNametags) by [HanSolo1000Falcon](https://github.com/HanSolo1000Falcon)
- Verified users, friending system, and recent player checks from [GorillaFriends](https://github.com/rusjj/gorillafriends) by [RusJJ](https://github.com/rusjj)
