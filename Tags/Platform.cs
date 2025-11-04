using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    internal class Platform
    {
        internal static bool UseOculusName = false;

        internal static void GetPlatform(TextMeshPro tmp, VRRig rig)
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;
            string result;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                result = "[Steam]";
            else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") | rig.Creator.GetPlayerRef().CustomProperties.Count > 1)
                result = $"[{(UseOculusName ? "Oculus Rift" : "Oculus PCVR")}]";
            else
                result = $"[{(UseOculusName ? "Oculus Quest" : "Meta")}]";

            tmp.text = result;
        }

        internal static void UpdateNametag(TextMeshPro component, VRRig rig) =>
            GetPlatform(component, rig);
    }
}
