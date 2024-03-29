﻿using SeedMachines.Framework.Minigames;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SeedMachines.Framework.BigCraftables
{
    [XmlType("Mods_SeedMachines_SeedBandit")]
    public class SeedBandit : IBigCraftable
    {
        public SeedBandit() : base()
        {
            this.wrapperName = "Seed Bandit";
            this.animate();
        }

        public SeedBandit(StardewValley.Object baseObject, IBigCraftableWrapper wrapper)
            : base(baseObject, wrapper)
        { }

        public override void onClick(ButtonPressedEventArgs args)
        {
            ModEntry.modHelper.Input.Suppress(args.Button);

            if (Game1.player.Money >= ModEntry.settings.seedBanditOneGamePrice)
            {
                Game1.player.Money -= ModEntry.settings.seedBanditOneGamePrice;

                SalableSeedsEnumerator salableObjects = new SalableSeedsEnumerator();
                var listOfSeeds = new List<StardewValley.Object>();
                foreach (StardewValley.Object obj in salableObjects)
                {
                    listOfSeeds.Add(obj);
                }
                Random rnd = new Random();
                int rdnSeedsIndex = rnd.Next(listOfSeeds.Count);

                Game1.player.addItemByMenuIfNecessary((Item)listOfSeeds[rdnSeedsIndex]);
            }
            else
            {
                Game1.addHUDMessage(new HUDMessage(ModEntry.modHelper.Translation.Get("seed-bandit.not-enough-money"), 3));
            }
            //Game1.currentMinigame = new SeedBanditSlots();
        }
    }
}
