using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace UniversalCompressor.Logica
{
    public struct TuplaLZ77
    {
        
        public int desplazamiento; // Distancia hacia atrás donde empieza la coincidencia       
        public int longitud; // Cantidad de bytes que coinciden
        public byte siguiente; // Siguiente byte que aparece después de la coincidencia (0 si es fin de archivo)

        public TuplaLZ77(int desplazamiento, int longitud, byte siguiente)
        {
            this.desplazamiento = desplazamiento;
            this.longitud = longitud;
            this.siguiente = siguiente;
        }
    }

    public static class LZ77
    {
        private const int tamanoVentana = 4096; // 4KB

        public static List<TuplaLZ77> Comprimir(byte[] datos)
        {
            List<TuplaLZ77> resultado = new List<TuplaLZ77>();
            int posicion = 0;

            while (posicion < datos.Length)
            {
                int mejorDesplazamiento = 0;
                int mejorLongitud = 0;

                //ventana de búsqueda (Search Buffer)
                int inicioVentana = Math.Max(0, posicion - tamanoVentana);

                // Bucle para buscar la coincidencia más larga
                for (int i = inicioVentana; i < posicion; i++)
                {
                    int longitudActual = 0;

                    // Compara bytes hasta que termine el archivo, la longitud sea máxima, o termine la coincidencia
                    while (posicion + longitudActual < datos.Length &&
                           datos[i + longitudActual] == datos[posicion + longitudActual])
                    {
                        longitudActual++;
                    }

                    // Actualizar la mejor coincidencia
                    if (longitudActual > mejorLongitud)
                    {
                        mejorLongitud = longitudActual;
                        mejorDesplazamiento = posicion - i;
                    }
                }

                // Crea la Tupla

                byte siguienteByte = (posicion + mejorLongitud < datos.Length)
                                    ? datos[posicion + mejorLongitud]
                                    : (byte)0; // Usar 0 para EOF o un byte no utilizado si se requiere

                if (mejorLongitud == 0)
                {
                    mejorLongitud = 0;
                    mejorDesplazamiento = 0;
                    siguienteByte = datos[posicion];
                }

                resultado.Add(new TuplaLZ77(mejorDesplazamiento, mejorLongitud, siguienteByte));

                //avanza la posición en el buffer de entrada
                posicion += mejorLongitud + 1;
            }

            return resultado;
        }

        public static byte[] Descomprimir(List<TuplaLZ77> tuplas)
        {
            List<byte> salida = new List<byte>();

            foreach (var t in tuplas)
            {
                // Si hay una coincidencia (desplazamiento > 0)
                if (t.desplazamiento > 0)
                {
                    // Calcular el índice de inicio para copiar desde la salida ya generada
                    int inicio = salida.Count - t.desplazamiento;

                    // Copiar los bytes indicados por la longitud
                    for (int i = 0; i < t.longitud; i++)
                    {
                        salida.Add(salida[inicio + i]);
                    }
                }
                // Agregar el siguiente byte literal si existe
                if (t.siguiente != (byte)0)
                    salida.Add(t.siguiente);
            }

            return salida.ToArray();
        }

        // Transforma la lista de tuplas a un arreglo de bytes
        public static byte[] TuplasABytes(List<TuplaLZ77> lista)
        {
            List<byte> buffer = new List<byte>();

            foreach (var t in lista)
            {
                // Estructura de la tupla: Int (4 bytes) + Int (4 bytes) + Byte (1 byte) = 9 bytes por tupla
                byte[] bDesplazamiento = BitConverter.GetBytes(t.desplazamiento);
                byte[] bLongitud = BitConverter.GetBytes(t.longitud);

                buffer.AddRange(bDesplazamiento);
                buffer.AddRange(bLongitud);
                buffer.Add(t.siguiente);
            }
            return buffer.ToArray();
        }

        // Reconstruye la lista de tuplas a partir de los datos leídos
        public static List<TuplaLZ77> BytesATuplas(byte[] data)
        {
            List<TuplaLZ77> lista = new List<TuplaLZ77>();

            // Se recorre el array en bloques de 9 bytes
            int pos = 0;
            const int tuplaSize = 9;

            while (pos < data.Length)
            {
                // 4 bytes: Desplazamiento
                int desplazamiento = BitConverter.ToInt32(data, pos);

                // 4 bytes: Longitud
                int longitud = BitConverter.ToInt32(data, pos + 4);

                // 1 byte: Siguiente
                byte siguiente = data[pos + 8];

                lista.Add(new TuplaLZ77(desplazamiento, longitud, siguiente));
                pos += tuplaSize;
            }
            return lista;
        }
    }
}