using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saveable
{
  public interface ISaveable
  {
    void Save();

    void Load();
  }
}
