using System.Collections.Generic;

namespace CorpseDreamRando {
    public class CorpseCoords {
        public static Dictionary<(string, string), string> nameToPlacement = new();
        public static List<CorpseCoords> masterList = new();

        public string name;
        public string objectName;
        public string scene;
        public string pinScene;
        public string key;
        public float x;
        public float y;

        public string placementName;

        public void translate() {
            placementName = $"Corpse_Dream-{name}";
            nameToPlacement.Add((scene, objectName), placementName);
            masterList.Add(this);
        }
    }
}
