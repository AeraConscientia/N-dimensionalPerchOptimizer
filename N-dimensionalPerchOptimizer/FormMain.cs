using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace N_dimensionalPerchOptimizer
{
    public partial class FormMain : Form
    {
        /// <summary>Размерность задачи</summary>
        public int N_dim = 0;

        Graphics graphics = null;

        private int MaxIteration = 0;
        private Perch resultBest;

        /// <summary>Количество стай</summary>
        public int NumFlocks = 0;
        /// <summary>Количество окуней в стае</summary>
        public int NumPerchInFlock = 0;
        /// <summary>Количество шагов до окончания движения внутри стаи</summary>
        public int NStep = 0;
        /// <summary>Глубина продвижения внутри котла</summary>
        public double sigma = 0;

        /// <summary>Параметр распределения Леви</summary>
        public double lambda = 0;
        /// <summary>Величина шага</summary>
        public double alfa = 0;

        /// <summary>Число перекоммутаций</summary>
        public int PRmax = 0;
        /// <summary>Число шагов при перекоммутации</summary>
        public int deltapr = 0;

        //private int population = 0;
        public int population = 0;

        public FormMain()
        {
            InitializeComponent();
            InitDataGridView();
        }

        /// <summary>Загрузка параметров аглоритма</summary>
        private void InitDataGridView()
        {
            dataGridView2.RowCount = 6;
            dataGridView2.Rows[0].Cells[0].Value = "Кол-во шагов до окончания движения";
            dataGridView2.Rows[0].Cells[1].Value = 100;

            dataGridView2.Rows[1].Cells[0].Value = "Максимальное количество итераций";
            dataGridView2.Rows[1].Cells[1].Value = 4;

            dataGridView2.Rows[2].Cells[0].Value = "Количество стай";
            dataGridView2.Rows[2].Cells[1].Value = 4;

            dataGridView2.Rows[3].Cells[0].Value = "Количество окуней в стае";
            dataGridView2.Rows[3].Cells[1].Value = 3;

            dataGridView2.Rows[4].Cells[0].Value = "Число перекоммутаций";
            dataGridView2.Rows[4].Cells[1].Value = 10;

            dataGridView2.Rows[5].Cells[0].Value = "Число шагов в перекоммутации";
            dataGridView2.Rows[5].Cells[1].Value = 5;


            dataGridView4.RowCount = 2;
            dataGridView4.Rows[0].Cells[0].Value = "Параметр распределения";
            dataGridView4.Rows[0].Cells[1].Value = (1.5).ToString();

            dataGridView4.Rows[1].Cells[0].Value = "Величина шага";
            dataGridView4.Rows[1].Cells[1].Value = (0.6).ToString();
        }

        private void LoadParams()
        {
            // Параметры окуней
            NumFlocks       = Convert.ToInt32(dataGridView2.Rows[2].Cells[1].Value);
            NumPerchInFlock = Convert.ToInt32(dataGridView2.Rows[3].Cells[1].Value);
            NStep           = Convert.ToInt32(dataGridView2.Rows[0].Cells[1].Value);
            MaxIteration    = Convert.ToInt32(dataGridView2.Rows[1].Cells[1].Value);
            PRmax           = Convert.ToInt32(dataGridView2.Rows[4].Cells[1].Value);
            deltapr         = Convert.ToInt32(dataGridView2.Rows[5].Cells[1].Value);

            // Для Леви
            lambda  = Convert.ToDouble(dataGridView4.Rows[0].Cells[1].Value);
            alfa    = Convert.ToDouble(dataGridView4.Rows[1].Cells[1].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadParams();
            AlgorithmPerch algPerch;

            object[] X;
            object[] X2;
            object[] X3;
            object[] U, U_2, U_3;


            switch (tabControl2.SelectedIndex) // Считывание параметров для задачи
            {
                case 0:
                    N_dim = Convert.ToInt32(numericUpDownN1.Value);
                    double U1 = Convert.ToDouble(textBoxU1.Text);
                    double U2 = Convert.ToDouble(textBoxU2.Text);
                    double x0 = Convert.ToDouble(textBoxX1.Text);
                    algPerch = new AlgorithmTask1(U1, U2, x0);

                    break;
                case 1: // тут не заглушка
                    N_dim = 3 * Convert.ToInt32(numericUpDownN2.Value);
                    double U11 = Convert.ToDouble(textBoxU11.Text);
                    double U12 = Convert.ToDouble(textBoxU12.Text);
                    double U21 = Convert.ToDouble(textBoxU21.Text);
                    double U22 = Convert.ToDouble(textBoxU22.Text);
                    double U31 = Convert.ToDouble(textBoxU31.Text);
                    double U32 = Convert.ToDouble(textBoxU32.Text);
                    double x00 = Convert.ToDouble(textBoxX11.Text);
                    double x11 = Convert.ToDouble(textBoxX22.Text);
                    double x22 = Convert.ToDouble(textBoxX33.Text);
                    algPerch = new AlgorithmTask2(U11, U12, U21, U22, U31, U32, x00, x11, x22);
                    break;
                case 2:
                    N_dim = Convert.ToInt32(numericUpDownN3.Value);
                    double U1_3 = Convert.ToDouble(textBoxU1_3.Text);
                    double U2_3 = Convert.ToDouble(textBoxU2_3.Text);
                    double x0_3 = Convert.ToDouble(textBoxX0_3.Text);
                    algPerch = new AlgorithmTask3(U1_3, U2_3, x0_3);
                    break;
                default:
                    return;
            }
            resultBest = algPerch.StartAlg(MaxIteration, NumFlocks, NumPerchInFlock, NStep, lambda, alfa, PRmax, deltapr, N_dim);

            Result result = Result.GetInstance();
            switch (tabControl2.SelectedIndex) // занесение в таблицу результатов
            {
                case 0:
                    X = new object[N_dim+1];
                    U = new object[N_dim];
                    for (int i = 0; i < N_dim; i++)
                    {
                        U[i] = result.U[i];
                    }
                    for (int i = 0; i < N_dim+1; i++)
                    {
                        X[i] = result.X[i];
                    }
                    dataGridViewX_separate.RowCount = 1;
                    dataGridViewX_separate.ColumnCount = N_dim + 1;
                    dataGridViewX_separate.Rows[0].SetValues(X);

                    dataGridViewU_separate.RowCount = 1;
                    dataGridViewU_separate.ColumnCount = N_dim;
                    
                    dataGridViewU_separate.Rows[0].SetValues(U);

                    dataGridViewX_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    dataGridViewU_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    break;
                case 1:
                    X = new object[N_dim + 3];
                    X2 = new object[N_dim + 3];
                    X3 = new object[N_dim + 3];
                    U = new object[N_dim];
                    U_2 = new object[N_dim / 3];
                    U_3 = new object[N_dim / 3];
                    for (int i = 0; i < N_dim; i++)
                    {
                        U[i] = result.U[i];
                    }
                    Array.Copy(U,    N_dim/3, U_2, 0, N_dim/3);
                    Array.Copy(U, 2* N_dim/3, U_3, 0, N_dim/3);
                    for (int i = 0; i < N_dim/3; i++)
                    {
                        X[i] = result.X[i];
                        X2[i] = result.X2[i];
                        X3[i] = result.X3[i];
                    }
                    dataGridViewX_separate.RowCount = 3;
                    dataGridViewX_separate.ColumnCount = N_dim + 3;

                    dataGridViewX_separate.Rows[0].SetValues(X);
                    dataGridViewX_separate.Rows[1].SetValues(X2);
                    dataGridViewX_separate.Rows[2].SetValues(X3);

                    dataGridViewU_separate.RowCount = 3;
                    dataGridViewU_separate.ColumnCount = N_dim;
                    dataGridViewU_separate.Rows[0].SetValues(U);
                    dataGridViewU_separate.Rows[1].SetValues(U_2);
                    dataGridViewU_separate.Rows[2].SetValues(U_3);

                    dataGridViewX_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    dataGridViewU_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    dataGridViewX_separate.Rows[1].DefaultCellStyle.Format = "n5";
                    dataGridViewU_separate.Rows[1].DefaultCellStyle.Format = "n5";
                    dataGridViewX_separate.Rows[2].DefaultCellStyle.Format = "n5";
                    dataGridViewU_separate.Rows[2].DefaultCellStyle.Format = "n5";
                    break;
                case 2:
                    X = new object[N_dim + 1];
                    U = new object[N_dim];
                    for (int i = 0; i < N_dim; i++)
                    {
                        U[i] = result.U[i];
                    }
                    for (int i = 0; i < N_dim + 1; i++)
                    {
                        X[i] = result.X[i];
                    }
                    dataGridViewX_separate.RowCount = 1;
                    dataGridViewX_separate.ColumnCount = N_dim + 1;
                    dataGridViewX_separate.Rows[0].SetValues(X);

                    dataGridViewU_separate.RowCount = 1;
                    dataGridViewU_separate.ColumnCount = N_dim;
                    dataGridViewU_separate.Rows[0].SetValues(U);

                    dataGridViewX_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    dataGridViewU_separate.Rows[0].DefaultCellStyle.Format = "n5";
                    break;
                default:
                    return;
            }

            labelMinI.Text = result.fitness.ToString();


            if (graphics == null)
                switch (tabControl2.SelectedIndex)
                {
                    case 0:
                        graphics = new Graphics(1);
                        break;
                    case 1:
                        graphics = new Graphics(3);
                        break;
                    case 2:
                        graphics = new Graphics(1);
                        break;
                    default:
                        break;
                }
            switch (tabControl2.SelectedIndex)
                {
                    case 0:
                        graphics.UpdateGraph(1);
                        break;
                    case 1:
                        graphics.UpdateGraph(3);
                        break;
                    case 2:
                        graphics.UpdateGraph(1);
                        break;
                    default:
                        break;
                }

            graphics.Show();
        }

        /// <summary>Запись протокола и его вызов</summary>
        private void buttonProtocol_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("protocol.txt", FileMode.Append, FileAccess.Write);
            StreamWriter r = new StreamWriter(fs);

//            r.Write(
//    @"
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//<>< ◄►◄ <>< ◄►◄ <>< ◄►◄ <>< ◄►◄ <>< ◄►◄ <><   ><> ►◄► ><> ►◄► ><> ►◄► ><> ►◄► ><> ►◄► ><>
//►◄►                                                                                   ◄►◄
//<><                      Протокол применения метода стаи окуней                       ><>
//►◄►         к задаче поиска оптимального управления и траектории дискретных систем    ◄►◄
//<><                                                                                   ><>
//►◄► ><> ►◄► ><> ►◄► ><> ►◄► ><> ►◄► ><> ►◄►   ◄►◄ <>< ◄►◄ <>< ◄►◄ <>< ◄►◄ <>< ◄►◄ <>< ◄►◄
//
//                ");
            r.Write(
                @"
| <><    <><    <><    <><    <><    <><    <><      ><>    ><>    ><>    ><>    ><>    ><>    ><> |
|                                                                                                  |
| <><                          Протокол применения метода стаи окуней                          ><> |
|              к задаче поиска оптимального управления и траектории дискретных систем              |
| <><                                                                                          ><> |
|__________________________________________________________________________________________________|

1. Постановка задачи

2. Параметры метода стаи окуней

3. Результаты работы

Оптимальное управление u*

Оптимальная траектория x*

                ");
            r.Write(String.Format(@"|{0, 5} |  -----------------------+------------|", 0));
            //r.Write("\r\n");

            r.Close();

            fs.Close();
            Process.Start("protocol.txt");
        }

        /// <summary>Очистка протокола</summary>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("protocol.txt", FileMode.Create, FileAccess.Write);
            fs.Close();
        }

        private void buttonGraphs_Click(object sender, EventArgs e)
        {
            if (graphics == null)
                switch (tabControl2.SelectedIndex)
                {
                    case 0:
                        graphics = new Graphics(1);
                        break;
                    case 1:
                        graphics = new Graphics(3);
                        break;
                    case 2:
                        graphics = new Graphics(1);
                        break;
                    default:
                        break;
                }
            graphics.Show();
        }
    }
}
