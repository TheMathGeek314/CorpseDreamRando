using Modding;
using System.IO;
using System.Linq;
using System.Reflection;
using ItemChanger;
using ItemChanger.Tags;

namespace CorpseDreamRando {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            DefineLocations();
            DefineItems();

            if(ModHooks.GetMod("RandoSettingsManager") is Mod) {
                RSMInterop.Hook();
            }
        }

        public static void DefineLocations() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string cdCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("CorpseDreamData.json"));
            using Stream cdStream = assembly.GetManifestResourceStream(cdCoords);
            foreach(CorpseCoords data in new ParseJson(cdStream).parseFile<CorpseCoords>())
                data.translate();

            foreach(CorpseCoords data in CorpseCoords.masterList) {
                CorpseDreamLocation cdLoc = new() { name = data.placementName, sceneName = data.scene };
                InteropTag tag = AddTag(cdLoc);
                tag.Properties["PinSprite"] = new EmbeddedSprite("corpse_pin");
                tag.Properties["WorldMapLocation"] = (string.IsNullOrEmpty(data.pinScene) ? data.scene : data.pinScene, data.x, data.y);
                Finder.DefineCustomLocation(cdLoc);
            }
        }

        public static void DefineItems() {
            foreach(CorpseCoords data in CorpseCoords.masterList) {
                CorpseDreamItem cdItem = new(data.placementName, data.key);
                Finder.DefineCustomItem(cdItem);
            }
        }

        public static InteropTag AddTag(TaggableObject obj) {
            InteropTag tag = obj.GetOrAddTag<InteropTag>();
            tag.Message = "RandoSupplementalMetadata";
            tag.Properties["ModSource"] = CorpseDreamRando.instance.GetName();
            return tag;
        }
    }
}
