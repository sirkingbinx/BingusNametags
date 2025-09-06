using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    public class ModList
    {
        public static Dictionary<VRRig, GameObject> mtags = new Dictionary<VRRig, GameObject>();

        private static string GetMods(VRRig rig)
        {
            string result = "";

            // a little fucked, but gets properties
            foreach (string prop in rig.OwningNetPlayer.GetPlayerRef().CustomProperties.Keys)
            {
                if (Main.KnownMods.TryGetValue(prop.ToLower(), out string modinfo))
                    result += $"[{modinfo}] ";
            }

            return result;
        }

        static float offset = 0.6f;

        private static void UpdateTag(VRRig rig)
        {
            if (!mtags.ContainsKey(rig))
                mtags[rig] = NametagCreator.CreateTag(rig, Color.blue, offset, GetMods(rig));

            TextMeshPro component = mtags[rig].GetComponent<TextMeshPro>();
            component.text = GetMods(rig);

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            mtags[rig].transform.position = transform.position + new Vector3(0f, offset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                mtags[rig].transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        public static void Update()
        {
            if (GorillaParent.instance != null)
            {
                List<VRRig> list = new List<VRRig>();
                foreach (KeyValuePair<VRRig, GameObject> keyValuePair in mtags)
                {
                    if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key))
                    {
                        GameObject.Destroy(keyValuePair.Value);
                        list.Add(keyValuePair.Key);
                    }
                }

                foreach (VRRig key in list)
                    mtags.Remove(key);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        ModList.UpdateTag(vrrig);
            }
        }
    }
}
