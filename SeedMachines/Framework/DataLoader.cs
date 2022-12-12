using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SeedMachines.Framework.BigCraftables;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMachines.Framework
{
    class DataLoader : IAssetEditor
    {
        public static IJsonAssetsAPI jsonAssetsAPI;
        public static ISpaceCoreAPI spaceCoreAPI;
        public static bool isJsonAssetsLoaded;
        public static bool isSpaceCoreLoaded;

        public static IAssetData craftingRecipesAsset;
        public static IAssetData bigCraftableInformationsAsset;

        private bool craftablesTilesheetWasPatched = false;
        private bool dataAssetsWasPatched = false;

        public static Dictionary<string, object> AssetsToLoad = new Dictionary<string, object>();

        public DataLoader()
        {
            isJsonAssetsLoaded = ModEntry.modHelper.ModRegistry.IsLoaded("spacechase0.JsonAssets");
            isSpaceCoreLoaded = ModEntry.modHelper.ModRegistry.IsLoaded("spacechase0.SpaceCore");

            //Register XmlSerializer Types
            if (DataLoader.isSpaceCoreLoaded == true)
            {
                spaceCoreAPI = ModEntry.modHelper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore");
                if (spaceCoreAPI != null)
                {
                    spaceCoreAPI.RegisterSerializerType(typeof(SeedMachine));
                    spaceCoreAPI.RegisterSerializerType(typeof(SeedBandit));
                }
            }

            //Load Assets
            if (isJsonAssetsLoaded == true)
            {
                prepareJsonAssetsJSONs(ModEntry.settings.themeName);
                jsonAssetsAPI = ModEntry.modHelper.ModRegistry.GetApi<IJsonAssetsAPI>("spacechase0.JsonAssets");
                jsonAssetsAPI.LoadAssets(Path.Combine(ModEntry.modHelper.DirectoryPath, "assets", "SeedMachines" + ModEntry.settings.themeName + "JA"));
                prepareCorrectIDs();
            }
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return false;
        }

        public void Edit<T>(IAssetData asset)
        {
            if (isJsonAssetsLoaded == true) return;
        }


        public void prepareJsonAssetsJSONs(String themeName)
        {
            foreach(String wrapperName in IBigCraftableWrapper.getAllWrappers().Keys)
            {
                ModEntry.modHelper.Data.WriteJsonFile(
                    "assets/SeedMachines" + themeName + "JA/BigCraftables/" + wrapperName + "/big-craftable.json",
                    IBigCraftableWrapper.getWrapper(wrapperName).getJsonAssetsModel()
                );
            }
        }

        public void prepareCorrectIDs()
        {
            foreach (String wrapperName in IBigCraftableWrapper.getAllWrappers().Keys)
            {
                int bigCraftableId = jsonAssetsAPI.GetBigCraftableId(wrapperName);
                IBigCraftableWrapper.getWrapper(wrapperName).itemID = bigCraftableId;
            }
 
        }
    }
}
