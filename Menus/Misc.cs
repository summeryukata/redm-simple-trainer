using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Misc : Drawing
    {
        public static async Task Draw()
        {
            SetMenuTitle("Utils", "miscellaneous things~");

            AddBool("Display coords?", ref g_ingameCoords);
            AddBool("Debug controls?", ref g_debugControls);

            StyleMenu();

            await Task.FromResult(0);
        }
    }
}
