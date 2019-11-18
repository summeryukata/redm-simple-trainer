using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public static class Storage
    {
        internal static Type stringType = typeof(string);
        internal static Type floatType = typeof(float);
        internal static Type intType = typeof(int);
        internal static Type vector2Type = typeof(Vector2);
        internal static Type vector3Type = typeof(Vector3);
        internal static Type quatType = typeof(Quaternion);

        private static T GetInternal<T>(string key)
        {
            Type genericType = typeof(T);

            if (genericType == intType)
            {
                return (T)(object)API.GetResourceKvpInt(key);
            }
            else if (genericType == floatType)
            {
                return (T)(object)API.GetResourceKvpFloat(key);
            }
            else if (genericType == stringType)
            {
                return (T)(object)API.GetResourceKvpString(key);
            }
            else if (genericType == vector2Type)
            {
                Vector2 retVec = new Vector2
                {
                    X = API.GetResourceKvpFloat($"Vec2_{key}_X"),
                    Y = API.GetResourceKvpFloat($"Vec2_{key}_Y")
                };

                return (T)(object)retVec;
            }
            else if (genericType == vector3Type)
            {
                Vector3 retVec = new Vector3
                {
                    X = API.GetResourceKvpFloat($"Vec3_{key}_X"),
                    Y = API.GetResourceKvpFloat($"Vec3_{key}_Y"),
                    Z = API.GetResourceKvpFloat($"Vec3_{key}_Z")
                };

                return (T)(object)retVec;
            }
            else if (genericType == quatType)
            {
                Quaternion retQuat = new Quaternion
                {
                    X = API.GetResourceKvpFloat($"Quat_{key}_X"),
                    Y = API.GetResourceKvpFloat($"Quat_{key}_Y"),
                    Z = API.GetResourceKvpFloat($"Quat_{key}_Z"),
                    W = API.GetResourceKvpFloat($"Quat_{key}_W")
                };

                return (T)(object)retQuat;
            }
            else
            {
                Debug.WriteLine("Unknown type requested");
                return default;
            }
        }

        public static bool TryGet<T>(string key, out T outVal)
        {
            if (GetInternal<int>($"IsSet_{key}") != 0)
            {
                outVal = GetInternal<T>(key);

                return true;
            }

            outVal = default;
            return false;
        }

        public static void Set(string key, int value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvpInt(key, value);
        }

        public static void Set(string key, float value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvpFloat(key, value);
        }

        public static void Set(string key, string value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvp(key, value);
        }

        public static void Set(string key, Vector2 value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvpFloat($"Vec2_{key}_X", value.X);
            API.SetResourceKvpFloat($"Vec2_{key}_Y", value.Y);
        }

        public static void Set(string key, Vector3 value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvpFloat($"Vec3_{key}_X", value.X);
            API.SetResourceKvpFloat($"Vec3_{key}_Y", value.Y);
            API.SetResourceKvpFloat($"Vec3_{key}_Z", value.Z);
        }

        public static void Set(string key, Quaternion value)
        {
            API.SetResourceKvpInt($"IsSet_{key}", 1);
            API.SetResourceKvpFloat($"Quat_{key}_X", value.X);
            API.SetResourceKvpFloat($"Quat_{key}_Y", value.Y);
            API.SetResourceKvpFloat($"Quat_{key}_Z", value.Z);
            API.SetResourceKvpFloat($"Quat_{key}_W", value.W);
        }

        public static void Delete(string key)
        {
            API.DeleteResourceKvp(key);
        }

        public static List<T> Find<T>(string keyPattern)
        {
            List<T> results = new List<T>();

            int searchHandle = API.StartFindKvp(keyPattern);

            if (searchHandle != -1)
            {
                string key;

                do
                {
                    key = API.FindKvp(searchHandle);

                    if (key != null)
                    {
                        if (TryGet<T>(key, out T outVal))
                        {
                            results.Add(outVal);
                        }
                    }
                } while (key != null);
            }

            API.EndFindKvp(searchHandle);

            return results;
        }
    }
}
