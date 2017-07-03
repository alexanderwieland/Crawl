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


    public void Add_rooms( int v )
    {
      for ( int i = 0; i < v; i++ )
      {
        Room room = new Room( Generator.rand.Next( 2, 8 ) * 2 + 1, Generator.rand.Next( 2, 8 ) * 2 + 1 );

        this.Add_array( room.Room_map, new Vector2( Generator.rand.Next( 0, this.Width / 2 - 1 ) * 2 + 1, Generator.rand.Next( 0, this.Height / 2 - 1 ) * 2 + 1 ) );
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

    internal bool Check_n_Replace( Tile tile, int[,] fig, TILE_TYPE type )
    {
      // Geht es sich von den lengen her aus?
      if ( tile.X_Position + fig.GetLength( 0 ) > this.Width || tile.Y_Position + fig.GetLength( 1 ) > this.Height )
        return false;

      // Matcht das erste Tile?
      if ( tile.type != type )
        return false;

      // Match eines nicht?
      for ( int i = 0; i < fig.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < fig.GetLength( 1 ); j++ )
        {
          if ( fig[ i, j ] == 0 && this.tiles[ tile.X_Position + i, tile.Y_Position + j ].type != TILE_TYPE.NONE )
            return false;
          if ( fig[ i, j ] == 1 && this.tiles[ tile.X_Position + i, tile.Y_Position + j ].type != type )
            return false;
          if ( fig[ i, j ] == 2 && this.tiles[ tile.X_Position + i, tile.Y_Position + j ].type != TILE_TYPE.NONE )
            return false;
          if ( fig[ i, j ] == 3 && this.tiles[ tile.X_Position + i, tile.Y_Position + j ].type != type  )
            return false;

          if ( fig[ i, j ] == 1 )//|| fig[ i, j ] == 3 )
          {
            // bei 1 und 3 muss i schaun ob die eh 2 freie haben
            if ( this.Has_2_empty( this.tiles[ tile.X_Position + i, tile.Y_Position + j ] ) == false )
            {
              return false;
            }
          }
        }
      }

      // Ersetzen
      for ( int i = 0; i < fig.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < fig.GetLength( 1 ); j++ )
        {
          if ( fig[ i, j ] == 1 )
          {
            this.tiles[ tile.X_Position + i, tile.Y_Position + j ].Change_biom( TILE_TYPE.NONE );
            this.tiles[ tile.X_Position + i, tile.Y_Position + j ].in_main_region = true;
          }

          if ( fig[ i, j ] == 2 )
          {
            this.tiles[ tile.X_Position + i, tile.Y_Position + j ].Change_biom( type );
            //this.tiles[ tile.X_Position + i, tile.Y_Position + j ].in_main_region = true;
          }

          if ( fig[ i, j ] == 3 )
          {
            this.tiles[ tile.X_Position + i, tile.Y_Position + j ].in_main_region = false;
          }

        }
      }

      return true;
    }


    private bool Has_2_empty( Tile tile )
    {

      int empty_counter = 0;

      if ( this.Is_down_free( tile ) )
      {
        empty_counter++;
      }
      if ( this.Is_left_free( tile ) )
      {
        empty_counter++;
      }
      if ( this.Is_right_free( tile ) )
      {
        empty_counter++;
      }
      if ( this.Is_up_free( tile ) )
      {
        empty_counter++;
      }

      if ( empty_counter == 2 )
      {
        return true;
      }

      return false;
    }

    public Tile Search_for_tile( TILE_TYPE tile_type )
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

    public void Fill_with_none()
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ] == null )
          {
            this.tiles[ i, j ] = Tile.Get_new_tile( TILE_TYPE.NONE, i, j );
          }
        }
      }
    }


    public bool Is_left_tile_in_main_region( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool Is_right_tile_in_main_region( Tile tile )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool Is_up_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool Is_down_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool Is_left_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool Is_right_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool Is_up_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool Is_down_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public Tile Search_for_tile_without_main_region( TILE_TYPE tile_type )
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


    public Tile Search_for_tile_without_main_region_from_pos( TILE_TYPE tile_type, Tile from_tile )
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


    internal void Add_array( Tile[,] map_to_add, Vector2 startpos )
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
            map_to_add[ i, j ].position = new Vector2( ( i + (int)startpos.X ), ( j + (int)startpos.Y ) );
            tiles[ i + (int)startpos.X, j + (int)startpos.Y ] = map_to_add[ i, j ];
          }
        }
      }
    }

    public bool Is_within( int starta, int end, int value )
    {
      if ( value >= starta && value <= end )
      {
        return true;
      }
      return false;
    }


    public bool Is_left_free( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_right_free( Tile tile )
    {
      if ( tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_up_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && this.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_down_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && this.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_up_left_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_up_right_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool Is_down_right_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && tile.X_Position + 1 < this.tiles.GetLength( 0 ) && this.tiles[ tile.X_Position + 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

    public bool Is_down_left_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < this.tiles.GetLength( 1 ) && tile.X_Position - 1 > 0 && this.tiles[ tile.X_Position - 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

  }
}
