using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
/// ===============================
///  AUTHOR 
///  Mykyta Shvets
/// ===============================

namespace Car_Race
{

    class FileWriter<T>
    {

        public void Write(string fileName, ObservableCollection<T> list)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            List<T> data = new List<T>(list);
            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, data);
            }
        }
    }
}
