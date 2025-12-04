using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalCompressor.Logica
{
    public class LZ78
    {
        // Realiza la compresión del texto utilizando el algoritmo LZ78 
        public List<(int, char)> Comprimir(string texto)
        {
            // Diccionario para almacenar los patrones y sus índices 
            Dictionary<string, int> dict = new Dictionary<string, int>();
            List<(int, char)> resultado = new List<(int, char)>();

            string actual = "";
            int contador = 1; // Los índices en LZ78 inician en 1 porque 0 es nulo/vacío

            for (int i = 0; i < texto.Length; i++)
            {
                char letra = texto[i];
                string combinado = actual + letra;

                if (dict.ContainsKey(combinado))
                {
                    // Si la combinación ya existe, se continúa acumulando caracteres
                    actual = combinado;
                }
                else
                {
                    // Es una secuencia nueva:
                    // Se recupera el índice del prefijo anterior
                    int idx = 0;
                    if (actual != "")
                    {
                        idx = dict[actual];
                    }

                    // Se añade la tupla (índice del prefijo, nuevo carácter) a la lista de salida
                    resultado.Add((idx, letra));

                    // Se registra la nueva combinación
                    dict.Add(combinado, contador);
                    contador++;

                    // Se reinicia el buffer de lectura
                    actual = "";
                }
            }
            // Manejo del caso donde queda una secuencia pendiente al final del texto
            if (actual.Length > 0)
            {
                char lastChar = actual[actual.Length - 1];
                string resto = actual.Substring(0, actual.Length - 1);

                int idx = 0;
                if (resto != "")
                {
                    idx = dict[resto];
                }
                resultado.Add((idx, lastChar));
            }

            return resultado;
        }

        // Realiza la descompresión de la lista de tuplas generada por LZ78
        public string Descomprimir(List<(int, char)> lista)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            StringBuilder sb = new StringBuilder();
            int contador = 1;

            foreach (var item in lista)
            {
                int idx = item.Item1;
                char letra = item.Item2;

                string palabra = "";

                // Si el índice no es 0, se recupera la cadena base del diccionario
                if (idx != 0)
                {
                    palabra = dict[idx];
                }

                // Se forma la nueva secuencia uniendo el prefijo recuperado y el carácter actual
                string nuevo = palabra + letra;

                // Se une al constructor del texto final
                sb.Append(nuevo);

                // Se actualiza el diccionario 
                dict.Add(contador, nuevo);
                contador++;
            }

            return sb.ToString();
        }



        // Transforma la lista de tuplas a un arreglo de bytes
        public byte[] TuplasABytes(List<(int, char)> lista)
        {
            List<byte> buffer = new List<byte>();

            foreach (var t in lista)
            {
                // Conversión int y char a arrays de bytes
                byte[] bInt = BitConverter.GetBytes(t.Item1);
                byte[] bChar = BitConverter.GetBytes(t.Item2);

                buffer.AddRange(bInt);
                buffer.AddRange(bChar);
            }
            return buffer.ToArray();
        }

        // Reconstruye la lista de tuplas a partir de los datos leídos
        public List<(int, char)> BytesATuplas(byte[] data)
        {
            List<(int, char)> lista = new List<(int, char)>();

            // Se recorre el array en bloques de 6 bytes (4 bytes para int + 2 bytes para char)
            int pos = 0;
            while (pos < data.Length)
            {
                int num = BitConverter.ToInt32(data, pos);
                char letra = BitConverter.ToChar(data, pos + 4);

                lista.Add((num, letra));
                pos += 6;
            }
            return lista;
        }
    }
}