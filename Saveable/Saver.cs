using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saveable
{
  class Saver
  {
    static void Save( List<ISaveable> list_to_save, string savedir )
    {
      if ( !Directory.Exists(savedir ) )
      {
        Directory.CreateDirectory(savedir );
      }

      foreach ( ISaveable sv in list_to_save )
      {
        sv.Save( );
      }      
    }
  }
}
