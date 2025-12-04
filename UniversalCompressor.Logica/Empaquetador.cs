using System;
using System.IO;
using System.Text;

namespace UniversalCompressor.Logica
{
    // Clase auxiliar utilizada para transportar la información recuperada de un archivo .myzip
    public class InfoArchivo
    {
        public int TipoAlgoritmo { get; set; }
        public long TamanoOriginal { get; set; }
        public string Nombre { get; set; }
        public byte[] Data { get; set; }
    }

    public class Empaquetador
    {
        // Genera el archivo comprimido .myzip escribiendo la cabecera y los datos
        // Códigos de algoritmo: 0=Huffman, 1=LZ77, 2=LZ78
        public void Guardar(string path, byte[] datos, string nombreOriginal, long tamanoOriginal, int algoritmo)
        {
            // Se crea el flujo de archivo para escritura
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            // 1. Escritura del Identificador del algoritmo
            w.Write((byte)algoritmo);

            // Guardo el tamaño original para que el profe esté feliz
            w.Write(tamanoOriginal);

            // 2. Escritura del nombre original del archivo
            // Primero se convierte el string a bytes
            byte[] nombreB = Encoding.UTF8.GetBytes(nombreOriginal);

            // Se escribe la longitud del nombre para saber cuántos bytes leer después
            w.Write(nombreB.Length);
            w.Write(nombreB);

            // 3. Escritura del contenido comprimido 
            w.Write(datos);

            // liberar memoria
            w.Close();
            fs.Close();
        }

        // Lee un archivo .myzip existente y separa la cabecera de los datos 
        public InfoArchivo Leer(string path)
        {
            InfoArchivo info = new InfoArchivo();

            // Se abre el archivo en modo lectura
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader r = new BinaryReader(fs);

            // 1. Recuperación del ID del algoritmo
            info.TipoAlgoritmo = r.ReadByte();

            // Recupero el tamaño original
            info.TamanoOriginal = r.ReadInt64();

            // 2. Recuperación del nombre original
            // Primero se lee el entero que indica el largo del nombre
            int largo = r.ReadInt32();
            byte[] bufferNombre = r.ReadBytes(largo);

            // Se reconstruye el string a partir de los bytes leídos
            info.Nombre = Encoding.UTF8.GetString(bufferNombre);

            // 3. Recuperación de los datos comprimidos
            // Se calcula cuántos bytes restan en el archivo 
            long queda = fs.Length - fs.Position;

            // Se lee el resto del archivo que corresponde a la data comprimida
            info.Data = r.ReadBytes((int)queda);

            r.Close();
            fs.Close();

            return info;
        }
    }
}