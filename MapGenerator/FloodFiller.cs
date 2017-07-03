using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  class FloodFiller
  {

    public FloodFiller(  )
    {

    }


    public void Fill_with_gravel( TileMap map )
    {
      //Fill everything with gravel
      for ( int i = 1; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 1; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            map.tiles[ i, j ] = Tile.Get_new_tile( TILE_TYPE.GRAVEL, i, j );
          }
        }
      }
    }


    public void Straiten( TileMap map )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          map.tiles[ i, j ].in_main_region = false;
        }
      }

      // 0 = Empty   ->  Empty
      // 1 = of type ->  Empty
      // 2 = Empty   ->  to type
      // 3 = of type ->  of type

      List<int[,]> fig_list = new List<int[,]>
      {
        new int[,]
        {
          { 1,1,3 },
          { 1,0,2 },
          { 1,1,3 }
        },
        new int[,]
        {
          { 1,1,1 },
          { 1,0,1 },
          { 3,2,3 }
        },
        new int[,]
        {
          { 3,2,3 },
          { 1,0,1 },
          { 1,1,1 }
        },
        new int[,]
        {
          { 3,1,1 },
          { 2,0,1 },
          { 3,1,1 }
        },
        new int[,]
        {
          { 1,1,1,1,1 },
          { 1,0,0,0,1 },
          { 3,2,2,2,3 }
        },
        new int[,]
        {
          { 3,2,2,2,3 },
          { 1,0,0,0,1 },
          { 1,1,1,1,1 },
        },
        new int[,]
        {
          { 3,1,1 },
          { 2,0,1 },
          { 2,0,1 },
          { 2,0,1 },
          { 3,1,1 },
        },
        new int[,]
        {
          { 1,1,3 },
          { 1,0,2 },
          { 1,0,2 },
          { 1,0,2 },
          { 1,1,3 },
        }
      };

      int did_something = 0;

      do
      {
        did_something = 0;
        for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
        {
          for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
          {
            foreach ( var fig in fig_list )
            {
              if ( map.Check_n_Replace( map.tiles[ i, j ], fig, TILE_TYPE.GRASS ) )
                did_something++;
            }
          }
        }
      } while ( did_something > 2 );
    }

    public void Defill( TileMap map  )
    {
      Tile t;
      while ( (t = this.Get_next_3_empty( map ) )!= null)
      {
        t.Change_biom( TILE_TYPE.NONE );
      }
    }

    private Tile Get_next_3_empty( TileMap map )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.GRASS )
          {
            int empty_counter = 0;

            if ( map.Is_down_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_left_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_right_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_up_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }

            if ( empty_counter == 3 )
            {              
              return map.tiles[ i, j ];
            }
          }
        }
      }
      return null;
    }

    private Tile Has_2_of_type( TileMap map, TILE_TYPE tt )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == tt )
          {
            int empty_counter = 0;

            if ( map.Is_down_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_left_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_right_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( map.Is_up_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }

            if ( empty_counter == 3 )
            {
              return map.tiles[ i, j ];
            }
          }
        }
      }
      return null;
    }

    public void Init_flood_fill( TileMap map  )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            Floodfill( map, map.tiles[ i, j ] );
          }
        }
      }
    }

    public void Floodfill( TileMap map, Tile tile )
    {
      Stack<Tile> tiles = new Stack<Tile>( );

      Stack<Tile> backtrack = new Stack<Tile>( );

      // 1
      if ( tile.type != TILE_TYPE.NONE || !Tile_is_valid( map,tile, Orientation.NONE ) )
      {
        return;
      }

      tiles.Push( tile );

      while ( tiles.Count > 0 )
      {
        Tile popped = tiles.Pop( );

        if ( popped.type == TILE_TYPE.NONE )
        {
          int x = (int)popped.X_Position;
          int y = (int)popped.Y_Position;

          map.tiles[ x, y ].Change_biom( TILE_TYPE.GRASS );

          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }


          int mitte_x = map.tiles.GetLength( 0 ) / 2;
          int mitte_y = map.tiles.GetLength( 1 ) / 2;

          if ( x <= mitte_x && y <= mitte_y )
          {
            // Links oben -> nach rechts unten
          }

          switch ( Generator.rand.Next( 3 ) )
          {
            case 0:
              if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              break;

            case 1:
              if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              break;

            case 2:
              if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              break;

            case 3:
              if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              break;
          }

        }
      }

      while ( backtrack.Count > 0 )
      {
        Tile popped = backtrack.Pop( );

        int x = (int)popped.X_Position;
        int y = (int)popped.Y_Position;

        if ( popped.type == TILE_TYPE.NONE )
        {
          map.tiles[ x, y ].Change_biom( TILE_TYPE.GRASS );
          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }

        }
        switch ( Generator.rand.Next( 3 ) )
        {
          case 0:
            if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            break;

          case 1:
            if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            break;

          case 2:
            if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            break;

          case 3:
            if ( map.Is_down_free( popped ) && Tile_is_valid( map,map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( map.Is_left_free( popped ) && Tile_is_valid( map,map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( map.Is_right_free( popped ) && Tile_is_valid( map,map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( map.Is_up_free( popped ) && Tile_is_valid( map,map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            break;
        }


      }
    }


    public bool Tile_is_valid( TileMap map, Tile tile, Orientation or )
    {

      if ( tile.X_Position % 2 == 1 && tile.Y_Position % 2 == 1 )
        return false;

      if ( or != Orientation.LEFT && !map.Is_left_free( tile ) )
        return false;

      if ( or != Orientation.RIGHT && !map.Is_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && !map.Is_up_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && !map.Is_down_free( tile ) )
        return false;



      if ( or != Orientation.DOWN && or != Orientation.LEFT && !map.Is_down_left_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && or != Orientation.RIGHT && !map.Is_down_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.LEFT && !map.Is_up_left_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.RIGHT && !map.Is_up_right_free( tile ) )
        return false;

      return true;
    }

  }
}
