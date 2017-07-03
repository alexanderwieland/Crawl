using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MapGenerator;

namespace DnDCrawl
{
  internal class SinglePlayer
  {
    private GameManager gameManager;
    private TileMap dungeon_map;

    private int old_zoom = 0;

    public SinglePlayer( GameManager gameManager )
    {
      
      this.gameManager = gameManager;
      this.dungeon_map = gameManager.map_generator.Get_new_dungeon_map( );

    }
    

    private void Check_user_input()
    {
      if ( GameManager.mouse.ScrollWheelValue > old_zoom  )
      {
        gameManager.camera.Zoom += 0.05f;
      }
      if ( GameManager.mouse.ScrollWheelValue < old_zoom )
      {
        gameManager.camera.Zoom -= 0.05f;
      }
      if ( GameManager.keyboard.IsKeyDown(  Keys.Right ) || GameManager.keyboard.IsKeyDown( Keys.D ) )
      {
        gameManager.camera.Move( new Vector2( 25f, 0f ) );
      }
      if ( GameManager.keyboard.IsKeyDown( Keys.Left ) || GameManager.keyboard.IsKeyDown( Keys.A ) )
      {
        gameManager.camera.Move( new Vector2( -25f, 0f ) );
      }
      if ( GameManager.keyboard.IsKeyDown( Keys.Up ) || GameManager.keyboard.IsKeyDown( Keys.W ) )
      {
        gameManager.camera.Move( new Vector2( 0f, -25f ) );
      }
      if ( GameManager.keyboard.IsKeyDown( Keys.Down ) || GameManager.keyboard.IsKeyDown( Keys.S ) )
      {
        gameManager.camera.Move( new Vector2( 0f, 25f ) );
      }

      old_zoom = GameManager.mouse.ScrollWheelValue;
    }

    public void Update( )
    {
      Check_user_input( );
    }

    public void Draw( GameTime gameTime )
    {
      gameManager.GraphicsDevice.Clear( Color.Black );

      gameManager.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, gameManager.camera.Get_transformation( gameManager.GraphicsDevice ) );

      dungeon_map.Draw( gameManager.spriteBatch );

      gameManager.spriteBatch.End( );
      
    }

  }
}