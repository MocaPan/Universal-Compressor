using System;
using System.Collections.Generic;
using System.IO;
using UniversalCompressor.Logica; 
class Program
{
    static void Main(string[] args)
    {
        LZ78 lz = new LZ78();
        Empaquetador pack = new Empaquetador();
        MisEstadisticas stats = new MisEstadisticas(); // Instancio mis estadisticas

        string textoOriginal = "";
        for (int i = 0; i < 200; i++) textoOriginal += "HOLA_ESTO_ES_UNA_PRUEBA_REPETIDA_";

        Console.WriteLine("Texto tamaño: " + textoOriginal.Length);

        // --- COMPRIMIR ---
        Console.WriteLine("Comenzando...");

        // 1. Empiezo a medir
        stats.Empezar();

        // 2. Hago la compresión
        var lista = lz.Comprimir(textoOriginal);
        byte[] bytes = lz.TuplasABytes(lista);

        // Guardo el archivo
        pack.Guardar("prueba.myzip", bytes, "test.txt", 2);

        // 3. Termino de medir
        stats.Terminar(textoOriginal.Length, bytes.Length);

        Console.WriteLine(stats.ObtenerResumen());

        Console.ReadLine();
    }
}