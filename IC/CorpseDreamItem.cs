using ItemChanger.Items;
using ItemChanger.Tags;

namespace CorpseDreamRando {
    public class CorpseDreamItem: LoreItem {
        public CorpseDreamItem() {
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("corpse_pin");
        }
    }
}
