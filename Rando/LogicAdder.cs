using System.IO;
using RandomizerCore.Json;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace CorpseDreamRando {
    public static class LogicAdder {
        public static void Hook() {
            RCData.RuntimeLogicOverride.Subscribe(50, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb) {
            if(!CorpseDreamRando.Settings.Enabled)
                return;
            JsonLogicFormat fmt = new();
            using Stream s = typeof(LogicAdder).Assembly.GetManifestResourceStream("CorpseDreamRando.Resources.logic.json");
            lmb.DeserializeFile(LogicFileType.Locations, fmt, s);

            DefineItems(lmb, fmt);
        }

        private static void DefineItems(LogicManagerBuilder lmb, JsonLogicFormat fmt) {
            foreach(CorpseCoords data in CorpseCoords.masterList) {
                lmb.AddItem(new EmptyItem(data.placementName));
            }
        }
    }
}
