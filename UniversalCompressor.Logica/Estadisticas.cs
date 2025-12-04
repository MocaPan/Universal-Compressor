using System;
using System.Diagnostics;

namespace UniversalCompressor.Logica
{
    // Clase encargada de capturar y calcular las métricas de rendimiento
    public class MisEstadisticas
    {
        public long Tiempo { get; set; }
        public long Memoria { get; set; }
        public double Porcentaje { get; set; }

        // Variables privadas para el estado interno de la medición
        private Stopwatch _reloj;
        private long _memoriaInicial;

        public MisEstadisticas()
        {
            _reloj = new Stopwatch();
        }

        // Inicializa el proceso de medición limpiando el entorno previo
        public void Empezar()
        {
            // Se fuerza la recolección de basura para asegurar que la medición de memoria inicie desde una base limpia y no haya algo que interfiera
           
            GC.Collect();
            GC.WaitForPendingFinalizers();

            _memoriaInicial = GC.GetTotalMemory(true);
            _reloj.Restart();
        }

        // Finaliza la medición y realiza los cálculos de las estadísticas
        public void Terminar(long original, long comprimido)
        {
            _reloj.Stop();
            long memoriaFinal = GC.GetTotalMemory(false);

            Tiempo = _reloj.ElapsedMilliseconds;

            // Se calcula la diferencia de memoria consumida durante el proceso
            Memoria = memoriaFinal - _memoriaInicial;

            // Validación para evitar valores negativos 
            if (Memoria < 0) Memoria = 0;

            // Cálculo de la tasa de compresión 
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

        // Genera una cadena de texto formateada con el resumen de los resultados
        public string ObtenerResumen()
        {
            return "Tiempo: " + Tiempo + "ms | Memoria: " + Memoria + " bytes | Tasa: " + Math.Round(Porcentaje, 2) + "%";
        }
    }
}