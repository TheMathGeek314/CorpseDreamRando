using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace CorpseDreamRando {
    public class RandoMenuPage {
        internal MenuPage CdRandoPage;
        internal MenuElementFactory<GlobalSettings> cdMEF;
        internal VerticalItemPanel cdVIP;

        internal SmallButton JumpToCdButton;

        internal static RandoMenuPage Instance { get; private set; }

        public static void OnExitMenu() {
            Instance = null;
        }

        public static void Hook() {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button) {
            button = Instance.JumpToCdButton;
            return true;
        }

        private void SetTopLevelButtonColor() {
            if(JumpToCdButton != null) {
                JumpToCdButton.Text.color = CorpseDreamRando.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private static void ConstructMenu(MenuPage landingPage) => Instance = new(landingPage);

        private RandoMenuPage(MenuPage landingPage) {
            CdRandoPage = new MenuPage(Localize("CorpseDreamRando"), landingPage);
            cdMEF = new(CdRandoPage, CorpseDreamRando.Settings);
            cdVIP = new(CdRandoPage, new(0, 300), 75f, true, cdMEF.Elements);
            Localize(cdMEF);
            foreach(IValueElement e in cdMEF.Elements) {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            JumpToCdButton = new(landingPage, Localize("CorpseDreamRando"));
            JumpToCdButton.AddHideAndShowEvent(landingPage, CdRandoPage);
            SetTopLevelButtonColor();
        }
    }
}
