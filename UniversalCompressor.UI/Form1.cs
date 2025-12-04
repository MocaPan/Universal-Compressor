using System;
using System.IO;
using System.Windows.Forms;
using UniversalCompressor.Logica;

namespace UniversalCompressor.UI
{
    public partial class Form1 : Form
    {
        string rutaArchivoOriginal = "";
        string nombreArchivoOriginal = "";

        public Form1()
        {
            InitializeComponent();
            cboAlgoritmo.SelectedIndex = 2;
        }

        private void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de texto|*.txt|Todos|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                rutaArchivoOriginal = ofd.FileName;
                nombreArchivoOriginal = Path.GetFileName(ofd.FileName);
                txtContenido.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void btnComprimir_Click(object sender, EventArgs e)
        {
            if (txtContenido.Text.Length == 0)
            {
                MessageBox.Show("Primero cargue un archivo o escriba texto.");
                return;
            }

            string textoOriginal = txtContenido.Text;
            long tamanoOriginal = textoOriginal.Length;

            Empaquetador pack = new Empaquetador();
            MisEstadisticas stats = new MisEstadisticas();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MyZip|*.myzip";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            string rutaGuardar = sfd.FileName;

            stats.Empezar();

            int algoritmoID = cboAlgoritmo.SelectedIndex;
            byte[] datosComprimidos = null;

            switch (algoritmoID)
            {
                case 0:
                    datosComprimidos = Huffman.Comprimir(System.Text.Encoding.UTF8.GetBytes(textoOriginal));
                    break;

                case 1:
                    var tuplas77 = LZ77.Comprimir(System.Text.Encoding.UTF8.GetBytes(textoOriginal));
                    datosComprimidos = LZ77.TuplasABytes(tuplas77);
                    break;

                case 2:
                    LZ78 lz = new LZ78();
                    var listaTuplas = lz.Comprimir(textoOriginal);
                    datosComprimidos = lz.TuplasABytes(listaTuplas);
                    break;
            }

            pack.Guardar(rutaGuardar, datosComprimidos, nombreArchivoOriginal, tamanoOriginal, algoritmoID);

            stats.Terminar(tamanoOriginal, datosComprimidos.Length);

            lblTiempo.Text = "Tiempo: " + stats.Tiempo + " ms";
            lblMemoria.Text = "Memoria: " + stats.Memoria + " bytes";
            lblTasa.Text = "Tasa: " + Math.Round(stats.Porcentaje, 2) + "%";

            MessageBox.Show("Archivo comprimido correctamente.");
        }

        private void btnDescomprimir_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MyZip|*.myzip";

            if (ofd.ShowDialog() != DialogResult.OK) return;

            Empaquetador pack = new Empaquetador();
            var info = pack.Leer(ofd.FileName);

            string textoRecuperado = "";

            switch (info.TipoAlgoritmo)
            {
                case 0:
                    textoRecuperado = System.Text.Encoding.UTF8.GetString(Huffman.Descomprimir(info.Data));
                    break;

                case 1:
                    var tuplas = LZ77.BytesATuplas(info.Data);
                    textoRecuperado = System.Text.Encoding.UTF8.GetString(LZ77.Descomprimir(tuplas));
                    break;

                case 2:
                    LZ78 lz = new LZ78();
                    var tuplas78 = lz.BytesATuplas(info.Data);
                    textoRecuperado = lz.Descomprimir(tuplas78);
                    break;
            }

            txtResultado.Text = textoRecuperado;
            MessageBox.Show("Archivo original: " + info.Nombre);
        }
    }
}

