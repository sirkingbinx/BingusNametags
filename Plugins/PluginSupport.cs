using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BingusNametags.Plugins
{
    public class PluginSupport : MonoBehaviour
    {
        public static PluginSupport instance;

        public void Awake()
        {
            instance = this;
            GorillaTagger.OnPlayerSpawned(GameAwake);
        }

        public void GameAwake()
        {
            var classesWithAttribute = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(BingusNametagPlugin)));

            foreach (Type type in classesWithAttribute)
            {
                Console.WriteLine($"BingusNametagsPlugin: Loading nametag plugin: {type.FullName}");

                BingusNametagPlugin attribute = (BingusNametagPlugin)Attribute.GetCustomAttribute(type, typeof(BingusNametagPlugin));
                MethodInfo tryUpdate = type.GetMethod("Update", BindingFlags.Public);

                if (tryUpdate == null)
                    Debug.LogError($"BingusNametagsPlugin: Error - TYPE: ${type.FullName}\nNo update method found, try adding a public void Update().");
                else
                    Main.UpdateTags += delegate { tryUpdate.Invoke(type, null); };

                Debug.Log($"BingusNametagsPlugin: Loaded nametag plugin: ${type.FullName}");
            }
        }
    }
}
