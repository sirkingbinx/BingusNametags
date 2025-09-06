using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    public class Platform
    {
        public static Dictionary<VRRig, GameObject> ptags = new Dictionary<VRRig, GameObject>();
        public static bool UseOculusName = false;

        private static string GetPlatform(VRRig rig)
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;
            string result;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                result = "[Steam]";
            else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") | rig.Creator.GetPlayerRef().CustomProperties.Count > 1)
                result = $"[{(UseOculusName ? "Oculus Rift" : "Rift PCVR")}]";
            else
                result = $"[{(UseOculusName ? "Oculus Quest" : "Meta")}]";

            return result;
        }

        static float offset = 0.8f;

        private static void UpdateTag(VRRig rig)
        {
            if (!ptags.ContainsKey(rig))
                ptags[rig] = NametagCreator.CreateTag(rig, Main.accentColor, offset, GetPlatform(rig));

            TextMeshPro component = ptags[rig].GetComponent<TextMeshPro>();
            component.text = GetPlatform(rig);

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

        public static void Update()
        {
            if (GorillaParent.instance != null)
            {
                List<VRRig> list = new List<VRRig>();
                foreach (KeyValuePair<VRRig, GameObject> keyValuePair in ptags)
                {
                    if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key))
                    {
                        GameObject.Destroy(keyValuePair.Value);
                        list.Add(keyValuePair.Key);
                    }
                }

                foreach (VRRig key in list)
                    ptags.Remove(key);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        Platform.UpdateTag(vrrig);
            }
        }
    }
}
