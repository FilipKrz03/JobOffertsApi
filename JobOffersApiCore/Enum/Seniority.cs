using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Enum
{
    public enum Seniority
    {
        [EnumMember(Value = "Junior")]
        Junior ,

        [EnumMember(Value = "Mid")]
        Mid ,

        [EnumMember(Value = "Senior")]
        Senior ,

        [EnumMember(Value = "Unknown")]
        Unknown
    }
}
