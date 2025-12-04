namespace UniversalCompressor.UI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnCargarArchivo = new System.Windows.Forms.Button();
            this.btnComprimir = new System.Windows.Forms.Button();
            this.btnDescomprimir = new System.Windows.Forms.Button();
            this.cboAlgoritmo = new System.Windows.Forms.ComboBox();
            this.txtContenido = new System.Windows.Forms.TextBox();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.lblTiempo = new System.Windows.Forms.Label();
            this.lblMemoria = new System.Windows.Forms.Label();
            this.lblTasa = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCargarArchivo
            // 
            this.btnCargarArchivo.Location = new System.Drawing.Point(12, 12);
            this.btnCargarArchivo.Name = "btnCargarArchivo";
            this.btnCargarArchivo.Size = new System.Drawing.Size(140, 27);
            this.btnCargarArchivo.TabIndex = 0;
            this.btnCargarArchivo.Text = "Cargar archivo";
            this.btnCargarArchivo.UseVisualStyleBackColor = true;
            this.btnCargarArchivo.Click += new System.EventHandler(this.btnCargarArchivo_Click);
            // 
            // btnComprimir
            // 
            this.btnComprimir.Location = new System.Drawing.Point(12, 90);
            this.btnComprimir.Name = "btnComprimir";
            this.btnComprimir.Size = new System.Drawing.Size(140, 27);
            this.btnComprimir.TabIndex = 1;
            this.btnComprimir.Text = "Comprimir";
            this.btnComprimir.UseVisualStyleBackColor = true;
            this.btnComprimir.Click += new System.EventHandler(this.btnComprimir_Click);
            // 
            // btnDescomprimir
            // 
            this.btnDescomprimir.Location = new System.Drawing.Point(12, 123);
            this.btnDescomprimir.Name = "btnDescomprimir";
            this.btnDescomprimir.Size = new System.Drawing.Size(140, 27);
            this.btnDescomprimir.TabIndex = 2;
            this.btnDescomprimir.Text = "Descomprimir";
            this.btnDescomprimir.UseVisualStyleBackColor = true;
            this.btnDescomprimir.Click += new System.EventHandler(this.btnDescomprimir_Click);
            // 
            // cboAlgoritmo
            // 
            this.cboAlgoritmo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlgoritmo.FormattingEnabled = true;
            this.cboAlgoritmo.Items.AddRange(new object[] {
            "Huffman",
            "LZ77",
            "LZ78"});
            this.cboAlgoritmo.Location = new System.Drawing.Point(12, 55);
            this.cboAlgoritmo.Name = "cboAlgoritmo";
            this.cboAlgoritmo.Size = new System.Drawing.Size(140, 23);
            this.cboAlgoritmo.TabIndex = 3;
            // 
            // txtContenido
            // 
            this.txtContenido.Location = new System.Drawing.Point(170, 12);
            this.txtContenido.Multiline = true;
            this.txtContenido.Name = "txtContenido";
            this.txtContenido.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContenido.Size = new System.Drawing.Size(380, 200);
            this.txtContenido.TabIndex = 4;
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(170, 230);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(380, 200);
            this.txtResultado.TabIndex = 5;
            // 
            // lblTiempo
            // 
            this.lblTiempo.AutoSize = true;
            this.lblTiempo.Location = new System.Drawing.Point(12, 180);
            this.lblTiempo.Name = "lblTiempo";
            this.lblTiempo.Size = new System.Drawing.Size(54, 15);
            this.lblTiempo.TabIndex = 6;
            this.lblTiempo.Text = "Tiempo:";
            // 
            // lblMemoria
            // 
            this.lblMemoria.AutoSize = true;
            this.lblMemoria.Location = new System.Drawing.Point(12, 205);
            this.lblMemoria.Name = "lblMemoria";
            this.lblMemoria.Size = new System.Drawing.Size(60, 15);
            this.lblMemoria.TabIndex = 7;
            this.lblMemoria.Text = "Memoria:";
            // 
            // lblTasa
            // 
            this.lblTasa.AutoSize = true;
            this.lblTasa.Location = new System.Drawing.Point(12, 230);
            this.lblTasa.Name = "lblTasa";
            this.lblTasa.Size = new System.Drawing.Size(34, 15);
            this.lblTasa.TabIndex = 8;
            this.lblTasa.Text = "Tasa:";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(564, 450);
            this.Controls.Add(this.lblTasa);
            this.Controls.Add(this.lblMemoria);
            this.Controls.Add(this.lblTiempo);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.txtContenido);
            this.Controls.Add(this.cboAlgoritmo);
            this.Controls.Add(this.btnDescomprimir);
            this.Controls.Add(this.btnComprimir);
            this.Controls.Add(this.btnCargarArchivo);
            this.Name = "Form1";
            this.Text = "Universal Compressor UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnCargarArchivo;
        private System.Windows.Forms.Button btnComprimir;
        private System.Windows.Forms.Button btnDescomprimir;
        private System.Windows.Forms.ComboBox cboAlgoritmo;
        private System.Windows.Forms.TextBox txtContenido;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label lblTiempo;
        private System.Windows.Forms.Label lblMemoria;
        private System.Windows.Forms.Label lblTasa;
    }
}
