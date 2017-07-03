using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MapGenerator;

namespace DnDCrawl
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class GameManager : Game
  {

    public enum GameState
    {
      Video,
      MainMenu,
      HighScore,
      SinglePlayer,
      Multiplayer,
      Exiting
    }

    public static GameState CurrentGameState = GameState.MainMenu;
    public static MouseState mouse;
    public static KeyboardState keyboard;
    public GraphicsDeviceManager graphics;
    public SpriteBatch spriteBatch;

    MainMenu mainmenu;
    SinglePlayer singleplayer;
    public Generator map_generator;


    public Camera camera = new Camera( );

    public GameManager()
    {      

      this.map_generator = new Generator( );

      graphics = new GraphicsDeviceManager( this )
      {
        PreferredBackBufferWidth = Global_Settings.draw_width,
        PreferredBackBufferHeight = Global_Settings.draw_height
      };
      IsMouseVisible = true;

      mainmenu = new MainMenu( this );

      Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {

      this.spriteBatch = new SpriteBatch( this.GraphicsDevice );

      base.Initialize( );
    }
    
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch( GraphicsDevice );

      this.map_generator.Content = this.Content;
      this.map_generator.LoadTextures( );
      mainmenu.LoadContent( );

      // TODO: use this.Content to load your game content here
    }
    
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }
    
    protected override void Update( GameTime gameTime )
    {
      mouse = Mouse.GetState( );
      keyboard = Keyboard.GetState( );

      if ( Keyboard.GetState( ).IsKeyDown( Keys.Escape ) )
        Exit( );

      switch ( CurrentGameState )
      {
        case GameState.Video:
          //if (vidPlayer.State == MediaState.Stopped)
          CurrentGameState = GameState.MainMenu;
          break;
        case GameState.MainMenu:

          mainmenu.Update( gameTime );

          break;
        case GameState.SinglePlayer:
          if ( singleplayer == null )
          singleplayer = new SinglePlayer( this );

          singleplayer.Update( );

          break;
     
        case GameState.Exiting:
          this.Exit( );
          break;
      }

      if ( keyboard.IsKeyDown( Keys.Escape ) )
      {
        if ( CurrentGameState == GameState.MainMenu )
          CurrentGameState = GameState.Exiting;
        else
        {

        }
      }
      GraphicsDevice.Flush( );
      base.Update( gameTime );
    }
    
    protected override void Draw( GameTime gameTime )
    {
      GraphicsDevice.Clear( Color.Black );

      switch ( CurrentGameState )
      {
        case GameState.Video:
          //vidTexture = vidPlayer.GetTexture();
          //spriteBatch.Begin();

          //spriteBatch.Draw(vidTexture, vidRect, Color.White);

          //spriteBatch.End();
          break;
        case GameState.MainMenu:
          this.mainmenu.Draw( );
          break;
        case GameState.SinglePlayer:

          if ( singleplayer == null )
            singleplayer = new SinglePlayer( this );

          singleplayer.Draw( gameTime );
          
          break;
        case GameState.Multiplayer:
       
          break;
        case GameState.HighScore:
        
          break;
        case GameState.Exiting:

          break;
      }


      base.Draw( gameTime );
    }

  }
}
