using System;
using System.Diagnostics;

namespace UniversalCompressor.Logica
{
    // Clase para guardar los datos
    public class MisEstadisticas
    {
        public long Tiempo { get; set; }
        public long Memoria { get; set; }
        public double Porcentaje { get; set; }

        // Variables auxiliares privadas
        private Stopwatch _reloj;
        private long _memoriaInicial;

        public MisEstadisticas()
        {
            _reloj = new Stopwatch();
        }

        // Llamar a esto antes de empezar el algoritmo
        public void Empezar()
        {
            // Limpiamos la basura primero para que sea más exacto
            GC.Collect();
            GC.WaitForPendingFinalizers();

            _memoriaInicial = GC.GetTotalMemory(true);
            _reloj.Restart();
        }

        // Llamar a esto justo después de terminar
        public void Terminar(long original, long comprimido)
        {
            _reloj.Stop();
            long memoriaFinal = GC.GetTotalMemory(false);

            Tiempo = _reloj.ElapsedMilliseconds;

            // Calculo la diferencia de memoria
            Memoria = memoriaFinal - _memoriaInicial;
            if (Memoria < 0) Memoria = 0; // Por si acaso da negativo

            // Calculo de la tasa
            if (original > 0)
            {
                double division = (double)comprimido / original;
                Porcentaje = (1.0 - division) * 100;
            }
            else
            {
                Porcentaje = 0;
            }
        }

        // Método para imprimir rápido
        public string ObtenerResumen()
        {
            return "Tiempo: " + Tiempo + "ms | Memoria: " + Memoria + " bytes | Tasa: " + Math.Round(Porcentaje, 2) + "%";
        }
    }
}