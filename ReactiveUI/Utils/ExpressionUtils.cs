using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reactive {
    internal static class ExpressionUtils {
        public static string GetPropertyNameOrThrow<T>(this Expression<T> expression) {
            if (expression.Body is not MemberExpression memberExpression) {
                throw new ArgumentException("The expression is not a member access expression");
            }
            return memberExpression.Member.Name;
        }

        public static Action<T, TValue> GeneratePropertySetter<T, TValue>(this Expression<Func<T, TValue>> expression) {
            var members = CreateCallStack(expression);
            
            return (obj, value) => {
                object? currentObject = obj;
                
                for (var i = 0; i < members.Count - 1; i++) {
                    members[i].GetValueImplicitly(currentObject, out currentObject);
                }
                
                members.Last().SetValueImplicitly(currentObject!, value);
            };
        }

        public static Func<T, TValue> GeneratePropertyGetter<T, TValue>(this Expression<Func<T, TValue>> expression) {
            var members = CreateCallStack(expression);
            
            return obj => {
                object? currentObject = obj;
                
                foreach (var member in members) {
                    member.GetValueImplicitly(currentObject, out currentObject);
                }
                
                return (TValue)currentObject!;
            };
        }

        private static IList<MemberInfo> CreateCallStack<T, TValue>(Expression<Func<T, TValue>> expression) {
            var members = new List<MemberInfo>();
            var exp = expression.Body;
            
            while (exp is MemberExpression memberExp) {
                var member = memberExp.Member;
                ValidateMemberOrThrow(member);
                
                members.Add(member);
                exp = memberExp.Expression;
            }

            if (members.Count == 0) {
                throw new ArgumentException("The expression is not a member access expression");
            }
            
            // reverse to start from the outermost property
            members.Reverse();
            return members;
        }
        
        private static void ValidateMemberOrThrow(MemberInfo? member) {
            if (!member.ValidateValueMember()) {
                throw new NotSupportedException("The member type is not supported. Consider using field or property");
            }
        }
    }
}