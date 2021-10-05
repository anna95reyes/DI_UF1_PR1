using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace le_petit_chef.Model
{
    public enum Unitat
    {
        [Description("uds.")] UDS,
        [Description("g.")] G,
        [Description("ml.")] ML

        // al codi posar:
        // EnumDescriptionConverter.getDesc(pal);
    }
}
