using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;  
using System.Drawing.Text;
using System.Drawing.Drawing2D;

//Color Branco = Color.White; //Color Preto = Color.Black; // Color.FromArgb(0, 0, 0);

namespace JOGODOSPONTINHOS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            buttonNovoJogo.Enabled = false;
            labelYouDead.Hide();
        }

        bool SeEhPermitidoJogar = true;

        // tamanho
        int TamanhoDoCenarioX = 600;
        int TamanhoDoCenarioY = 600;
        int TamanhoDosObjetosX = 20;
        int TamanhoDosObjetosY = 20;

        Bitmap Cenario;
        Graphics EditarGrafico;
        Rectangle FundoDoCenario;

       (int x, int y) Player;
     
        class Objeto
        {
            public int x;
            public int y;
            public int id;
            public string Tipo = "null";
        }

        class Casa
        {
            public bool Ocupada = false;
            public string Tipo = "null";

        }

        Casa CasaVasia = new Casa();
        Casa CasaComida = new Casa();
        Casa CasaTNT = new Casa();
        Casa CasaPlayer = new Casa();

        Casa[,] Casas = new Casa[30, 30];


        List<Objeto> Comidas = new List<Objeto>();

        List<Objeto> TNTs = new List<Objeto>();


        int TeclaPrecionada = 0;

        int TotalDePontos = 0;


        private void Form1_Load(object sender, EventArgs e)
        {
            CasaVasia.Ocupada = false;
            CasaVasia.Tipo = "null";
            CasaComida.Ocupada = true;
            CasaComida.Tipo = "COMIDA";
            CasaTNT.Ocupada = true;
            CasaTNT.Tipo = "TNT";
            CasaPlayer.Ocupada = true;
            CasaPlayer.Tipo = "PLAYER";


            for (int i = 0; i < 30; i++)
            {
                for (int i2 = 0; i2 < 30; i2++)
                {
                    Casas[i, i2] = CasaVasia;
                }
            }


            Player.x = 15;
            Player.y = 15;

            Cenario = new Bitmap(TamanhoDoCenarioX, TamanhoDoCenarioY, PixelFormat.Format24bppRgb);
            EditarGrafico = Graphics.FromImage(Cenario);
            FundoDoCenario = new Rectangle(0, 0, TamanhoDoCenarioX, TamanhoDoCenarioY);
            EditarGrafico.FillRectangle(Brushes.White, FundoDoCenario);
            EditarGrafico.Save();


            //AddComidaNocenario(6, 7);
            //AddTNTNocenario(25, 8);

            AtualizaDisplayer();

        }

        int ContadorDetempoComida = 0;
        int ContadorDetempoTNT = 0;

        void LogicaDoJogo()
        {
            if (SeEhPermitidoJogar == true)
            {
                // meu numero aleatorio
                Random Aleatorio = new Random();


                ContadorDetempoComida += 1;
                ContadorDetempoTNT += 1;

                int ComidaPega = -1;

                // remove comida
                Objeto ComidaAserRemovida = new Objeto();

                for (int i = 0; i < Comidas.Count; i++)
                {
                    if (Player.x == Comidas[i].x && Player.y == Comidas[i].y)
                    {
                        ComidaPega = Comidas[i].id;
                        ComidaAserRemovida = Comidas[i];
                    }
                }

                if (ComidaPega != -1)
                {
                    Comidas.Remove(ComidaAserRemovida);
                    TotalDePontos += 1;

                    Casas[ComidaAserRemovida.x, ComidaAserRemovida.y] = CasaVasia;
                }

                // pisa nas TNT
                for (int i = 0; i < TNTs.Count; i++)
                {
                    if (Player.x == TNTs[i].x && Player.y == TNTs[i].y)
                    {
                        SeEhPermitidoJogar = false;
                        buttonNovoJogo.Enabled = true;
                        labelYouDead.Show();
                    }
                }


                // add comida
                if (Comidas.Count == 0)
                {
                    ContadorDetempoComida = 0;

                    bool PodeContinuar = true;

                    int Px = Aleatorio.Next(30);
                    int Py = Aleatorio.Next(30);

                    while (PodeContinuar == true)
                    {
                        //Console.WriteLine("esta ativo");

                        if (Casas[Px, Py].Ocupada == false)
                        {
                            PodeContinuar = false;
                        }
                        else
                        {
                            Px = Aleatorio.Next(30);
                            Py = Aleatorio.Next(30);
                        }

                    }
            
                    AddComidaNocenario(Px, Py);
                }
       


                if (ContadorDetempoComida == 200)
                {
                    for (int i = 0; i < Comidas.Count; i++)
                    {
                        Casas[Comidas[i].x, Comidas[i].y] = CasaVasia;
                    }

                    Comidas.Clear();

                    ContadorDetempoComida = 0;

                    //Random Aleatorio = new Random();
                    int Px = Aleatorio.Next(30);
                    int Py = Aleatorio.Next(30);

                    bool PodeContinuar = true;

                    while (PodeContinuar == true)
                    {
                        //Console.WriteLine("esta ativo");

                        if (Casas[Px, Py].Ocupada == false)
                        {
                            PodeContinuar = false;
                        }
                        else
                        {
                            Px = Aleatorio.Next(30);
                            Py = Aleatorio.Next(30);
                        }

                    }


                    AddComidaNocenario(Px, Py);

                }

                // add as tnt

                if (ContadorDetempoTNT == 250)
                {
                    /*
                    for (int i = 0; i < TNTs.Count; i++)
                    {
                        Casas[TNTs[i].x, TNTs[i].y] = CasaVasia;
                       
                    }

                    TNTs.Clear();
                    */


                    ContadorDetempoTNT = 0;

                    //Random Aleatorio = new Random();
                    int Px = Aleatorio.Next(30);
                    int Py = Aleatorio.Next(30);

                    bool PodeContinuar = true;

                    while (PodeContinuar == true)
                    {
                        //Console.WriteLine("esta ativo");

                        if (Casas[Px, Py].Ocupada == false)
                        {
                            PodeContinuar = false;
                        }
                        else
                        {
                            Px = Aleatorio.Next(30);
                            Py = Aleatorio.Next(30);
                        }

                    }


                    AddTNTNocenario(Px, Py);

                }

            }
        }

        void AtualizaDisplayer()
        {
            Rectangle Cordenadasplayer = new Rectangle(Player.x * TamanhoDosObjetosX, Player.y * TamanhoDosObjetosY, TamanhoDosObjetosX, TamanhoDosObjetosY);
            EditarGrafico.FillRectangle(Brushes.White, FundoDoCenario);
            EditarGrafico.FillRectangle(Brushes.Black, Cordenadasplayer);

            for (int i = 0; i < Comidas.Count; i++)
            {
                Rectangle RetanguloComida = new Rectangle(Comidas[i].x * TamanhoDosObjetosX, Comidas[i].y * TamanhoDosObjetosY, TamanhoDosObjetosX, TamanhoDosObjetosY);
                EditarGrafico.FillRectangle(Brushes.Green, RetanguloComida);
                
            }

            for (int i = 0; i < TNTs.Count; i++)
            {
                Rectangle RetanguloTNT = new Rectangle(TNTs[i].x * TamanhoDosObjetosX, TNTs[i].y * TamanhoDosObjetosY, TamanhoDosObjetosX, TamanhoDosObjetosY);
                EditarGrafico.FillRectangle(Brushes.Red, RetanguloTNT);

            }


            EditarGrafico.Save();

            pictureBoxCenario.Image = Cenario;
            pictureBoxCenario.SizeMode = PictureBoxSizeMode.Zoom;

            labelTotalDePontos.Text = "Total de pontos: " + TotalDePontos;
        }

        int TotalisadorDeIDsDasComidas = 0;

        void AddComidaNocenario(int x, int y)
        {
            Objeto NovaComida = new Objeto();
            NovaComida.x = x;
            NovaComida.y = y;
            NovaComida.id = TotalisadorDeIDsDasComidas;
            TotalisadorDeIDsDasComidas++;
            Comidas.Add(NovaComida);
            Casas[x, y] = CasaComida;
        }

        int TotalisadorDeIDsDasTNTs = 0;

        void AddTNTNocenario(int x, int y)
        {
            Objeto NovaTNT = new Objeto();
            NovaTNT.x = x;
            NovaTNT.y = y;
            NovaTNT.id = TotalisadorDeIDsDasTNTs;
            TotalisadorDeIDsDasTNTs++;
            TNTs.Add(NovaTNT);
            Casas[x, y]= CasaTNT;
        }



        void MovePlayer()
        {
            if (SeEhPermitidoJogar == true)
            {

                if (TeclaPrecionada == 38 || TeclaPrecionada == 87)
                {
                    if (Player.y + -1 >= 0)
                    {
                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.y += -1;

                        Casas[Player.x, Player.y] = CasaPlayer;
                    }
                }

                if (TeclaPrecionada == 40 || TeclaPrecionada == 83)
                {
                    if (Player.y + 1 < 30)
                    {
                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.y += 1;

                        Casas[Player.x, Player.y] = CasaPlayer;
                    }


                }

                if (TeclaPrecionada == 37 || TeclaPrecionada == 65)
                {
                    if (Player.x + -1 >= 0)
                    {
                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.x += -1;

                        Casas[Player.x, Player.y] = CasaPlayer;
                    }

                }
                if (TeclaPrecionada == 39 || TeclaPrecionada == 68)
                {
                    if (Player.x + 1 < 30)
                    {
                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.x += 1;

                        Casas[Player.x, Player.y] = CasaPlayer;
                    }
                }

                TeclaPrecionada = 0;
            }
        }
        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            TeclaPrecionada = e.KeyValue;
        }

        private void timerAtualizaDesign_Tick(object sender, EventArgs e)
        {
            MovePlayer();
            LogicaDoJogo();
            AtualizaDisplayer();
        }

        private void buttonNovoJogo_Click(object sender, EventArgs e)
        {
            Player.x = 15;
            Player.y = 15;

            TotalDePontos = 0;

            Comidas.Clear();
            TNTs.Clear();

            for (int i = 0; i < 30; i++)
            {
                for (int i2 = 0; i2 < 30; i2++)
                {
                    Casas[i, i2] = CasaVasia;
                }
            }

            SeEhPermitidoJogar = true;

            buttonNovoJogo.Enabled = false;
            labelYouDead.Hide();
        }
    }
}
