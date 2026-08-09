using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BlueShadeStudio.Core
{
    public static class Search
    {
        public static bool Matches(string text, string query)
        {
            if (string.IsNullOrEmpty(query)) return true;
            return text.ToLower().Contains(query.ToLower());
        }

        public static bool IsPropertyVisible(string propertyName, string displayName, string query, bool advancedMode)
        {
            if (string.IsNullOrEmpty(query)) return true;

            bool matchesName = Matches(propertyName, query);
            bool matchesDisplay = Matches(displayName, query);

            return matchesName || matchesDisplay;
        }

        public static List<string> FilterProperties(Material material, string query, bool advancedMode)
        {
            if (string.IsNullOrEmpty(query)) return null;

            List<string> visibleProps = new List<string>();
            Shader shader = material.shader;
            int propCount = shader.GetPropertyCount();

            for (int i = 0; i < propCount; i++)
            {
                string name = shader.GetPropertyName(i);
                string displayName = name;
                if (IsPropertyVisible(name, displayName, query, advancedMode))
                {
                    visibleProps.Add(name);
                }
            }

            return visibleProps;
        }
    }
}
