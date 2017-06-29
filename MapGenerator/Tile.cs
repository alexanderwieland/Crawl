using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  public class Tile
  {
    public Vector2 position;

    public TILE_TYPE type;

    public TILE_ORIENTATION orientation;

    public Texture2D texture_in;
    public Texture2D texture_out;

    public int draw_height = 0;

    public int X_Position_in_pixel
    {
      get { return (int)position.X; }
    }
    public int Y_Position_in_pixel
    {
      get { return (int)position.Y; }
    }
    public int X_Position
    {
      get { return (int)position.X / Global_Settings.tile_pixels; }
    }
    public int Y_Position
    {
      get { return (int)position.Y / Global_Settings.tile_pixels; }
    }

    public bool in_main_region = false;
    public bool untouchable = false;

    public Tile( Texture2D texture_in, Texture2D texture_out, Vector2 position, TILE_TYPE type, TILE_ORIENTATION orientation )
    {
      this.texture_in = texture_in;
      this.texture_out = texture_out;
      this.position = position;
      this.orientation = orientation;
      this.type = type;
    }

    public static Tile get_new_tile( TILE_TYPE type, int x, int y )
    {
      return new Tile( Generator.tiles[ type ][ TILE_ORIENTATION.CENTER ], Generator.tiles[ type ][ TILE_ORIENTATION.CENTER ], new Vector2( x * Global_Settings.tile_pixels, y * Global_Settings.tile_pixels ), type, TILE_ORIENTATION.CENTER );
    }

    public void Draw( SpriteBatch spriteBatch, int a, int b )
    {
      Texture2D used_texture = null;
      Rectangle source_rect = Rectangle.Empty;

      if ( type == TILE_TYPE.NONE )
      {
        return;
      }
      get_source( out used_texture, out source_rect );
      spriteBatch.Draw( used_texture, position, source_rect, Color.White );

      if ( this.in_main_region )
      {
        //spriteBatch.Draw( Generator.tiles[TILE_TYPE.MAIN_REGION][TILE_ORIENTATION.CENTER], position, source_rect, Color.White );
      }
      //spriteBatch.DrawString(sf, a + ", " + b, position, Color.Black);
    }

    public void change_biom( TILE_TYPE type )
    {
      this.texture_in = Generator.tiles[ type ][ TILE_ORIENTATION.CENTER ];

      this.type = type;
    }

    private void get_source( out Texture2D used_texture, out Rectangle source_rect )
    {
      used_texture = texture_in;

      switch ( orientation )
      {
        case TILE_ORIENTATION.NW:
          source_rect = new Rectangle(
            0,
            0,
            Global_Settings.tile_pixels,
            Global_Settings.tile_pixels
            );
          break;
        case TILE_ORIENTATION.N:
          source_rect = new Rectangle(
            Global_Settings.tile_pixels * 1,
            Global_Settings.tile_pixels * 0,
            Global_Settings.tile_pixels,
            Global_Settings.tile_pixels
            );
          break;
        case TILE_ORIENTATION.NE:
          source_rect = new Rectangle(
            Global_Settings.tile_pixels * 2,
            Global_Settings.tile_pixels * 0,
            Global_Settings.tile_pixels,
            Global_Settings.tile_pixels
            );
          break;
        case TILE_ORIENTATION.E:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 2,
           Global_Settings.tile_pixels * 1,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        case TILE_ORIENTATION.SE:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 2,
           Global_Settings.tile_pixels * 2,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        case TILE_ORIENTATION.S:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 1,
           Global_Settings.tile_pixels * 2,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        case TILE_ORIENTATION.SW:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 0,
           Global_Settings.tile_pixels * 2,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        case TILE_ORIENTATION.W:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 0,
           Global_Settings.tile_pixels * 1,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        case TILE_ORIENTATION.CENTER:
          source_rect = new Rectangle(
           Global_Settings.tile_pixels * 1,
           Global_Settings.tile_pixels * 1,
           Global_Settings.tile_pixels,
           Global_Settings.tile_pixels
           );
          break;
        default:
          source_rect = Rectangle.Empty;
          break;
      }
    }
  }
}