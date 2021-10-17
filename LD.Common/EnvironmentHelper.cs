using LD.Models.Enums;

namespace LD.Common
{
    public static class EnvironmentHelper
    {
        public static Environments Environment = Environments.Unknown;

        public static Environments GetEnvironment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
                return Environments.Unknown;

            switch (environment.ToLower())
            {
                case "local":
                    return Environments.Local;

                case "dev":
                case "development":
                    return Environments.Development;

                default:
                    return Environments.Unknown;
            }

        }


        public static string GetEnvironmentFilename(Environments environment)
        {
            switch (environment)
            {
                case Environments.Local:
                    return "local";

                case Environments.Development:
                    return "dev";

                case Environments.Unknown:
                default:
                    return "";
            }

        }

        public static bool In(this Environments env, params Environments[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (env == list[i])
                    return true;
            }
            return false;
        }
    }
}
