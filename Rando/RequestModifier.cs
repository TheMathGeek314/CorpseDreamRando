using Modding;
using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace CorpseDreamRando {
    public class RequestModifier {
        private static bool loreExists = false;

        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyDreamDefs);
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
            RequestBuilder.OnUpdate.Subscribe(-499.5f, DefinePools);
            RequestBuilder.OnUpdate.Subscribe(-1000, CheckLoreRando);
        }

        public static void ApplyDreamDefs(RequestBuilder rb) {
            if(!CorpseDreamRando.Settings.Enabled)
                return;
            foreach(CorpseCoords data in CorpseCoords.masterList) {
                string name = data.placementName;
                if(loreExists && (data.name == "Lighthouse" || data.name == "Basin_Key"))
                    continue;
                rb.AddLocationByName(name);
                rb.EditLocationRequest(name, info => {
                    info.customPlacementFetch = (factory, placement) => {
                        if(factory.TryFetchPlacement(name, out AbstractPlacement ap))
                            return ap;
                        AbstractLocation absLoc = Finder.GetLocation(name);
                        absLoc.flingType = FlingType.DirectDeposit;
                        AbstractPlacement absPla = absLoc.Wrap();
                        factory.AddPlacement(absPla);
                        return absPla;
                    };
                    info.getLocationDef = () => new() {
                        Name = name,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false,
                        SceneName = data.scene
                    };
                });
            }
        }

        private static void SetupItems(RequestBuilder rb) {
            if(!CorpseDreamRando.Settings.Enabled)
                return;
            foreach(CorpseCoords data in CorpseCoords.masterList) {
                if(loreExists && (data.name == "Lighthouse" || data.name == "Basin_Key"))
                    continue;
                rb.EditItemRequest(data.placementName, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = data.placementName,
                        Pool = PoolNames.Lore,
                        MajorItem = false,
                        PriceCap = 1
                    };
                });
                rb.AddItemByName(data.placementName);
            }
        }

        private static void DefinePools(RequestBuilder rb) {
            if(!CorpseDreamRando.Settings.Enabled)
                return;

            rb.OnGetGroupFor.Subscribe(0.01f, ResolveCorpseDreamGroup);
            bool ResolveCorpseDreamGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb) {
                if(type == RequestBuilder.ElementType.Item || type == RequestBuilder.ElementType.Location) {
                    if(item.StartsWith("Corpse_Dream-")) {
                        gb = rb.GetGroupFor(LocationNames.Lore_Tablet_Ancient_Basin);
                        return true;
                    }
                }
                gb = default;
                return false;
            }
        }

        private static void CheckLoreRando(RequestBuilder rb) {
            loreExists = ModHooks.GetMod("LoreRandomizer") is Mod ? checkLoreExists() : false;
        }
        
        private static bool checkLoreExists() {
            LoreRandomizer.Menu.RandoSettings rs = LoreRandomizer.LoreRandomizer.RandoSettings;
            return rs.Enabled && rs.RandomizeDreamNailDialogue;
        }
    }
}
