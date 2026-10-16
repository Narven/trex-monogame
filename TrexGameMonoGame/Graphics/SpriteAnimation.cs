using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexGameMonoGame.Graphics;

public class SpriteAnimation
{
  private List<SpriteAnimationFrame> _frames = new();

  public bool PlayOnce { get; set; }

  public bool IsPlaying { get; private set; }

  public float PlaybackProgress { get; private set; }

  public SpriteAnimationFrame this[int index] => GetFrame(index);

  public SpriteAnimationFrame CurrentFrame
  {
    get
    {
      return _frames
        .Where(f => f.TimeStamp <= PlaybackProgress)
        .OrderBy(f => f.TimeStamp)
        .LastOrDefault();
    }
  }

  public float Duration
  {
    get
    {
      if(_frames.Count == 0) return 0;
      return _frames.Max(f => f.TimeStamp);
    }
  }

  public void AddFrame(Sprite sprite, float timeStamp)
  {
    SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
    _frames.Add(frame);
  }

  public void Update(GameTime gametTime)
  {
    if(!IsPlaying) return;

    PlaybackProgress += (float)gametTime.ElapsedGameTime.TotalSeconds;

    if(PlaybackProgress > Duration)
    {
      if(!PlayOnce)
      {
        PlaybackProgress -= Duration;
      }
      else
      {
        Stop();
      }
    }
  }

  public void Draw(SpriteBatch spriteBatch, Vector2 position)
  {
    SpriteAnimationFrame frame = CurrentFrame;
    if(frame != null)
    {
      frame.Sprite.Draw(spriteBatch, position);
    }
  }

  public void Play(bool playOnce = false)
  {
    IsPlaying = true;
    PlayOnce = playOnce;
  }

  public void Stop()
  {
    IsPlaying = false;
    PlaybackProgress = 0;
  }

  public SpriteAnimationFrame GetFrame(int index)
  {
    if(index < 0 || index >= _frames.Count)
    {
      throw new ArgumentOutOfRangeException(nameof(index),
        $"The frame with index {index}, does not exist in this animation.");
    }

    return _frames[index];
  }

  public void Clear()
  {
    Stop();
    _frames.Clear();
  }
}
