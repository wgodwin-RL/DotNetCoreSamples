using LD_Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD_Common
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

                case "pm":
                case "prodmirror":
                case "prod mirror":
                    return Environments.ProdMirror;

                case "prod":
                case "production":
                    return Environments.Production;

                case "staging":
                    return Environments.Staging;

                case "qa":
                case "qualityassurance":
                    return Environments.QualityAssurance;


                default:
                    return Environments.Unknown;
            }

        }


        public static string GetEnvironmentFilename(Environments environment)
        {
            switch (environment)
            {

                //
                //AWS
                //
                case Environments.Local:
                    return "local";

                case Environments.Development:
                    return "dev";

                case Environments.ProdMirror:
                    return "pm";

                case Environments.Production:
                    return "prod";

                case Environments.Staging:
                    return "staging";

                case Environments.QualityAssurance:
                    return "qa";


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
