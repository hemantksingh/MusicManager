using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Test.UI.MusicManager
{
    public class ScenarioReader
    {
        public string GetScenarios()
        {
            var scenarios = new StringBuilder();
            foreach (Type type in GetType()
                .Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(ScenarioAttribute), false) != null))
            {
                var scenario = type.GetCustomAttribute<ScenarioAttribute>();

                scenarios.AppendLine("Scenario: " + scenario.Description);
                scenarios.AppendLine();
                scenarios.AppendLine(type.Name.RemoveUnderScore());
                scenarios.AppendLine();

                foreach (MethodInfo whenMethod in type.GetMethodsWith<SetUpAttribute>())
                {
                    scenarios.AppendLine(whenMethod.Name.RemoveUnderScore().ToSentence());
                }

                scenarios.AppendLine();

                foreach (MethodInfo thenMethod in type.GetMethodsWith<TestAttribute>())
                {
                    scenarios.AppendLine(thenMethod.Name.RemoveUnderScore().ToSentence());
                }

                scenarios.AppendLine();
            }

            return scenarios.ToString();
        }

        [Test]
        public void WriteScenarios()
        {
            Debug.WriteLine(GetScenarios());
        }
    }

    public static class ScenarioReaderExtension
    {
        public static string RemoveUnderScore(this string text)
        {
            return text.Replace('_', ' ');
        }

        public static string ToSentence(this string text)
        {
            return Regex.Replace(text, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static IEnumerable<MethodInfo> GetMethodsWith<T>(this Type type)
        {
            return type.GetMethods()
                       .Where(method => method.GetCustomAttributes(typeof (T), false).Any())
                       .ToArray();
        }
    }
}