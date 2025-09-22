using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    internal class Name
    {
        internal static void UpdateNametag(TextMeshPro component, VRRig rig)
        {
            if (GFriends) {
                if (GFriendsIntegration.Verified(rig.OwningNetPlayer) | rig.OwningNetPlayer.UserId == "DEFC9810769F1F55")
                    component.text = $"<color=#7fff7f>{rig.OwningNetPlayer.NickName}</color>";
                else if (GFriendsIntegration.Friend(rig.OwningNetPlayer))
                    component.text = $"<color=#cc7fe5>{rig.OwningNetPlayer.NickName}</color>";
                else if (GFriendsIntegration.RecentlyPlayedWith(rig.OwningNetPlayer))
                    component.text = $"<color=#ffa0a0>{rig.OwningNetPlayer.NickName}</color>"; 
                else
                    component.text = rig.OwningNetPlayer.NickName;
            } else {
                // bingus
                // plz do not steal my shiny rocks
                if (rig.OwningNetPlayer.UserId == "DEFC9810769F1F55")
                    component.text = $"<color=#7fff7f>{rig.OwningNetPlayer.NickName}</color>";

                component.text = rig.OwningNetPlayer.NickName;
            }
        }
    }
}
