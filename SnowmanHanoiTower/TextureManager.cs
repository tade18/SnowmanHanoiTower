using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnowmanHanoiTower;

public static class TextureManager
{
    public static Texture2D Pixel { get; private set; }

    public static void Load(GraphicsDevice graphicsDevice)
    {
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }
}