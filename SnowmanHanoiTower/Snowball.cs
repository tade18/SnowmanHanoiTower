using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Snowball
{
    public Texture2D Texture { get; private set; }
    public Vector2 Position { get; set; }
    public int Width => Texture.Width;
    public int Height => Texture.Height;

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public Snowball(Texture2D texture)
    {
        Texture = texture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}