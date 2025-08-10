using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace SeedMachines.Framework
{
    public interface IJsonAssetsAPI
    {
        void LoadAssets(string path);
        string GetBigCraftableId(string name);
    }
}
