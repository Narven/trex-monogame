using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexGameMonoGame.Graphics;

public class Sprite
{
  public Texture2D Texture { get; private set; }

  public int X { get; set; }

  public int Y { get; set; }

  public int Width { get; set; }

  public int Height { get; set; }

  public Color TintColor { get; set; } = Color.White;

  /// <summary>
  /// Creates a Sprite
  /// </summary>
  /// <param name="texture"></param>
  /// <param name="x"></param>
  /// <param name="y"></param>
  /// <param name="width"></param>
  /// <param name="height"></param>
  public Sprite(Texture2D texture, int x, int y, int width, int height)
  {
    Texture = texture;
    X = x;
    Y = y;
    Width = width;
    Height = height;
  }

  public void Draw(SpriteBatch spriteBatch, Vector2 position)
  {
    spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), TintColor);
  }
}
