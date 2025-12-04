using System;
using System.IO;
using System.Text;

namespace UniversalCompressor.Logica
{
    // Clase para devolver lo que leo del archivo
    public class InfoArchivo
    {
        public int TipoAlgoritmo { get; set; }
        public string Nombre { get; set; }
        public byte[] Data { get; set; }
    }

    public class Empaquetador
    {
        // Guardar en formato .myzip
        // algoritmo: 0=Huffman, 1=LZ77, 2=LZ78
        public void Guardar(string path, byte[] datos, string nombreOriginal, int algoritmo)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            // 1. Guardo ID del algoritmo
            w.Write((byte)algoritmo);

            // 2. Guardo nombre del archivo original
            byte[] nombreB = Encoding.UTF8.GetBytes(nombreOriginal);
            w.Write(nombreB.Length); // Largo del nombre
            w.Write(nombreB);        // Nombre

            // 3. Guardo el contenido comprimido
            w.Write(datos);

            w.Close();
            fs.Close();
        }

        public InfoArchivo Leer(string path)
        {
            InfoArchivo info = new InfoArchivo();

            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader r = new BinaryReader(fs);

            // 1. Leer ID
            info.TipoAlgoritmo = r.ReadByte();

            // 2. Leer Nombre
            int largo = r.ReadInt32();
            byte[] bufferNombre = r.ReadBytes(largo);
            info.Nombre = Encoding.UTF8.GetString(bufferNombre);

            // 3. Leer Datos
            // Resto lo que ya lei para saber cuanto queda
            long queda = fs.Length - fs.Position;
            info.Data = r.ReadBytes((int)queda);

            r.Close();
            fs.Close();

            return info;
        }
    }
}