using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalCompressor.Logica
{
    public class LZ78
    {
        public List<(int, char)> Comprimir(string texto)
        {
            // Diccionario: string -> int
            Dictionary<string, int> dict = new Dictionary<string, int>();
            List<(int, char)> resultado = new List<(int, char)>();

            string actual = "";
            int contador = 1; // Los indices empiezan en 1

            for (int i = 0; i < texto.Length; i++)
            {
                char letra = texto[i];
                string combinado = actual + letra;

                if (dict.ContainsKey(combinado))
                {
                    // Si ya está, sigo leyendo
                    actual = combinado;
                }
                else
                {
                    // Busco el indice del anterior
                    int idx = 0;
                    if (actual != "")
                    {
                        idx = dict[actual];
                    }

                    // Guardo la tupla (indice, letra)
                    resultado.Add((idx, letra));

                    // Agrego al diccionario
                    dict.Add(combinado, contador);
                    contador++;

                    // Reinicio
                    actual = "";
                }
            }

            // Si sobró algo al final
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

                // Si el indice no es 0, busco qué texto era
                if (idx != 0)
                {
                    palabra = dict[idx];
                }

                string nuevo = palabra + letra;

                // Añado al texto final
                sb.Append(nuevo);

                // Guardo en el diccionario para despues
                dict.Add(contador, nuevo);
                contador++;
            }

            return sb.ToString();
        }

        // Métodos para convertir a bytes (para el archivo .myzip)
        public byte[] TuplasABytes(List<(int, char)> lista)
        {
            List<byte> buffer = new List<byte>();

            foreach (var t in lista)
            {
                // Convierto el int y el char a bytes
                byte[] bInt = BitConverter.GetBytes(t.Item1);
                byte[] bChar = BitConverter.GetBytes(t.Item2);

                buffer.AddRange(bInt);
                buffer.AddRange(bChar);
            }
            return buffer.ToArray();
        }

        public List<(int, char)> BytesATuplas(byte[] data)
        {
            List<(int, char)> lista = new List<(int, char)>();

            // Recorro de 6 en 6 (4 bytes del int + 2 del char)
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