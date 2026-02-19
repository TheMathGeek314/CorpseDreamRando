using Modding;
using System.IO;
using System.Linq;
using System.Reflection;
using ItemChanger;
using ItemChanger.Tags;
using MenuChanger;
using MenuChanger.MenuElements;
using ItemChanger.UIDefs;

namespace CorpseDreamRando {
    internal static class RandoInterop {
        public static void Hook() {
            RandomizerMod.Menu.RandomizerMenuAPI.AddMenuPage(_ => { }, BuildConnectionMenuButton);
            RequestModifier.Hook();
            LogicAdder.Hook();

            DefineLocations();
            DefineItems();

            if(ModHooks.GetMod("RandoSettingsManager") is Mod) {
                RSMInterop.Hook();
            }
        }

        private static bool BuildConnectionMenuButton(MenuPage landingPage, out SmallButton settingsButton) {
            SmallButton button = new(landingPage, "CorpseDreamRando");

            void UpdateButtonColor() {
                button.Text.color = CorpseDreamRando.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }

            UpdateButtonColor();
            button.OnClick += () => {
                CorpseDreamRando.Settings.Enabled = !CorpseDreamRando.Settings.Enabled;
                UpdateButtonColor();
            };
            settingsButton = button;
            return true;
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
                CorpseDreamItem cdItem = new() {
                    name = data.placementName,
                    loreSheet = "Lore Tablets",
                    loreKey = data.key,
                    textType = TextType.LeftLore,
                    UIDef = new LoreUIDef {
                        lore = new LanguageString("Lore Tablets", data.key),
                        textType = TextType.LeftLore,
                        name = new BoxedString(data.placementName.Replace("-", " - ").Replace("_", " ")),
                        shopDesc = new BoxedString("This bug's final thoughts, their dying wish, now on sale for an incredible price!\r\n\r\nThis is a metaphor for capitalism."),
                        sprite = new EmbeddedSprite("corpse_pin")
                    }
                };
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
