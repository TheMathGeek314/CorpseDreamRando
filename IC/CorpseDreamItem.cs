using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace CorpseDreamRando {
    public class CorpseDreamItem: LoreItem {
        public CorpseDreamItem(string placementName, string key) {
            name = placementName;
            loreSheet = "Lore Tablets";
            loreKey = key;
            textType = TextType.Lore;

            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("corpse_pin");
            UIDef = new LoreUIDef {
                lore = new LanguageString("Lore Tablets", key),
                textType = TextType.LeftLore,
                name = new BoxedString(name.Replace("-", " - ").Replace("_", " ")),
                shopDesc = new BoxedString("This bug's final thoughts, their dying wish, now on sale for an incredible price!\r\n\r\nThis is a metaphor for capitalism."),
                sprite = new EmbeddedSprite("corpse_pin")
            };
        }
    }
}
