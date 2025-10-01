using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BingusNametags.Plugins
{
    public class PluginManager
    {
        public static bool PluginsEnabled = true;
        internal static List<BingusNametagsPlugin> Plugins = new List<BingusNametagsPlugin>();
    }

    public class BingusNametagsPlugin
    {
        private Dictionary<VRRig, GameObject> tags = new Dictionary<VRRig, GameObject>();
        private bool _Enabled = true;

        public bool Enabled {
            get {
                return _Enabled;
            }

            set {
                _Enabled = value;

                if (value) {
                    Main.UpdateTags += assignedPluginManager.Update;
                } else if (!value) {
                    Main.UpdateTags -= assignedPluginManager.Update;
                }
            }
        }

        private void Update()
        {
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
                        UpdateTagLocal(vrrig);
            }
        }

        private event Func<VRRig, string, string> UpdateTag;
        private float tagOffset = 1.2f;

        private void UpdateTagLocal(VRRig rig)
        {
            if (!tags.ContainsKey(rig))
                tags[rig] = NametagCreator.CreateTag(rig, tagOffset, "Text");

            TextMeshPro component = tags[rig].GetComponent<TextMeshPro>();

            string text = "";

            if (UpdateTag != null) {
                string _text = UpdateTag(rig, Configuration.accentColor);
                // check if it contains color tags

                if (!_text.Contains("<color=")) {
                    // add our own
                    text = $"<color={Configuration.accentColor}>{text}</color>";
                }
            }

            component.text = text;

            Transform transform = rig.transform.Find("Head") ?? rig.transform;
            tags[rig].transform.position = transform.position + new Vector3(0f, tagOffset, 0f);

            if (Camera.main != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                tags[rig].transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        public BingusNametagsPlugin(Func<VRRig, string, string> _NametagUpdate, bool __Enabled = true, float _NametagOffset = 0f) {
            if (!PluginManager.PluginsEnabled) throw new Exception("Plugins are not currently enabled.");

            Plugins.Add(this);

            this.UpdateTag += _NametagUpdate;
            this.tagOffset = _NametagOffset != 0f ? _NametagOffset : (1.2f + (PluginManager.Plugins.IndexOf(this) * 0.2f));
            this._Enabled = __Enabled;
        }
    }
}
