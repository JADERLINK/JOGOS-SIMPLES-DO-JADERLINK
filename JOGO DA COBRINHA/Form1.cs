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

namespace JOGODACOBRINHA
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

        // imagem do cenario
        Bitmap Cenario;
        Graphics EditarGrafico;
        Rectangle FundoDoCenario;

        (int x, int y) Player;
        (int x, int y) OldPlayer;

        // objetos do jogo
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

        List<Objeto> PartesPlayer = new List<Objeto>();

        int TeclaPrecionada = 0;

        int TotalDePontos = 0;

        int DirecaoQueOPlayerNaoPodeIr = 3;
        // 0 null
        // 1 n pode cima, foi pra baixo
        // 2 n pode baixo, foi pra cima
        // 3 n pode esquerda, foi pra direita
        // 4 n pode direita, foi pra esquerda 


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



            AddPartesPlayerNocenario(13, 15);
            AddPartesPlayerNocenario(14, 15);


            Player.x = 15;
            Player.y = 15;
            OldPlayer.x = 15;
            OldPlayer.y = 15;

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

                // se player ultrapassa parede, morre
                if (SePassouparede == true)
                {
                    SeEhPermitidoJogar = false;
                    buttonNovoJogo.Enabled = true;
                    labelYouDead.Show();
                }


                bool PegouComida = false;
                
                
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

                    PegouComida = true;
                }



                // move o player cobrinha
                if (OldPlayer.x != Player.x || OldPlayer.y != Player.y)
                {

                    AddPartesPlayerNocenario(OldPlayer.x, OldPlayer.y);

                    if (PegouComida == false)
                    {
                        Objeto ObjetoAserRemovido = PartesPlayer[0];
                        PartesPlayer.Remove(ObjetoAserRemovido);

                        Casas[ObjetoAserRemovido.x, ObjetoAserRemovido.y] = CasaVasia;
                    }
                   

                    OldPlayer = Player;
                }




                // pisa nas TNT // morre
                for (int i = 0; i < TNTs.Count; i++)
                {
                    if (Player.x == TNTs[i].x && Player.y == TNTs[i].y)
                    {
                        SeEhPermitidoJogar = false;
                        buttonNovoJogo.Enabled = true;
                        labelYouDead.Show();
                    }
                }

                // player pisou em uma parte de player // morre

                for (int i = 0; i < PartesPlayer.Count; i++)
                {
                    if (Player.x == PartesPlayer[i].x && Player.y == PartesPlayer[i].y)
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
       

                if (ContadorDetempoComida == 400)
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

                if (ContadorDetempoTNT == 300)
                {
                    
                    for (int i = 0; i < TNTs.Count; i++)
                    {
                        Casas[TNTs[i].x, TNTs[i].y] = CasaVasia;
                       
                    }

                    TNTs.Clear();
                    

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

           
            

            for (int i = 0; i < PartesPlayer.Count; i++)
            {
                Rectangle RetanguloParteplayer = new Rectangle(PartesPlayer[i].x * TamanhoDosObjetosX, PartesPlayer[i].y * TamanhoDosObjetosY, TamanhoDosObjetosX, TamanhoDosObjetosY);
                EditarGrafico.FillRectangle(Brushes.Black, RetanguloParteplayer);
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

        int TotalisadorDeIDsDasPartesPlayer = 0;

        void AddPartesPlayerNocenario(int x, int y)
        {
            Objeto PartePlayer = new Objeto();
            PartePlayer.x = x;
            PartePlayer.y = y;
            PartePlayer.id = TotalisadorDeIDsDasPartesPlayer;
            TotalisadorDeIDsDasPartesPlayer++;
            PartesPlayer.Add(PartePlayer);
            Casas[x, y] = CasaPlayer;
        }


        bool SeHumanoJogou = false;

        void MovePlayer()
        {
            if (SeEhPermitidoJogar == true)
            {

                OldPlayer = Player;

                if (TeclaPrecionada == 38 || TeclaPrecionada == 87) // cima
                {
                    if (Player.y + -1 >= 0 && DirecaoQueOPlayerNaoPodeIr != 1)//&& DirecaoQueOPlayerNaoPodeIr != 2)
                    {

                        DirecaoAIr = 2;

                        DirecaoQueOPlayerNaoPodeIr = 2;

                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.y += -1;

                        Casas[Player.x, Player.y] = CasaPlayer;

                        SeHumanoJogou = true;
                    }
                }

                if (TeclaPrecionada == 40 || TeclaPrecionada == 83) // baixo
                {
                    if (Player.y + 1 < 30 && DirecaoQueOPlayerNaoPodeIr != 2)//&& DirecaoQueOPlayerNaoPodeIr != 1)
                    {
                        DirecaoAIr = 1;

                        DirecaoQueOPlayerNaoPodeIr = 1;

                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.y += 1;

                        Casas[Player.x, Player.y] = CasaPlayer;

                        SeHumanoJogou = true;
                    }


                }

                if (TeclaPrecionada == 37 || TeclaPrecionada == 65) // esquerda
                {
                    if (Player.x + -1 >= 0 && DirecaoQueOPlayerNaoPodeIr != 3)// && DirecaoQueOPlayerNaoPodeIr != 4)
                    {

                        DirecaoAIr = 4;

                        DirecaoQueOPlayerNaoPodeIr = 4;

                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.x += -1;

                        Casas[Player.x, Player.y] = CasaPlayer;

                        SeHumanoJogou = true;
                    }

                }

                if (TeclaPrecionada == 39 || TeclaPrecionada == 68) // direita
                {
                    if (Player.x + 1 < 30 && DirecaoQueOPlayerNaoPodeIr != 4) //&& DirecaoQueOPlayerNaoPodeIr != 3)
                    {

                        DirecaoAIr = 3;

                        DirecaoQueOPlayerNaoPodeIr = 3;

                        Casas[Player.x, Player.y] = CasaVasia;

                        Player.x += 1;

                        Casas[Player.x, Player.y] = CasaPlayer;

                        SeHumanoJogou = true;
                    }
                }

                TeclaPrecionada = 0;


            }

        }

        void AutoMovePlayer()
        {

            if (SeEhPermitidoJogar == true)
            {
                //Console.WriteLine(ContatadorDeTempoMovPlayer);

                if (SeHumanoJogou == false)
                {
                    if (ContatadorDeTempoMovPlayer >= 13)
                    {
                        ContatadorDeTempoMovPlayer = 0;

                        if (DirecaoAIr == 2)
                        {
                            int Check = Player.y + -1;

                            if (Check < 0)
                            {
                                SePassouparede = true;
                            }

                            if (SePassouparede == false) // cima
                            {

                                //DirecaoQueOPlayerNaoPodeIr = 2;

                                Casas[Player.x, Player.y] = CasaVasia;

                                Player.y += -1;

                                Casas[Player.x, Player.y] = CasaPlayer;
                            }

                        }

                        if (DirecaoAIr == 1)
                        {
                            int Check = Player.y + 1;

                            if (Check >= 30)
                            {
                                SePassouparede = true;
                            }

                            if (SePassouparede == false) // baixo
                            {

                                //  DirecaoQueOPlayerNaoPodeIr = 1;

                                Casas[Player.x, Player.y] = CasaVasia;

                                Player.y += 1;



                                Casas[Player.x, Player.y] = CasaPlayer;
                            }

                        }

                        if (DirecaoAIr == 4)
                        {
                            int Check = Player.x + -1;

                            if (Check < 0)
                            {
                                SePassouparede = true;
                            }

                            if (SePassouparede == false) // esquerda
                            {

                                //DirecaoQueOPlayerNaoPodeIr = 4;

                                Casas[Player.x, Player.y] = CasaVasia;

                                Player.x += -1;

                                Casas[Player.x, Player.y] = CasaPlayer;
                            }

                        }

                        if (DirecaoAIr == 3)
                        {
                            int Check = Player.x + 1;


                            if (Check >= 30)
                            {
                                SePassouparede = true;
                            }

                            if (SePassouparede == false) //direita
                            {

                                // DirecaoQueOPlayerNaoPodeIr = 3;

                                Casas[Player.x, Player.y] = CasaVasia;

                                Player.x += 1;

                                Casas[Player.x, Player.y] = CasaPlayer;
                            }

                        }

                    }

                }

                ContatadorDeTempoMovPlayer++;
                SeHumanoJogou = false;
            }
        }


        int ContatadorDeTempoMovPlayer = 0;
        int DirecaoAIr = 3;
        bool SePassouparede = false;


        bool Pausado = false;

        void pausarJogo(KeyEventArgs e)
        {
            if (e.KeyValue == 80 && SeEhPermitidoJogar == true)
            {
                if (Pausado == false)
                {
                    Pausado = true;
                    labelYouDead.Text = "Jogo Pausado";
                    labelYouDead.Show();
                }
                else
                {
                    Pausado = false;
                    labelYouDead.Text = "You Dead!";
                    labelYouDead.Hide();
                }
            }


        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine(e.KeyValue);
            TeclaPrecionada = e.KeyValue;
            pausarJogo(e);
        }

        // timer q faz o jogo rodar
        private void timerAtualizaDesign_Tick(object sender, EventArgs e)
        {
            if (Pausado == false)
            {
                MovePlayer();
                // remova "AutoMovePlayer();", para a acobrinha parar de andar sozinha.
                AutoMovePlayer();
                LogicaDoJogo();
                AtualizaDisplayer();
            }
        }

        private void buttonNovoJogo_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int i2 = 0; i2 < 30; i2++)
                {
                    Casas[i, i2] = CasaVasia;
                }
            }

            Player.x = 15;
            Player.y = 15;

            PartesPlayer.Clear();

            AddPartesPlayerNocenario(13, 15);
            AddPartesPlayerNocenario(14, 15);

            DirecaoQueOPlayerNaoPodeIr = 3;
            DirecaoAIr = 3;

            SePassouparede = false;

            TotalDePontos = 0;

            Comidas.Clear();
            TNTs.Clear();

            SeEhPermitidoJogar = true;

            buttonNovoJogo.Enabled = false;
            labelYouDead.Hide();
        }
    }
}
