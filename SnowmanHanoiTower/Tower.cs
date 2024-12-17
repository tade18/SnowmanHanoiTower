using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SnowmanHanoiTower;

public class Tower
{
    public List<Snowball> Snowballs { get; private set; }
    public Vector2 Position { get; private set; }

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y - 200, 100, 500);

    public Tower(Vector2 position)
    {
        Position = position;
        Snowballs = new List<Snowball>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureManager.Pixel, new Rectangle((int)Position.X, (int)Position.Y, 100, 20), Color.BurlyWood);
        spriteBatch.Draw(TextureManager.Pixel, new Rectangle((int)Position.X + 45, (int)Position.Y - 150, 10, 150), Color.Brown);

        int yOffset = -10;
        foreach (var snowball in Snowballs)
        {
            snowball.Position = new Vector2(Position.X + (100 - snowball.Width) / 2, Position.Y - snowball.Height - yOffset);
            snowball.Draw(spriteBatch);
            yOffset += snowball.Height - 15;
        }
    }
}