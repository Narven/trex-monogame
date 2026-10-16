using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexGameMonoGame.Entities;

public interface IGameEntity
{
  int DrawOrder { get; }

  public void Update(GameTime gameTime);

  public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}
