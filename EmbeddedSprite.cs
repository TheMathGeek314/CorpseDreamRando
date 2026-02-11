using System;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;

namespace CorpseDreamRando {
    [Serializable]
    public class EmbeddedSprite: ISprite {
        private static SpriteManager EmbeddedSpriteManager = new(typeof(EmbeddedSprite).Assembly, "CorpseDreamRando.Resources.");

        public string key;
        public EmbeddedSprite(string key) {
            this.key = key;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Sprite Value => EmbeddedSpriteManager.GetSprite(key);
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }
}
