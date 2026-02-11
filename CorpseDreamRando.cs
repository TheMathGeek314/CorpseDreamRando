using Modding;
using System.Collections.Generic;
using System.Reflection;

namespace CorpseDreamRando {
    public class CorpseDreamRando: Mod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "CorpseDreamRando";
        public override string GetVersion() => "1.0.0.0";

        public static GlobalSettings Settings { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => Settings = s;
        public GlobalSettings OnSaveGlobal() => Settings;

        internal static CorpseDreamRando instance;

        public CorpseDreamRando(): base(null) {
            instance = this;
        }

        public override void Initialize() {
            RandoInterop.Hook();
            HomothetyEasterEgg();
        }

        private void HomothetyEasterEgg() {
            Dictionary<string, Dictionary<string, string>> currentEntrySheets = typeof(Language.Language).GetField("currentEntrySheets", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, Dictionary<string, string>>;
            currentEntrySheets["Lore Tablets"]["MIMIC_CORPSE_07"] = "Dearest Homothety (AKA \"Moth\") (AKA \"Randoman\"): I am writing to inform you of a glaring error in your randomization algorithm for the game Hollow Knight.<page>Though I was assured that the item locations were random, and indeed was swayed by your very name, there was not one but TWO so-called \"vanilla\" locations. Please, I implore you, fix your game.";
        }
    }
}

//finish logic (including MoreStags interop either here or there)