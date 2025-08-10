using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace SeedMachines.Framework.BigCraftables
{
    public abstract class IBigCraftableWrapper
    {
        private static Dictionary<string, IBigCraftableWrapper> wrappers = new Dictionary<string, IBigCraftableWrapper>();

        static IBigCraftableWrapper()
        {
            addWrapper(new SeedMachineWrapper());
            addWrapper(new SeedBanditWrapper());
        }

        private static void addWrapper(IBigCraftableWrapper wrapper)
        {
            wrappers.Add(wrapper.name, wrapper);
        }

        public static IBigCraftableWrapper getWrapper(string name)
        {
            return wrappers[name];
        }

        public static Dictionary<string, IBigCraftableWrapper> getAllWrappers()
        {
            return wrappers;
        }

        public static void checkAndInjectDynamicObject(OverlaidDictionary objects, Vector2 key)
        {
            if (!(objects[key] is IBigCraftable) && wrappers.ContainsKey(objects[key].name))
            {
                IBigCraftableWrapper wrapper = wrappers[objects[key].name];
                objects[key] = (IBigCraftable)Activator.CreateInstance(wrapper.dynamicObjectType, objects[key], wrapper);
            }
        }

        public static void checkAndRemoveDynamicObject(OverlaidDictionary objects, Vector2 key)
        {
            if (objects[key] is IBigCraftable)
            {
                objects[key] = ((IBigCraftable)objects[key]).baseObject;
            }
        }

        //-----------------

        public string itemID;
        public String name;
        public int price;
        public bool availableOutdoors;
        public bool availableIndoors;
        public int fragility;
        //public int isLamp;
        public int edibility = -300;
        public String typeAndCategory;

        public String ingredients;
        public String location = "Home";
        public String unlockConditions;

        public Type dynamicObjectType;
        public int maxAnimationIndex;
        public int millisecondsBetweenAnimation;

        //-----------------

        private String getTranslationBaseName()
        {
            return name.ToLower().Replace(' ', '-');
        }

        public String getDefaultLabel()
        {
            return CustomTranslator.getTranslation("default", getTranslationBaseName() + ".label");
        }
        public String getDefaultDescription()
        {
            return CustomTranslator.getTranslation("default", getTranslationBaseName() + ".description");
        }

        public IDictionary<String, String> getAllTranslationsForLabel()
        {
            return CustomTranslator.getAllTranslationsByLocales(getTranslationBaseName() + ".label");
        }
        public IDictionary<String, String> getAllTranslationsForDescription()
        {
            return CustomTranslator.getAllTranslationsByLocales(getTranslationBaseName() + ".description");
        }

        public JsonAssetsBigCraftableModel getJsonAssetsModel()
        {
            JsonAssetsBigCraftableModel result = new JsonAssetsBigCraftableModel();
            result.Name = getDefaultLabel();
            result.Description = getDefaultDescription();
            result.Price = price;
            result.IsDefault = true;
            result.ProvidesLight = false;
            result.ReserveExtraIndexCount = maxAnimationIndex;

            result.Recipe = new JsonAssetsBigCraftableRecipe();
            result.Recipe.IsDefault = true;
            result.Recipe.CanPurchase = false;
            result.Recipe.ResultCount = 1;
            result.Recipe.Ingredients = new List<JsonAssetsBigCraftableIngredient>();
            string[] splittedIngredients = ingredients.Split(' ');
            for (int i = 0; i < splittedIngredients.Length; i+=2)
            {
                JsonAssetsBigCraftableIngredient ingredient = new JsonAssetsBigCraftableIngredient();
                ingredient.Object = splittedIngredients[i];
                ingredient.Count = Int32.Parse(splittedIngredients[i+1]);
                result.Recipe.Ingredients.Add(ingredient);
            }
            
            //result.TranslationKey = getTranslationBaseName();
            result.NameLocalization = getAllTranslationsForLabel();
            result.DescriptionLocalization = getAllTranslationsForDescription();

            return result;
        }
    }
}
