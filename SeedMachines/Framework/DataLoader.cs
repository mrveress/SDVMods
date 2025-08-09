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
using StardewModdingAPI.Framework.ModHelpers;

namespace SeedMachines.Framework
{
    class DataLoader
    {
        public static IJsonAssetsAPI jsonAssetsAPI;
        public static ISpaceCoreAPI spaceCoreAPI;
        public static bool isJsonAssetsLoaded;
        public static bool isSpaceCoreLoaded;

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
                try
                {
                    jsonAssetsAPI = ModEntry.modHelper.ModRegistry.GetApi<IJsonAssetsAPI>("spacechase0.JsonAssets");
                    if (jsonAssetsAPI != null)
                    {
                        jsonAssetsAPI.LoadAssets(
                            Path.Combine(ModEntry.modHelper.DirectoryPath, "assets", "SeedMachines" + ModEntry.settings.themeName + "JA")
                        );
                        prepareCorrectIDs();
                    }
                    else
                    {
                        isJsonAssetsLoaded = false;
                        jsonAssetsAPIErrorLog();
                    }
                }
                catch (Exception)
                {
                    isJsonAssetsLoaded = false;
                    jsonAssetsAPIErrorLog();
                }
            }
        }

        private static void jsonAssetsAPIErrorLog()
        {
            ModEntry.monitor.Log(
                "Json Assets API not available or interface mismatch. SeedMachines will run without JA features.",
                StardewModdingAPI.LogLevel.Warn);
        }

        public void prepareJsonAssetsJSONs(String themeName)
        {
            foreach (string locale in CustomTranslator.getAllLocales())
            {
                if (locale == "default") continue;
                IDictionary<string, string> translationMap = new Dictionary<string, string>();
                foreach (string parameter in CustomTranslator.getAllParameters(locale))
                {
                    string parameterName = parameter.EndsWith(".label") ? parameter.Replace(".label", ".name") : parameter;
                    if (parameterName.EndsWith(".name") || parameterName.EndsWith(".description"))
                    {
                        translationMap.Add(parameterName, CustomTranslator.getTranslation(locale, parameter));
                        translationMap.Add("big-craftable." + parameterName, CustomTranslator.getTranslation(locale, parameter));
                    }
                }
                ModEntry.modHelper.Data.WriteJsonFile(
                    "assets/SeedMachines" + themeName + "JA/i18n/" + locale + ".json",
                    translationMap
                );
            }
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
                string bigCraftableId = jsonAssetsAPI.GetBigCraftableId(wrapperName);
                IBigCraftableWrapper.getWrapper(wrapperName).itemID = bigCraftableId;
            }
 
        }
    }
}
