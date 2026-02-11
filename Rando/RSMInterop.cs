using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace CorpseDreamRando {
    internal static class RSMInterop {
        public static void Hook() {
            RandoSettingsManagerMod.Instance.RegisterConnection(new CorpseDreamSettingsProxy());
        }
    }

    internal class CorpseDreamSettingsProxy: RandoSettingsProxy<GlobalSettings, string> {
        public override string ModKey => CorpseDreamRando.instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; } = new EqualityVersioningPolicy<string>(CorpseDreamRando.instance.GetVersion());

        public override void ReceiveSettings(GlobalSettings settings) {
            settings ??= new();
            RandoMenuPage.Instance.cdMEF.SetMenuValues(settings);
        }

        public override bool TryProvideSettings(out GlobalSettings settings) {
            settings = CorpseDreamRando.Settings;
            return settings.Enabled;
        }
    }
}
