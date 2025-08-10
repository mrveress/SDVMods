using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMachines.Framework
{
    class SalableSeedsEnumerator
    {
        public IEnumerator<StardewValley.Object> GetEnumerator()
        {
            foreach (string id in Game1.objectData.Keys)
            {
                StardewValley.Object item;
                
                try
                {
                    item = new StardewValley.Object(id, 1);
                }
                catch
                {
                    continue;
                }

                // We only care about seed packets
                if (item.Category == StardewValley.Object.SeedsCategory)
                {
                    yield return item;
                }
            }
        }

        public static Dictionary<ISalable, ItemStockInformation> GetSeedsForSale()
        {
            Dictionary<ISalable, ItemStockInformation> result = new Dictionary<ISalable, ItemStockInformation>();
            foreach (ISalable obj in new SalableSeedsEnumerator())
            {
                int basePrice = obj.salePrice() <= 1 ? ModEntry.settings.seedMachinePriceForNonSalableSeeds : obj.salePrice();
                int price = Convert.ToInt32(Math.Ceiling(basePrice * ModEntry.settings.seedMachinePriceMultiplier));
                
                result.Add(obj, new ItemStockInformation(price, int.MaxValue));
            }
            return result;
        }
    }
}
