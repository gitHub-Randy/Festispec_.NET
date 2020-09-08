using FestiSpec.Entities.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Offline
{
    class Serializer
    {
        public static void Serialize(FestiSpecContext context)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("sync.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, context);
            stream.Close();
        }

        public FestiSpecContext Deserialize()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("sync.dat", FileMode.Open, FileAccess.Read);
            FestiSpecContext newcontext = (FestiSpecContext)formatter.Deserialize(stream);
            return newcontext;
        }
    }
}
