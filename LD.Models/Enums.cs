using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LD_Models.Enums
{


    public enum Environments
    {
        [Description("Unknown")]
        Unknown,

        [Description("Local")]
        Local,

        [Description("Development")]
        Development,

        [Description("Prod Mirror")]
        ProdMirror,

        [Description("Production")]
        Production,

        [Description("Staging")]
        Staging,

        [Description("Quality Assurance")]
        QualityAssurance,
    }
}
