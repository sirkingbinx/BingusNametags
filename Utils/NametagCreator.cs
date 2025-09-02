using TMPro;
using UnityEngine;
using static Main;

namespace BingusNametags
{
    public class NametagCreator
    {
        public static GameObject CreateTag(VRRig rig, Color color, float offset, string initialText = "Test")
        {
            GameObject gameObject = new GameObject("NameTag_TMP");
            Transform parent = rig.transform.Find("Body") ?? rig.transform;

            gameObject.transform.SetParent(parent, false);
            gameObject.transform.localPosition = new Vector3(0f, offset, 0f);
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
            textMeshPro.enableAutoSizing = false;
            textMeshPro.fontSize = 4f;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.color = color;
            textMeshPro.text = initialText;

            TMPLookAt tmplookAt = gameObject.AddComponent<TMPLookAt>();
            tmplookAt.who = rig;
            tmplookAt.text = textMeshPro;

            return gameObject;
        }
    }
}
