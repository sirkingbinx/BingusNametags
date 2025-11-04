using TMPro;
using UnityEngine;

namespace BingusNametags
{
    public class NametagCreator
    {
        public static GameObject CreateTag(VRRig rig, float offset, string initialText = "Test")
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

            if (Configuration.customFont)
                textMeshPro.font = Configuration.customFont;
            else
                textMeshPro.fontStyle = FontStyles.Bold;

            textMeshPro.color = Color.white;
            textMeshPro.text = initialText;

            TMPLookAt tmplookAt = gameObject.AddComponent<TMPLookAt>();
            tmplookAt.who = rig;
            tmplookAt.text = textMeshPro;

            return gameObject;
        }

        public class TMPLookAt : MonoBehaviour
        {
            private void Update()
            {
                if (who != null && text != null)
                {
                    Vector3 f = Camera.main.transform.forward;
                    f.y = 0f;
                    f.Normalize();

                    transform.rotation = Quaternion.LookRotation(f);
                }
            }

            public VRRig who;
            public TextMeshPro text;
        }
    }
}
