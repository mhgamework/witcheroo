using System;
using System.Linq;

namespace Modules.MoveScenario._Test._Incubating
{
    public class OSUtilities
    {
        public static string GetAllEnvironmentVariables()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            var rows = environmentVariables.Keys.Cast<string>().Select(key => $"{key}={environmentVariables[key]}");
            return string.Join("\n", rows);
        }
    }
}