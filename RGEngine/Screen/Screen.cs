using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
    public class Screen : gameController
    {
        static Screen _activescreen;
        static List<Screen> _screenList = new List<Screen>();
    }
}
