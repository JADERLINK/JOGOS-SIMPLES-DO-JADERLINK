namespace JOGODOSPONTINHOS
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxCenario = new System.Windows.Forms.PictureBox();
            this.timerAtualizaDesign = new System.Windows.Forms.Timer(this.components);
            this.labelTotalDePontos = new System.Windows.Forms.Label();
            this.buttonNovoJogo = new System.Windows.Forms.Button();
            this.labelYouDead = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenario)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCenario
            // 
            this.pictureBoxCenario.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxCenario.Location = new System.Drawing.Point(5, 5);
            this.pictureBoxCenario.Name = "pictureBoxCenario";
            this.pictureBoxCenario.Size = new System.Drawing.Size(600, 600);
            this.pictureBoxCenario.TabIndex = 0;
            this.pictureBoxCenario.TabStop = false;
            // 
            // timerAtualizaDesign
            // 
            this.timerAtualizaDesign.Enabled = true;
            this.timerAtualizaDesign.Interval = 1;
            this.timerAtualizaDesign.Tick += new System.EventHandler(this.timerAtualizaDesign_Tick);
            // 
            // labelTotalDePontos
            // 
            this.labelTotalDePontos.AutoSize = true;
            this.labelTotalDePontos.Location = new System.Drawing.Point(99, 615);
            this.labelTotalDePontos.Name = "labelTotalDePontos";
            this.labelTotalDePontos.Size = new System.Drawing.Size(99, 13);
            this.labelTotalDePontos.TabIndex = 1;
            this.labelTotalDePontos.Text = "Total De Pontos : 0";
            // 
            // buttonNovoJogo
            // 
            this.buttonNovoJogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNovoJogo.Location = new System.Drawing.Point(12, 611);
            this.buttonNovoJogo.Name = "buttonNovoJogo";
            this.buttonNovoJogo.Size = new System.Drawing.Size(80, 23);
            this.buttonNovoJogo.TabIndex = 2;
            this.buttonNovoJogo.Text = "NOVO JOGO";
            this.buttonNovoJogo.UseVisualStyleBackColor = true;
            this.buttonNovoJogo.Click += new System.EventHandler(this.buttonNovoJogo_Click);
            this.buttonNovoJogo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // labelYouDead
            // 
            this.labelYouDead.AutoSize = true;
            this.labelYouDead.BackColor = System.Drawing.Color.Transparent;
            this.labelYouDead.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYouDead.ForeColor = System.Drawing.Color.Purple;
            this.labelYouDead.Location = new System.Drawing.Point(326, 608);
            this.labelYouDead.Name = "labelYouDead";
            this.labelYouDead.Size = new System.Drawing.Size(151, 31);
            this.labelYouDead.TabIndex = 3;
            this.labelYouDead.Text = "You Dead!";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 637);
            this.Controls.Add(this.labelYouDead);
            this.Controls.Add(this.buttonNovoJogo);
            this.Controls.Add(this.labelTotalDePontos);
            this.Controls.Add(this.pictureBoxCenario);
            this.MinimumSize = new System.Drawing.Size(620, 670);
            this.Name = "Form1";
            this.Text = "JOGO DOS PONTINHOS";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCenario;
        private System.Windows.Forms.Timer timerAtualizaDesign;
        private System.Windows.Forms.Label labelTotalDePontos;
        private System.Windows.Forms.Button buttonNovoJogo;
        private System.Windows.Forms.Label labelYouDead;
    }
}

