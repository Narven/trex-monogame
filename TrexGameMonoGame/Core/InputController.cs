using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrexGameMonoGame.Entities;

namespace TrexGameMonoGame.Core;

public class InputController
{
  private Trex _trex;

  private KeyboardState _previousKeyboardState;

  public InputController(Trex trex)
  {
    _trex = trex;
  }

  public void ProcessControl(GameTime gameTime)
  {
    KeyboardState keyboardState = Keyboard.GetState();

    if(!_previousKeyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Up))
    {
      if(_trex.State != TrexState.Jumping)
      {
        _trex.BeginJump();
      }
    }
    else if(_trex.State == TrexState.Jumping && !keyboardState.IsKeyDown(Keys.Up))
    {
      _trex.CancelJump();
    }

    _previousKeyboardState = keyboardState;
  }
}
