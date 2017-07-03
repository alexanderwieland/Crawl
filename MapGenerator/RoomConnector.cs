using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  class RoomConnector
  {

    public RoomConnector( )
    {

    }

    public void Connect( TileMap map )
    {
      this.Set_connections( map );
      this.Generate_spanning_tree( map );
    }

    private void Generate_spanning_tree( TileMap map )
    {
      // Search for main room
      do
      {

        Tile main_room = null;
        while ( ( main_room = map.Search_for_tile_without_main_region_from_pos( TILE_TYPE.SAND, main_room ) ) != null )
        {
          main_room.in_main_region = true;

          if (
               map.Is_left_tile_of_type( main_room, TILE_TYPE.GRASS ) && map.Is_right_tile_of_type( main_room, TILE_TYPE.DIRT ) ||
               map.Is_right_tile_of_type( main_room, TILE_TYPE.GRASS ) && map.Is_left_tile_of_type( main_room, TILE_TYPE.DIRT )
             )
          {
            Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ main_room.X_Position - 1, main_room.Y_Position ] );
            Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ main_room.X_Position + 1, main_room.Y_Position ] );
            this.Remove_all_unnessesary_connections( map );
          }
          else if (
                     map.Is_up_tile_of_type( main_room, TILE_TYPE.GRASS ) && map.Is_down_tile_of_type( main_room, TILE_TYPE.DIRT ) ||
                     map.Is_down_tile_of_type( main_room, TILE_TYPE.GRASS ) && map.Is_up_tile_of_type( main_room, TILE_TYPE.DIRT )
                   )
          {

            Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ main_room.X_Position, main_room.Y_Position - 1 ] );
            Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ main_room.X_Position, main_room.Y_Position + 1 ] );
            this.Remove_all_unnessesary_connections( map );
          }

        }

      } while ( map.Search_for_tile_without_main_region( TILE_TYPE.SAND ) != null );
      this.Remove_all_unnessesary_connections2( map );

    }

    private void Remove_all_unnessesary_connections2( TileMap map )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {

          if (
               map.tiles[ i, j ].type == TILE_TYPE.SAND &&
               map.Is_left_tile_in_main_region( map.tiles[ i, j ] ) &&
               map.Is_right_tile_in_main_region( map.tiles[ i, j ] ) &&
               map.Is_left_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) &&
               map.Is_right_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT )
             )
          {
            if ( Generator.rand.Next( 100 ) < 5 )
            {
              map.tiles[ i, j ].in_main_region = true;
              continue;
            }
            map.tiles[ i, j ].Change_biom( TILE_TYPE.NONE );
          }
          if (
              map.tiles[ i, j ].type == TILE_TYPE.SAND &&
              map.Is_up_tile_in_main_region( map.tiles[ i, j ] ) &&
              map.Is_down_tile_in_main_region( map.tiles[ i, j ] ) &&
              map.Is_up_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) &&
              map.Is_down_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT )
            )
          {

            if ( Generator.rand.Next( 100 ) < 5 )
            {
              map.tiles[ i, j ].in_main_region = true;
              continue;
            }
            map.tiles[ i, j ].Change_biom( TILE_TYPE.NONE );
          }

        }
      }
    }

    private void Remove_all_unnessesary_connections( TileMap map )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {

          if ( map.tiles[ i, j ].type == TILE_TYPE.SAND && map.tiles[ i, j ].in_main_region == false )
          {
            if (
              ( map.Is_left_tile_in_main_region( map.tiles[ i, j ] ) && map.Is_right_tile_in_main_region( map.tiles[ i, j ] ) ) ||
              ( map.Is_up_tile_in_main_region( map.tiles[ i, j ] ) && map.Is_down_tile_in_main_region( map.tiles[ i, j ] ) )
            )
            {
              if ( ( map.Is_up_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) && map.Is_down_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) ) ||
                    ( map.Is_left_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) && map.Is_right_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) )
                )
              {
                continue;
              }
              if ( Generator.rand.Next( 100 ) < 3 )
              {
                map.tiles[ i, j ].in_main_region = true;
                continue;
              }
              map.tiles[ i, j ].Change_biom( TILE_TYPE.NONE );
            }
          }

        }
      }
    }

    public void Add_all_ajectiled_tiles_of_type_to_main_region( TileMap map, Tile tile )
    {
      tile.in_main_region = true;
      if ( map.Is_left_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position - 1, tile.Y_Position ].in_main_region )
      {
        Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ tile.X_Position - 1, tile.Y_Position ] );
      }
      if ( map.Is_right_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position + 1, tile.Y_Position ].in_main_region )
      {
        Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ tile.X_Position + 1, tile.Y_Position ] );
      }
      if ( map.Is_up_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position, tile.Y_Position - 1 ].in_main_region )
      {
        Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ tile.X_Position, tile.Y_Position - 1 ] );
      }
      if ( map.Is_down_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position, tile.Y_Position + 1 ].in_main_region )
      {
        Add_all_ajectiled_tiles_of_type_to_main_region( map, map.tiles[ tile.X_Position, tile.Y_Position + 1 ] );
      }
    }


    public void Set_connections( TileMap map )
    {
      for ( int i = 1; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 1; j < map.tiles.GetLength( 1 ); j++ )
        {
          //if ( map.map[ i, j ].type == TILE_TYPE.NONE && map.map[ i, j ].X_Position % 2 == 1 && map.map[ i, j ].Y_Position % 2 == 1  )
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            // UP DOWN
            if ( map.tiles[ i, j ].Y_Position + 1 < map.tiles.GetLength( 1 ) && ( map.tiles[ i, j + 1 ].type == TILE_TYPE.GRASS || map.tiles[ i, j + 1 ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].Y_Position - 1 < map.tiles.GetLength( 1 ) && map.tiles[ i, j - 1 ].type == TILE_TYPE.DIRT
              )
            {
              map.tiles[ i, j ].Change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].Y_Position - 1 < map.tiles.GetLength( 1 ) && ( map.tiles[ i, j - 1 ].type == TILE_TYPE.GRASS || map.tiles[ i, j - 1 ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].Y_Position + 1 < map.tiles.GetLength( 1 ) && map.tiles[ i, j + 1 ].type == TILE_TYPE.DIRT
              )
            {
              map.tiles[ i, j ].Change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].X_Position + 1 < map.tiles.GetLength( 0 ) && ( map.tiles[ i + 1, j ].type == TILE_TYPE.GRASS || map.tiles[ i + 1, j ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].X_Position - 1 < map.tiles.GetLength( 0 ) && map.tiles[ i - 1, j ].type == TILE_TYPE.DIRT
                 )
            {
              map.tiles[ i, j ].Change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].X_Position - 1 < map.tiles.GetLength( 0 ) && ( map.tiles[ i - 1, j ].type == TILE_TYPE.GRASS || map.tiles[ i - 1, j ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ i + 1, j ].type == TILE_TYPE.DIRT
                 )
            {
              map.tiles[ i, j ].Change_biom( TILE_TYPE.SAND );
            }
          }
        }
      }
    }
  }
}
