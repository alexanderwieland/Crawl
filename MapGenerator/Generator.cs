using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  static class Global_Settings
  {
    public static int tile_pixels = 50;
    public static int draw_width = 1000;
    public static int draw_height = 800;
  }

  public enum TILE_TYPE
  {
    SAND = 0,
    DIRT = 1,
    GRASS = 2,
    GRAVEL = 3,
    NONE = 4,
    MAIN_REGION = 5
  };

  public enum Orientation
  {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
  }

  public enum TILE_ORIENTATION
  {
    NW,
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    CENTER
  };

  public class Generator
  {

    public static Random rand = new Random( );
    public static Dictionary<TILE_TYPE, Dictionary<TILE_ORIENTATION, Texture2D>> tiles;
    public ContentManager Content;

    

    FloodFiller floodfiller;
    RoomConnector roomconnector;

    public Generator( )
    {
      tiles = new Dictionary<TILE_TYPE, Dictionary<TILE_ORIENTATION, Texture2D>>( );
      floodfiller = new FloodFiller( );
      roomconnector = new RoomConnector( );
    }

    public TileMap get_new_dungeon_map( )
    {
      TileMap map = new TileMap( 150, 100 );

      map.add_rooms( 500 );
      map.fill_with_none( );
      floodfiller.init_flood_fill( map );
      roomconnector.connect( map );
      floodfiller.defill( map );

      return map;
    }
    

    public void AddTexture( MapGenerator.TILE_TYPE type, MapGenerator.TILE_ORIENTATION or, Texture2D texture )
    {
      tiles[ type ].Add( or, texture );
    }

    public void AddTextureType( MapGenerator.TILE_TYPE type )
    {
      tiles.Add( type, new Dictionary<TILE_ORIENTATION, Texture2D>( ) );
    }


    public void loadTextures()
    {
      AddTextureType( TILE_TYPE.DIRT );
      AddTextureType( TILE_TYPE.GRASS );
      AddTextureType( TILE_TYPE.GRAVEL );
      AddTextureType( TILE_TYPE.SAND );
      AddTextureType( TILE_TYPE.NONE );
      AddTextureType( TILE_TYPE.MAIN_REGION );

      AddTexture( TILE_TYPE.GRASS, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Map/Grass_in" ) );
      AddTexture( TILE_TYPE.DIRT, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Map/Dirt_in" ) );
      AddTexture( TILE_TYPE.SAND, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Map/Sand_in" ) );
      AddTexture( TILE_TYPE.GRAVEL, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Map/Gravel_in" ) );
      AddTexture( TILE_TYPE.MAIN_REGION, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Map/MainRegion" ) );
      tiles[ TILE_TYPE.NONE ].Add( TILE_ORIENTATION.CENTER, null );
    }

  }
}
