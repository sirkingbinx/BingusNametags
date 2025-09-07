using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace BingusNametags.Plugins
{
    public class BingusNametagPlugin : Attribute
    {
        public float offset = 0f;

        public (GameObject, TextMeshPro) CreateNametag(VRRig rig, string initialText = "Nametag Test", bool useAccentColor = true)
        {
            GameObject tag = NametagCreator.CreateTag(rig, useAccentColor ? Main.accentColor : Color.white, this.offset, initialText);
            return (tag, tag.GetComponent<TextMeshPro>());
        }

        public void UpdatePositionAndRotation(VRRig rig, GameObject tag)
        {
            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            tag.transform.position = transform.position + new Vector3(0f, offset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                tag.transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        public BingusNametagPlugin(float _offset) =>
            offset = _offset;
    }
}
