using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SnowmanHanoiTower;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<Tower> towers;
    private Texture2D[] snowballTextures;
    private Texture2D backgroundTexture;
    private SpriteFont font;
    private Snowball heldSnowball = null;
    private Tower selectedTower = null;
    private ScoreManager scoreManager;
    protected bool isGameWon = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 576;
        _graphics.ApplyChanges();

        towers = new List<Tower>
        {
            new Tower(new Vector2(150, 500)),
            new Tower(new Vector2(500, 500)),
            new Tower(new Vector2(850, 500)),
        };

        scoreManager = new ScoreManager();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureManager.Load(GraphicsDevice);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("font");
        backgroundTexture = Content.Load<Texture2D>("snowBackground");

        snowballTextures = new Texture2D[5];
        for (int i = 0; i < 5; i++)
        {
            snowballTextures[i] = Content.Load<Texture2D>($"snowball{5 - i}");
        }

        for (int i = snowballTextures.Length; i > 0; i--)
        {
            towers[0].Snowballs.Add(new Snowball(snowballTextures[i - 1]));
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (isGameWon) return;

        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed && heldSnowball == null)
        {
            foreach (var tower in towers)
            {
                if (tower.Snowballs.Count > 0)
                {
                    var topSnowball = tower.Snowballs[^1];
                    if (topSnowball.BoundingBox.Contains(mouseState.Position))
                    {
                        heldSnowball = topSnowball;
                        tower.Snowballs.RemoveAt(tower.Snowballs.Count - 1);
                        selectedTower = tower;
                        break;
                    }
                }
            }
        }

        if (mouseState.LeftButton == ButtonState.Released && heldSnowball != null)
        {
            bool placed = false;

            foreach (var tower in towers)
            {
                if (tower.BoundingBox.Contains(mouseState.Position))
                {
                    if (tower.Snowballs.Count == 0 || tower.Snowballs[^1].Width > heldSnowball.Width)
                    {
                        tower.Snowballs.Add(heldSnowball);
                        scoreManager.IncreaseScore();
                        placed = true;
                        break;
                    }
                }
            }

            if (!placed && selectedTower != null)
            {
                selectedTower.Snowballs.Add(heldSnowball);
            }

            heldSnowball = null;
            selectedTower = null;

            CheckForWin();
        }

        base.Update(gameTime);
    }
    
    private void CheckForWin()
    {
        var lastTower = towers[2];
        if (lastTower.Snowballs.Count == 5)
        {
            bool isCorrectOrder = true;
            for (int i = 0; i < lastTower.Snowballs.Count - 1; i++)
            {
                if (lastTower.Snowballs[i].Width < lastTower.Snowballs[i + 1].Width)
                {
                    isCorrectOrder = false;
                    break;
                }
            }

            if (isCorrectOrder)
            {
                isGameWon = true;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
        _spriteBatch.DrawString(font, "Score: " + scoreManager.Score, new Vector2(100, 100), Color.Black);

        foreach (var tower in towers)
        {
            tower.Draw(_spriteBatch);
        }

        if (heldSnowball != null)
        {
            var mouseState = Mouse.GetState();
            heldSnowball.Position = new Vector2(mouseState.X - heldSnowball.Width / 2, mouseState.Y - heldSnowball.Height / 2);
            heldSnowball.Draw(_spriteBatch);
        }

        
        if (isGameWon)
        {
            DrawWinMessage();
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawWinMessage()
    {
        var message = "Well Done!";
        var messageSize = font.MeasureString(message);
        var position = new Vector2(
            (_graphics.PreferredBackBufferWidth - messageSize.X) / 2,
            (_graphics.PreferredBackBufferHeight - messageSize.Y) / 2
        );
        
        var backgroundRect = new Rectangle((int)position.X - 20, (int)position.Y - 10, (int)messageSize.X + 40, (int)messageSize.Y + 20);
        _spriteBatch.Draw(TextureManager.Pixel, backgroundRect, Color.Black * 0.75f);
        
        _spriteBatch.DrawString(font, message, position, Color.White);
    }
    }