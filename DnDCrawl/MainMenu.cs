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
    MenuText TextSP;
    MenuText TextMP;
    MenuText TextHS;
    MenuText TextEX;

    Texture2D menubackground;
    Rectangle menurec;
    MenuText title;
    
    SpriteFont mainmenu_font;

    GameManager game;

    public MainMenu( GameManager game )
    {
      this.game = game;
    }

    public void LoadContent( )
    {
      mainmenu_font = game.Content.Load<SpriteFont>( "Arial" );

      TextSP = new MenuText( "Single Player", mainmenu_font, game.graphics.GraphicsDevice );
      TextMP = new MenuText( "Multi Player", mainmenu_font, game.graphics.GraphicsDevice );
      TextHS = new MenuText( "Highscore", mainmenu_font, game.graphics.GraphicsDevice );
      TextEX = new MenuText( "Exit", mainmenu_font, game.graphics.GraphicsDevice );
      title = new MenuText( "Crawl", mainmenu_font, game.graphics.GraphicsDevice );

      title.SetPosition( new Vector2( 60, 40 ) );
      TextSP.SetPosition( new Vector2( 60, 120 ) );
      TextMP.SetPosition( new Vector2( 60, 180 ) );
      TextHS.SetPosition( new Vector2( 60, 240 ) );
      TextEX.SetPosition( new Vector2( 60, 300 ) );
      

      menubackground = game.Content.Load<Texture2D>( "MainMenu/MenuBG" );
      menurec = new Rectangle( 0, 0, Global_Settings.draw_width, Global_Settings.draw_height );
            

      //spPlayer = new Player(Content, SignedInGamer.SignedInGamers[0].DisplayName);
      //spPlayer = new Player(Content, "Philo");

      //vid = Content.Load<Video>("Intro");
      //vidRect = new Rectangle(0, 0, screenwith, screenheight);
      //if(CurrentGameState == GameState.Video)
      //vidPlayer.Play(vid);

    }

    internal void Update( GameTime gameTime )
    {            
      if ( TextSP.isClicked == true )
      {
        GameManager.CurrentGameState = GameManager.GameState.SinglePlayer;
        game.graphics.ApplyChanges( );
      }

      if ( TextEX.isClicked == true )
        GameManager.CurrentGameState = GameManager.GameState.Exiting;
      
      TextSP.Update( );
      TextMP.Update( );
      TextHS.Update( );
      TextEX.Update( );
      title.Update( );
    }

    internal void Draw(  )
    {
      game.spriteBatch.Begin( );

      game.spriteBatch.Draw( menubackground, menurec, Color.White );

      title.Draw( game.spriteBatch );
      TextSP.Draw( game.spriteBatch );
      TextMP.Draw( game.spriteBatch );
      TextHS.Draw( game.spriteBatch );
      TextEX.Draw( game.spriteBatch );
      title.Draw( game.spriteBatch );

      game.spriteBatch.End( );
    }
  }
}
