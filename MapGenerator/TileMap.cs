using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  public class TileMap
  {

    public Tile[,] tiles;

    public int Width
    {
      get { return tiles.GetLength( 0 ); }
    }

    public int Height
    {
      get { return tiles.GetLength( 1 ); }
    }

    public TileMap( int width, int height )
    {
      tiles = new Tile[ width, height ];
    }


    public void add_rooms( int v )
    {
      for ( int i = 0; i < v; i++ )
      {
        Room room = new Room( Generator.rand.Next( 2, 8 ) * 2 + 1, Generator.rand.Next( 2, 8 ) * 2 + 1 );

        this.add_array( room.Room_map, new Vector2( Generator.rand.Next( 0, this.Width / 2 - 1 ) * 2 + 1, Generator.rand.Next( 0, this.Height / 2 - 1 ) * 2 + 1 ) );
      }
    }

    public void Draw( SpriteBatch spriteBatch )
    {
      for ( int x = 0; x < Width; x++ )
      {
        for ( int y = 0; y < Height; y++ )
        {
          if ( tiles[ x, y ] != null )
            tiles[ x, y ].Draw( spriteBatch, x, y );
        }
      }
    }

    public Tile search_for_tile( TILE_TYPE tile_type )
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }

    public void fill_with_none()
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ] == null )
          {
            this.tiles[ i, j ] = Tile.get_new_tile( TILE_TYPE.NONE, i, j );
          }
        }
      }
    }


    public bool is_left_tile_in_main_region( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_right_tile_in_main_region( Tile tile )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_up_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_down_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_left_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_right_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_up_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_down_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public Tile search_for_tile_without_main_region( TILE_TYPE tile_type )
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type && this.tiles[ i, j ].in_main_region == false )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }


    public Tile search_for_tile_without_main_region_from_pos( TILE_TYPE tile_type, Tile from_tile )
    {
      int x = 0;
      int y = 0;
      if ( from_tile != null )
      {
        x = from_tile.X_Position;
        y = from_tile.Y_Position;
      }

      for ( int i = y; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = x; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type && this.tiles[ i, j ].in_main_region == false )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }


    internal void add_array( Tile[,] map_to_add, Vector2 startpos )
    {
      //check if array has enough space
      for ( int i = 0; i < map_to_add.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map_to_add.GetLength( 1 ); j++ )
        {
          if ( i + (int)startpos.X >= tiles.GetLength( 0 ) || j + (int)startpos.Y >= tiles.GetLength( 1 ) )
          {
            return;
          }
          if ( tiles[ i + (int)startpos.X, j + (int)startpos.Y ] != null )
          {
            return;
          }
        }
      }

      // Copy array in array
      for ( int i = 0; i < map_to_add.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map_to_add.GetLength( 1 ); j++ )
        {
          if ( map_to_add[ i, j ] != null )
          {
            map_to_add[ i, j ].position = new Vector2( ( i + (int)startpos.X ) * Global_Settings.tile_pixels, ( j + (int)startpos.Y ) * Global_Settings.tile_pixels );
            tiles[ i + (int)startpos.X, j + (int)startpos.Y ] = map_to_add[ i, j ];
          }
        }
      }
    }

    public bool is_within( int starta, int end, int value )
    {
      if ( value >= starta && value <= end )
      {
        return true;
      }
      return false;
    }


    public bool floodfill_is_left_free( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_right_free( Tile tile )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_left_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_right_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_right_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

    public bool floodfill_is_down_left_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

  }
}
