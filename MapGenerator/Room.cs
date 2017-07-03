using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MapGenerator
{
  class Room
  {
    private int max_width;

    public int Max_width
    {
      get { return max_width; }
      set { max_width = value; }
    }

    private int max_height;

    public int Max_height
    {
      get { return max_height; }
      set { max_height = value; }
    }

    private Tile[,] room_map;

    public Tile[,] Room_map
    {
      get { return room_map; }
      set { room_map = value; }
    }


    public Room( int max_width, int max_height )
    {
      this.Max_height = max_height;
      this.Max_width = max_width;

      this.Generate_room( );
    }

    private void Generate_room()
    {
      room_map = new Tile[ Max_width, Max_height ];

      //generate_walls( );
      Generate_floor( );
    }

    public void Draw( SpriteBatch spriteBatch, SpriteFont sf )
    {
      for ( int x = 0; x < Max_width; x++ )
      {
        for ( int y = 0; y < Max_height; y++ )
        {
          Room_map[ x, y ].Draw( spriteBatch, x, y );
        }
      }
    }


    private void Generate_floor()
    {
      for ( int i = 1; i < Max_width - 1; i++ )
      {
        for ( int j = 1; j < Max_height - 1; j++ )
        {
          if ( Room_map[ i, j ] == null )
            Room_map[ i, j ] = Tile.Get_new_tile( TILE_TYPE.DIRT, i, j );
        }
      }
    }

    private void Generate_walls()
    {
      // Generate upper wall
      for ( int i = 1; i < Max_width - 1; i++ )
      {
        Room_map[ i, 1 ] = Tile.Get_new_tile( TILE_TYPE.GRAVEL, i, 0 );
      }

      // Generate lower wall
      for ( int i = 1; i < Max_width - 1; i++ )
      {
        Room_map[ i, Max_height - 2 ] = Tile.Get_new_tile( TILE_TYPE.GRAVEL, i, Max_height - 1 );
      }

      // Generate left wall
      for ( int i = 1; i < Max_height - 1; i++ )
      {
        Room_map[ 1, i ] = Tile.Get_new_tile( TILE_TYPE.GRAVEL, 0, i );
      }

      // Generate right wall
      for ( int i = 1; i < Max_height - 1; i++ )
      {
        Room_map[ Max_width - 2, i ] = Tile.Get_new_tile( TILE_TYPE.GRAVEL, Max_width - 1, i );
      }
    }
  }
}
