namespace GrayHorizons.Extensions
{
    using System;

    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static void Toggle(this bool obj)
        {
            obj = !obj;
        }

        public static void TryCast<T>(this object obj, Action<T> callBack) where T: class
        {
            var cast = obj as T;
            if (cast.IsNotNull())
                callBack.Invoke(cast);
        }
    }
}

