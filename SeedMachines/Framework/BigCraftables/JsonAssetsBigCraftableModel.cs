using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMachines.Framework.BigCraftables
{
    public class JsonAssetsBigCraftableModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsDefault { get; set; }
        public bool ProvidesLight { get; set; }
        public int ReserveExtraIndexCount { get; set; }
        public JsonAssetsBigCraftableRecipe Recipe { get; set; }
        public IDictionary<string, string> NameLocalization { get; set; }
        public IDictionary<string, string> DescriptionLocalization { get; set; }
        //public string TranslationKey { get; set; }
    }

    public class JsonAssetsBigCraftableRecipe
    {
        public int ResultCount { get; set; }
        public IList<JsonAssetsBigCraftableIngredient> Ingredients { get; set; }
        public bool CanPurchase { get; set; }
    }

    public class JsonAssetsBigCraftableIngredient
    {
        public object Object { get; set; }
        public int Count { get; set; }
    }
}
