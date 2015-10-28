﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSift.Enum
{
    public enum AccountUsagePeriod
    {
        [Description("daily")]
        Daily,

        [Description("hourly")]
        Hourly,

        [Description("monthly")]
        Monthly
    }
}
