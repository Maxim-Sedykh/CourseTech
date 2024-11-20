using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseTech.Domain.Comparers
{
    public static class DynamicListComparer
    {
        public static bool AreListsOfDynamicEqual(List<dynamic> list1, List<dynamic> list2)
        {
            if (list1 == null || list2 == null || list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!AreDynamicObjectsEqual(list1[i], list2[i]))
                    return false;
            }
            return true;
        }

        private static bool AreDynamicObjectsEqual(dynamic obj1, dynamic obj2)
        {
            if (obj1 == null || obj2 == null)
                return obj1 == obj2;

            var dict1 = obj1 as IDictionary<string, object>;
            var dict2 = obj2 as IDictionary<string, object>;

            if (dict1 == null || dict2 == null)
                return false;

            if (dict1.Count != dict2.Count)
                return false;

            var keys = new HashSet<string>(dict1.Keys);
            foreach (var key in keys)
            {
                if (!dict2.TryGetValue(key, out var value) || !object.Equals(dict1[key], value))
                    return false;
            }

            return true;
        }
    }
}
