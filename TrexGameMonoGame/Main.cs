using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexGameMonoGame.Core;
using TrexGameMonoGame.Entities;
using TrexGameMonoGame.Graphics;

namespace TrexGameMonoGame;

public class Main : Game
{
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  public const int WINDOW_WIDTH = 600;
  public const int WINDOW_HEIGHT = 150;

  public const int TREX_START_POS_Y = WINDOW_HEIGHT - 16;
  public const int TREX_START_POS_X = 1;

  private const string ASSET_NAME_SPRITESHEET = "TrexSpritesheet";
  private const string ASSET_NAME_SFX_HIT = "hit";
  private const string ASSET_NAME_SFX_SCORE_REACHED = "score-reached";
  private const string ASSET_NAME_SFX_BUTTON_PRESSED = "button-press";

  private SoundEffect _sfxHit;
  private SoundEffect _sfxButtonPress;
  private SoundEffect _sfxScoreReached;

  private Texture2D _spriteSheetTexture;

  private Trex _trex;

  private InputController _inputController;

  public Main()
  {
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  protected override void Initialize()
  {
    base.Initialize();

    _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
    _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
    _graphics.ApplyChanges();
  }

  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    // Audio
    _sfxButtonPress = Content.Load<SoundEffect>(ASSET_NAME_SFX_BUTTON_PRESSED);
    _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_SFX_HIT);
    _sfxScoreReached = Content.Load<SoundEffect>(ASSET_NAME_SFX_SCORE_REACHED);

    // Textures
    _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

    // Entities
    _trex = new Trex(
      _spriteSheetTexture,
      new Vector2(TREX_START_POS_X, TREX_START_POS_Y - Trex.TrexSpriteHeight),
      _sfxButtonPress
    );
    _inputController = new InputController(_trex);
  }

  protected override void Update(GameTime gameTime)
  {
    if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
       Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    _trex.Update(gameTime);
    _inputController.ProcessControl(gameTime);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.AliceBlue);

    // crips sharp, no blur on sprites
    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

    _trex.Draw(_spriteBatch, gameTime);

    _spriteBatch.End();
    base.Draw(gameTime);
  }
}
