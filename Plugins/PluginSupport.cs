using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BingusNametags.Plugins
{
    public class BingusPluginManager
    {
        public static void Register<T>()
        {
            Type type = typeof(T);

            Console.WriteLine($"BingusNametagsPlugin: Loading nametag plugin: {type.FullName}");

            BingusNametagPlugin attribute = (BingusNametagPlugin)Attribute.GetCustomAttribute(type, typeof(BingusNametagPlugin));
            MethodInfo tryUpdate = type.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance);

            if (tryUpdate == null)
                Debug.LogError($"BingusNametagsPlugin: Error - TYPE: ${type.FullName}\nNo update method found, try adding a public void Update().");
            else
                Main.UpdateTags += delegate { tryUpdate.Invoke(type, null); };

            Debug.Log($"BingusNametagsPlugin: Loaded nametag plugin: ${type.FullName}");
        }
    }
}
