using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.Attrbutes
{
    public class IntDescriptionAttribute : DescriptionAttribute
    {
        public int IntValue { get; set; }
        public IntDescriptionAttribute(string description, int intValue = 0)
        {
            this.DescriptionValue = description;
            this.IntValue = intValue;
        }
    }
}
