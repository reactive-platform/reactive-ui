using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Reactive {
    [PublicAPI]
    public static class ReactiveUtils {
        #region Sprites

        public static Texture2D? CreateTexture(byte[] bytes) {
            if (bytes.Length == 0) return null;
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false, false);
            return texture.LoadImage(bytes) ? texture : null;
        }

        public static Sprite? CreateSprite(byte[] image) {
            return CreateTexture(image) is { } texture ? CreateSprite(texture) : null;
        }

        public static Sprite? CreateSprite(Texture2D texture) {
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(texture.width / 2f, texture.height / 2f)
            );
        }

        #endregion
    }
}