using System.ComponentModel;
using System.Reflection;

namespace LocalAIAgentApp.Model
{
    public static class EnumExtensions
    {
        /// <summary>
        /// GetDescription メソッドは、列挙型の値に関連付けられた説明を取得するための拡張メソッドです。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field.GetCustomAttribute<DescriptionAttribute>();

            return attr?.Description ?? value.ToString();
        }
    }
}
