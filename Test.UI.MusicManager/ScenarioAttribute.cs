using NUnit.Framework;

namespace Test.UI.MusicManager
{
    public class ScenarioAttribute : TestFixtureAttribute
    {
        public ScenarioAttribute(string description)
        {
            Description = description;
        }
    }
}