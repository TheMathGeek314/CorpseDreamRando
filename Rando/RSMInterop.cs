using System;
using System.Collections.Generic;
using System.Linq;
using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace CorpseDreamRando {
    internal static class RSMInterop {
        public static void Hook() {
            RandoSettingsManagerMod.Instance.RegisterConnection(new CorpseDreamSettingsProxy() {
                getter = () => CorpseDreamRando.Settings,
                setter = gs => CorpseDreamRando.Settings = gs
            });
        }
    }

    internal class CorpseDreamSettingsProxy: RandoSettingsProxy<GlobalSettings, Signature> {
        internal Func<GlobalSettings> getter;
        internal Action<GlobalSettings> setter;

        public override string ModKey => CorpseDreamRando.instance.GetName();

        public override VersioningPolicy<Signature> VersioningPolicy => new StructuralVersioningPolicy() { settingsGetter = getter };

        public override void ReceiveSettings(GlobalSettings settings) {
            setter(settings ??= new());
        }

        public override bool TryProvideSettings(out GlobalSettings settings) {
            settings = getter();
            return settings.Enabled;
        }
    }

    internal class StructuralVersioningPolicy: VersioningPolicy<Signature> {
        internal Func<GlobalSettings> settingsGetter;

        public override Signature Version => new() { FeatureSet = FeatureSetForSettings(settingsGetter()) };

        private static List<string> FeatureSetForSettings(GlobalSettings gs) => SupportedFeatures.Where(f => f.feature(gs)).Select(f => f.name).ToList();

        public override bool Allow(Signature s) => s.FeatureSet.All(name => SupportedFeatures.Any(sf => sf.name == name));

        public static List<(Predicate<GlobalSettings> feature, string name)> SupportedFeatures = new() {
            (gs => gs.Enabled, "CorpseDreamRando")
        };
    }

    internal struct Signature {
        public List<string> FeatureSet;
    }
}
