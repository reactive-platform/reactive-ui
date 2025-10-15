using System.Reflection;

namespace Reactive {
    internal static class ReflectionUtils {
        public const BindingFlags DefaultFlags = RequiredFlags | BindingFlags.Instance;
        public const BindingFlags StaticFlags = RequiredFlags | BindingFlags.Static;
        public const BindingFlags RequiredFlags = BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags UniversalFlags = RequiredFlags | BindingFlags.Instance | BindingFlags.Static;

        public static bool ValidateValueMember(this MemberInfo? memberInfo) {
            return memberInfo is FieldInfo or PropertyInfo;
        }
        
        public static bool GetValueImplicitly(this MemberInfo member, object? obj, out object? value) {
            value = member switch {
                FieldInfo fld => fld.GetValue(obj),
                PropertyInfo prop => prop.GetValue(obj),
                _ => null
            };
            return value is not null;
        }

        public static bool SetValueImplicitly(this MemberInfo member, object obj, object? value) {
            switch (member) {
                case FieldInfo fld:
                    fld.SetValue(obj, value);
                    break;
                case PropertyInfo prop:
                    prop.SetValue(obj, value);
                    break;
                case EventInfo evt:
                    evt.AddMethod.Invoke(obj, new[] { value! });
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}