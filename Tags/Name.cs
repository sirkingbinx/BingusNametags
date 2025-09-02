using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    public class Name
    {
        private static Dictionary<VRRig, GameObject> tags = new Dictionary<VRRig, GameObject>();

        static float offset = 1f;

        public static void Enabled = false;
        public static void GFriends = false;

        public static void Update()
        {
            if (!Enabled) return;
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

        private static void UpdateTag(VRRig rig)
        {
            if (!tags.ContainsKey(rig))
                tags[rig] = NametagCreator.CreateTag(rig, Color.white, offset);

            TextMeshPro component = tags[rig].GetComponent<TextMeshPro>();

            if (GFriends) {
                if (GFriendsIntegration.RecentlyPlayedWith(rig.OwningNetPlayer))
                    component.text = $"<color=#ffa0a0>{rig.OwningNetPlayer.NickName}</color>";
                
                if (GFriendsIntegration.Friend(rig.OwningNetPlayer))
                    component.text = $"<color=#cc7fe5>{rig.OwningNetPlayer.NickName}</color>";
            }

            bool verified = Main.KnownPeople.Contains(rig.OwningNetPlayer.UserId);

            if (!verified)
                component.text = rig.OwningNetPlayer.NickName;
            else
                component.text = $"<color=#7fff7f>{rig.OwningNetPlayer.NickName}</color>";

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            tags[rig].transform.position = transform.position + new Vector3(0f, offset, 0f);

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
