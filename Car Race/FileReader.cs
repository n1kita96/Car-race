using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
/// ===============================
///  AUTHOR 
///  Mykyta Shvets
/// ===============================
namespace Car_Race
{
    class FileReader<T>
    {
        public List<T> readToList(string fileName)
        {
            List<T> list;
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<T>>(json);
            }
            return list;
        }
    }
}
