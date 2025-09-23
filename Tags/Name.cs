using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    internal class Name
    {
        internal static void UpdateNametag(TextMeshPro component, VRRig rig)
        {
            if (GFriendsIntegration.Verified(rig.OwningNetPlayer) | rig.OwningNetPlayer.UserId == "DEFC9810769F1F55")
                component.text = $"<color={Configuration.GFriendsVerifiedColor}>{rig.OwningNetPlayer.NickName}</color>";
            else if (GFriendsIntegration.Friend(rig.OwningNetPlayer))
                component.text = $"<color={Configuration.GFriendsFriendColor}>{rig.OwningNetPlayer.NickName}</color>";
            else if (GFriendsIntegration.RecentlyPlayedWith(rig.OwningNetPlayer))
                component.text = $"<color={Configuration.GFriendsRecentColor}>{rig.OwningNetPlayer.NickName}</color>"; 
            else
                component.text = rig.OwningNetPlayer.NickName;
        }
    }
}
