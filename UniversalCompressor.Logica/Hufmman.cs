using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace UniversalCompressor.Logica
{
    public static class Huffman
    {
        public class NodoHuffman
        {
            public byte? simbolo;
            public int frecuencia;
            public NodoHuffman left;
            public NodoHuffman right;
            public bool leaf => left == null && right == null;
        }

        public class ComparadorNodos : IComparer<NodoHuffman>
        {
            public int Compare(NodoHuffman a, NodoHuffman b) => a.frecuencia.CompareTo(b.frecuencia);
        }

        private class BitWriter
        {
            private List<byte> buffer = new List<byte>();
            private byte currentByte = 0;
            private int bitCount = 0;

            public void WriteBit(char bit)
            {
                currentByte <<= 1;
                if (bit == '1')
                {
                    currentByte |= 1;
                }
                bitCount++;

                // Cuando el byte está completo, se añade al buffer
                if (bitCount == 8)
                {
                    buffer.Add(currentByte);
                    currentByte = 0;
                    bitCount = 0;
                }
            }

            public (byte[] bytes, int padding) Finalizar()
            {
                int padding = 0;
                if (bitCount > 0)
                {
                    // Se calcula el padding y se rellena el último byte
                    padding = 8 - bitCount;
                    currentByte <<= padding;
                    buffer.Add(currentByte);
                }
                return (buffer.ToArray(), padding);
            }
        }

        private const byte Hoja = 0;
        private const byte Interno = 1;

        // Serializa el árbol a una secuencia de bytes (recorrido pre-orden)
        public static byte[] ArbolABytes(NodoHuffman raiz)
        {
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                RecorrerYSerializar(raiz, w);
                return ms.ToArray();
            }
        }

        private static void RecorrerYSerializar(NodoHuffman nodo, BinaryWriter w)
        {
            if (nodo == null) return;

            if (nodo.leaf)
            {
                // Se marca como Hoja y se escribe el byte del símbolo
                w.Write(Hoja);
                w.Write(nodo.simbolo.Value);
            }
            else
            {
                // Se marca como Interno y se serializan los hijos
                w.Write(Interno);
                RecorrerYSerializar(nodo.left, w);
                RecorrerYSerializar(nodo.right, w);
            }
        }

        public static NodoHuffman BytesAArbol(BinaryReader r)
        {
            // Se lee el tipo de nodo (Hoja o Interno)
            byte tipo = r.ReadByte();

            if (tipo == Hoja)
            {
                // Se lee el byte del símbolo
                return new NodoHuffman { simbolo = r.ReadByte() };
            }
            else // tipo == Interno
            {
                // Se reconstruyen los subárboles izquierdo y derecho
                NodoHuffman left = BytesAArbol(r);
                NodoHuffman right = BytesAArbol(r);

                return new NodoHuffman { left = left, right = right };
            }
        }

        private static Dictionary<byte, int> ObtenerFrecuencias(byte[] datos)
        {
            var frec = new Dictionary<byte, int>();
            foreach (var b in datos)
            {
                if (!frec.ContainsKey(b)) frec[b] = 0;
                frec[b]++;
            }
            return frec;
        }

        private static NodoHuffman ConstruirArbol(byte[] datos)
        {
            var frecs = ObtenerFrecuencias(datos);
            // Se usa List para simular cola de prioridad, ordenando manualmente
            var cola = new List<NodoHuffman>();

            // Se crean nodos hoja para cada símbolo único
            foreach (var par in frecs)
            {
                var nodo = new NodoHuffman { simbolo = par.Key, frecuencia = par.Value };
                cola.Add(nodo);
            }

            // Manejo de caso extremo: archivo de un solo símbolo
            if (cola.Count == 1)
            {
                var raiz = cola[0];
                return new NodoHuffman { left = raiz, right = null, frecuencia = raiz.frecuencia };
            }

            // Se combinan los nodos de menor frecuencia hasta que solo quede la raíz
            while (cola.Count > 1)
            {
                // Ordenar la lista por frecuencia
                cola.Sort(new ComparadorNodos());

                // Extraer los dos primeros (menor frecuencia)
                var izq = cola[0];
                cola.RemoveAt(0);

                var der = cola[0];
                cola.RemoveAt(0);

                var nuevo = new NodoHuffman
                {
                    simbolo = null,
                    frecuencia = izq.frecuencia + der.frecuencia,
                    left = izq,
                    right = der
                };
                cola.Add(nuevo);
            }

            return cola[0]; // La raíz
        }

        private static void GenerarCodigos(NodoHuffman nodo, string codigo, Dictionary<byte, string> tabla)
        {
            if (nodo == null) return;

            if (nodo.leaf)
            {
                // Se almacena el código al llegar a una hoja
                tabla[nodo.simbolo.Value] = codigo == "" ? "0" : codigo;
                return;
            }

            // Izquierda es '0', Derecha es '1'
            GenerarCodigos(nodo.left, codigo + "0", tabla);
            GenerarCodigos(nodo.right, codigo + "1", tabla);
        }

        // Genera la tabla final de códigos (símbolo a código binario)
        public static Dictionary<byte, string> ObtenerTablaCodigos(NodoHuffman raiz)
        {
            var tabla = new Dictionary<byte, string>();
            GenerarCodigos(raiz, "", tabla);
            return tabla;
        }

        // Comprime los datos, adjuntando el árbol y el padding en el byte[] de salida
        public static byte[] Comprimir(byte[] datos)
        {
            var arbol = ConstruirArbol(datos);
            var tabla = ObtenerTablaCodigos(arbol);

            var bitWriter = new BitWriter();

            // 1. Codificar los datos, escribiendo bits en el BitWriter
            foreach (byte b in datos)
            {
                string codigo = tabla[b];
                foreach (char bit in codigo)
                {
                    bitWriter.WriteBit(bit);
                }
            }

            // 2. Finalizar el empaquetado y obtener el padding
            var (datosComprimidos, padding) = bitWriter.Finalizar();

            // 3. Serializar el árbol
            byte[] arbolBytes = ArbolABytes(arbol);

            // 4. Concatenar
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                w.Write(arbolBytes.Length);
                w.Write((byte)padding);
                w.Write(arbolBytes);
                w.Write(datosComprimidos);

                return ms.ToArray();
            }
        }

        public static byte[] Descomprimir(byte[] datosComprimidosConCabecera)
        {
            using (var ms = new MemoryStream(datosComprimidosConCabecera))
            using (var r = new BinaryReader(ms))
            {
                // Tamaño del Árbol y Padding
                int largoArbol = r.ReadInt32();
                int padding = r.ReadByte();

                //Reconstrucción del árbol
                NodoHuffman arbol = BytesAArbol(r);

                //Lectura de los datos comprimidos restantes
                long bytesData = ms.Length - ms.Position;
                byte[] data = r.ReadBytes((int)bytesData);

                // Decodificación bit a bit
                var resultado = new List<byte>();
                var nodoActual = arbol;

                foreach (byte b in data)
                {
                    for (int i = 7; i >= 0; i--)
                    {
                        // Se comprueba si el bit actual es 1 o 0
                        bool isOne = (b & (1 << i)) != 0;

                        // Se ignora el padding en el último byte
                        if (r.BaseStream.Position == ms.Length && i < padding)
                            break;

                        // Se recorre el árbol: 0=izquierda, 1=derecha
                        nodoActual = isOne ? nodoActual.right : nodoActual.left;

                        if (nodoActual.leaf)
                        {
                            // Se añade el símbolo al resultado y se vuelve a la raíz
                            resultado.Add(nodoActual.simbolo.Value);
                            nodoActual = arbol;
                        }
                    }
                }

                return resultado.ToArray();
            }
        }
    }
}