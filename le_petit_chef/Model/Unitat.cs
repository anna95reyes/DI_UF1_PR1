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

        /*
         * Faig servir el Descriptor per poder posar les unitats d'una manera mes agradable, i per poder agafar el parametre Descriptor,
         * faig anar la clase EnumDescriptionConverter i l'agafo desde codi cridant la funcio: EnumDescriptionConverter.getDesc(Enum enumObj);
         */
        [Description("uds.")] UDS,
        [Description("g.")] G,
        [Description("ml.")] ML
    }
}
