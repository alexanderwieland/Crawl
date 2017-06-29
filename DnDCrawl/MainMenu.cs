using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDCrawl
{
  class MainMenu
  {

    MenuButton buttonSP;
    MenuButton buttonMP;
    MenuButton buttonHS;
    MenuButton buttonEX;

    Texture2D menubackground;
    Rectangle menurec;
    Texture2D title;
    Rectangle titlerec;
    Texture2D hsbackground;

    GameManager game;

    public MainMenu( GameManager game )
    {
      this.game = game;
    }

    public void LoadContent( )
    {
      buttonSP = new MenuButton( game.Content.Load<Texture2D>( "MainMenu/SPButton" ), game.graphics.GraphicsDevice );
      buttonMP = new MenuButton( game.Content.Load<Texture2D>( "MainMenu/MPButton" ), game.graphics.GraphicsDevice );
      buttonHS = new MenuButton( game.Content.Load<Texture2D>( "MainMenu/HSButton" ), game.graphics.GraphicsDevice );
      buttonEX = new MenuButton( game.Content.Load<Texture2D>( "MainMenu/EXButton" ), game.graphics.GraphicsDevice );

      buttonSP.SetPosition( new Vector2( 60, 120 ) );
      buttonMP.SetPosition( new Vector2( 60, 180 ) );
      buttonHS.SetPosition( new Vector2( 60, 240 ) );
      buttonEX.SetPosition( new Vector2( 60, 300 ) );

      menubackground = game.Content.Load<Texture2D>( "MainMenu/MenuBG" );
      menurec = new Rectangle( 0, 0, Global_Settings.draw_width, Global_Settings.draw_height );

      title = game.Content.Load<Texture2D>( "MainMenu/Ueberschrift" );
      titlerec = new Rectangle( 60, 20, title.Width, title.Height );
      

      //spPlayer = new Player(Content, SignedInGamer.SignedInGamers[0].DisplayName);
      //spPlayer = new Player(Content, "Philo");

      //vid = Content.Load<Video>("Intro");
      //vidRect = new Rectangle(0, 0, screenwith, screenheight);
      //if(CurrentGameState == GameState.Video)
      //vidPlayer.Play(vid);

    }

    internal void Update( GameTime gameTime )
    {

      
      if ( buttonSP.isClicked == true )
      {
        GameManager.CurrentGameState = GameManager.GameState.SinglePlayer;
        game.graphics.ApplyChanges( );
      }

      if ( buttonEX.isClicked == true )
        GameManager.CurrentGameState = GameManager.GameState.Exiting;


      buttonSP.Update( );
      buttonMP.Update( );
      buttonHS.Update( );
      buttonEX.Update( );

      
    }

    internal void Draw(  )
    {
      game.spriteBatch.Begin( );

      game.spriteBatch.Draw( menubackground, menurec, Color.White );
      game.spriteBatch.Draw( title, titlerec, Color.White );

      buttonSP.Draw( game.spriteBatch );
      buttonMP.Draw( game.spriteBatch );
      buttonHS.Draw( game.spriteBatch );
      buttonEX.Draw( game.spriteBatch );

      game.spriteBatch.End( );
    }
  }
}
