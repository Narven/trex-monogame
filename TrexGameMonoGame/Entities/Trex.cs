using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TrexGameMonoGame.Graphics;

namespace TrexGameMonoGame.Entities;

public class Trex : IGameEntity
{
  private const int TrexIdleBackgroundSpritePosX = 40;
  private const int TrexIdleBackgroundSpritePosY = 0;
  private const int TrexSpritePosX = 848;
  private const int TrexSpritePosY = 0;
  private const float BlinkAnimationRandomMin = 2f;
  private const float BlinkAnimationRandomMax = 10f;
  private const float BlinkDuration = 0.2f;
  private const float Gravity = 1600f;
  private const float JumpVelocity = -480f; // pixels per second
  private const float CancelJumpVelocity = -100f;
  private const float MinJumpHeight = 40f;
  private readonly Random _random;
  private SpriteAnimation _blinkAnimation;
  private Sprite _idleBackgroundSprite;

  private float _startPosition;

  public const int TrexSpriteWidth = 44;
  public const int TrexSpriteHeight = 52;


  public Vector2 Position { get; private set; }

  public TrexState State { get; private set; }

  public bool IsAlive { get; private set; }

  public float Speed { get; private set; }


  private readonly Sprite _idleSprite;
  private readonly Sprite _idleBlinkSprite;

  private float _verticalVelocity;


  public int DrawOrder { get; }

  private SoundEffect _jumpSound;

  public Trex(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound)
  {
    Position = position;
    _jumpSound = jumpSound;

    _idleBackgroundSprite = new Sprite(spriteSheet, TrexIdleBackgroundSpritePosX,
      TrexIdleBackgroundSpritePosY, TrexSpriteWidth, TrexSpriteHeight);
    State = TrexState.Idle;

    _random = new Random();

    _idleSprite = new Sprite(spriteSheet, TrexSpritePosX, TrexSpritePosY, TrexSpriteWidth,
      TrexSpriteHeight);

    _idleBlinkSprite = new Sprite(spriteSheet, TrexSpritePosX + TrexSpriteWidth, TrexSpritePosY,
      TrexSpriteWidth,
      TrexSpriteHeight);

    _blinkAnimation = new SpriteAnimation();

    CreateBlinkAnimation();
    _startPosition = Position.Y;
    _blinkAnimation.Play();
  }

  public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    if(State == TrexState.Idle)
    {
      _idleBackgroundSprite.Draw(spriteBatch, Position);
      _blinkAnimation.Draw(spriteBatch, Position);
    }
    else if(State == TrexState.Jumping || State == TrexState.Falling)
    {
      _idleSprite.Draw(spriteBatch, Position);
    }
  }

  public void Update(GameTime gameTime)
  {
    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

    if(State == TrexState.Idle)
    {
      if(!_blinkAnimation.IsPlaying)
      {
        CreateBlinkAnimation();
        _blinkAnimation.Play();
      }

      _blinkAnimation.Update(gameTime);
    }
    else if(State == TrexState.Jumping || State == TrexState.Falling)
    {
      Position = new Vector2(Position.X, Position.Y + _verticalVelocity * dt);
      _verticalVelocity += Gravity * dt;

      if(_verticalVelocity >= 0)
      {
        State = TrexState.Falling;
      }

      if(Position.Y >= _startPosition)
      {
        Position = new Vector2(Position.X, _startPosition);
        _verticalVelocity = 0f;
        State = TrexState.Idle;
      }
    }
  }

  private void CreateBlinkAnimation()
  {
    double blinkTimestamp = BlinkAnimationRandomMin +
                            _random.NextDouble() * (BlinkAnimationRandomMax - BlinkAnimationRandomMin);

    // Set idle animation
    _blinkAnimation.Clear();
    _blinkAnimation.AddFrame(_idleSprite, 0);
    _blinkAnimation.AddFrame(_idleBlinkSprite, (float)blinkTimestamp);
    _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimestamp + BlinkDuration);
    _blinkAnimation.Play(playOnce: true);
  }

  public bool BeginJump()
  {
    if(State == TrexState.Jumping || State == TrexState.Falling)
    {
      return false;
    }

    State = TrexState.Jumping;
    _jumpSound.Play();
    _verticalVelocity = JumpVelocity;

    return true;
  }

  public bool CancelJump()
  {
    if(State != TrexState.Jumping || (_startPosition - Position.Y) < MinJumpHeight) return false;

    State = TrexState.Falling;
    _verticalVelocity = _verticalVelocity < CancelJumpVelocity ? CancelJumpVelocity : 0f;

    return true;
  }
}
