# Universal Compressor 📦

![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![Framework](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/License-GPLv3-green)

**Instituto Tecnológico de Costa Rica**
**Curso:** Algoritmos y Estructuras de Datos II
**Tarea Extraclase #4**

Universal Compressor es una aplicación de escritorio desarrollada en C# (.NET 8.0) que permite comprimir y descomprimir archivos de texto utilizando tres algoritmos clásicos. La herramienta genera archivos con un formato binario propio (`.myzip`) y proporciona un análisis detallado del rendimiento de cada algoritmo en tiempo real.

---

## 👥 Equipo de Desarrollo

| Estudiante | Carné | Rol / Responsabilidades |
| :--- | :--- | :--- |
| **Camila Lizano Brenes** | 2024255324 | **Interfaz Gráfica (UI)** <br> Arquitectura de la aplicación, manejo de eventos y orquestación. |
| **Jimena Castillo Campos** | 2024090066 | **Algoritmos I** <br> Implementación de Huffman (Árbol binario) y LZ77 (Ventana deslizante). |
| **Dylan Mora Porras** | 2024080786 | **Algoritmos II & Backend** <br> Implementación de LZ78, Módulo de Estadísticas y Empaquetado binario (`.myzip`). |

---

## 🚀 Funcionalidades

### 1. Algoritmos de Compresión
El usuario puede seleccionar entre tres métodos distintos para procesar sus archivos:
* **Huffman:** Codificación basada en frecuencias de caracteres.
* **LZ77:** Compresión basada en diccionario con ventana deslizante.
* **LZ78:** Compresión basada en diccionario dinámico.

### 2. Métricas de Rendimiento
Cada operación es analizada para proporcionar datos técnicos:
* ⏱️ **Tiempo:** Duración exacta del proceso (medido en milisegundos).
* 💾 **Memoria:** Consumo real de memoria RAM (delta entre inicio y fin del proceso).
* 📉 **Tasa de Compresión:** Porcentaje de reducción logrado respecto al archivo original.

### 3. Formato de Archivo (.myzip)
La aplicación implementa un empaquetador binario personalizado que estructura los datos de la siguiente manera para asegurar la integridad y la correcta descompresión:

| Orden | Tipo de Dato | Tamaño | Descripción |
| :--- | :--- | :--- | :--- |
| 1 | `byte` | 1 byte | **ID Algoritmo** (0=Huffman, 1=LZ77, 2=LZ78) |
| 2 | `long` | 8 bytes | **Tamaño Original** (Bytes del archivo antes de comprimir) |
| 3 | `int` | 4 bytes | **Longitud del Nombre** del archivo original |
| 4 | `bytes` | Variable | **Nombre Original** (Codificado en UTF-8) |
| 5 | `bytes` | Variable | **Payload** (Datos comprimidos por el algoritmo seleccionado) |

---

## 📖 Guía de Uso

### Requisitos Previos
* Sistema Operativo Windows.
* .NET Desktop Runtime 8.0 (si se ejecuta el binario) o Visual Studio 2022 (para compilar).

### Instrucciones paso a paso

**1. Compresión de Archivos:**
1.  Ejecute la aplicación.
2.  Haga clic en el botón **"Cargar archivo"** y seleccione cualquier archivo de texto plano (`.txt`). El contenido se visualizará en el panel izquierdo.
3.  Seleccione el algoritmo deseado en la lista desplegable (Huffman, LZ77 o LZ78).
4.  Haga clic en el botón **"Comprimir"**.
5.  Se abrirá un cuadro de diálogo para guardar el archivo resultante con extensión `.myzip`.
6.  Al finalizar, observe las estadísticas de Tiempo, Memoria y Tasa en la parte inferior izquierda.

**2. Descompresión de Archivos:**
1.  Haga clic en el botón **"Descomprimir"**.
2.  Busque y seleccione un archivo `.myzip` creado previamente con esta herramienta.
3.  La aplicación detectará automáticamente qué algoritmo se utilizó, recuperará el nombre original y mostrará el texto descomprimido en el panel derecho (`txtResultado`).
4.  Aparecerá un mensaje confirmando el nombre del archivo original recuperado.

---

## 🛠️ Tecnologías Utilizadas
* **Lenguaje:** C#
* **Framework:** .NET 8.0
* **Interfaz:** Windows Forms (WinForms)
* **IDE:** Microsoft Visual Studio 2022