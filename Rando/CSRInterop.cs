using ConnectionSettingsRando;

namespace CorpseDreamRando {
    internal class CSRInterop {
        public static void Hook() {
            CSR.Register(
                CorpseDreamRando.instance.GetName(),
                () => CorpseDreamRando.Settings,
                s => SettingsRandomizer.CopyTo(s, CorpseDreamRando.Settings)
            );
        }
    }
}
