using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace N_dimensionalPerchOptimizer
{
    public partial class FormMain : Form
    {
        /// <summary>Размерность задачи</summary>
        public int N_dim = 0;

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

            N_dim = Convert.ToInt32(numericUpDownN1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadParams();
            AlgorithmPerch algPerch;

            object[] X;
            object[] U;

            switch (tabControl2.SelectedIndex) 
            {
                case 0:
                    double U1 = Convert.ToDouble(textBoxU1.Text);
                    double U2 = Convert.ToDouble(textBoxU2.Text);
                    double x0 = Convert.ToDouble(textBoxX1.Text);
                    algPerch = new AlgorithmTask1(U1, U2, x0);

                    break;
                //case 1:
                //    double U11 = Convert.ToDouble(textBoxU11.Text);
                //    double U12 = Convert.ToDouble(textBoxU12.Text);
                //    double U21 = Convert.ToDouble(textBoxU21.Text);
                //    double U22 = Convert.ToDouble(textBoxU22.Text);
                //    double U31 = Convert.ToDouble(textBoxU31.Text);
                //    double U32 = Convert.ToDouble(textBoxU32.Text);
                //    double x00 = Convert.ToDouble(textBoxX11.Text);
                //    double x11 = Convert.ToDouble(textBoxX22.Text);
                //    double x22 = Convert.ToDouble(textBoxX33.Text);
                //    //algPerch = new AlgorithmTask1(U11, U12, U21, U22, U31, U32, x00, x11, x22);
                //    break;
                default:
                    return;
            }
            resultBest = algPerch.StartAlg(MaxIteration, NumFlocks, NumPerchInFlock, NStep, lambda, alfa, PRmax, deltapr, N_dim);

            Result result = Result.GetInstance();
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    X = new object[N_dim];
                    U = new object[N_dim];
                    for (int i = 0; i < N_dim; i++)
                    {
                        X[i] = result.X[i];
                        U[i] = result.U[i];
                    }
                    break;
                //case 1:
                //    double U11 = Convert.ToDouble(textBoxU11.Text);
                //    double U12 = Convert.ToDouble(textBoxU12.Text);
                //    double U21 = Convert.ToDouble(textBoxU21.Text);
                //    double U22 = Convert.ToDouble(textBoxU22.Text);
                //    double U31 = Convert.ToDouble(textBoxU31.Text);
                //    double U32 = Convert.ToDouble(textBoxU32.Text);
                //    double x00 = Convert.ToDouble(textBoxX11.Text);
                //    double x11 = Convert.ToDouble(textBoxX22.Text);
                //    double x22 = Convert.ToDouble(textBoxX33.Text);
                //    //algPerch = new AlgorithmTask1(U11, U12, U21, U22, U31, U32, x00, x11, x22);
                //    break;
                default:
                    return;
            }
            dataGridViewXans.RowCount = 1;
            dataGridViewXans.ColumnCount = N_dim + 1;
            dataGridViewXans.Rows[0].SetValues(X);

            dataGridViewUans.RowCount = 1;
            dataGridViewUans.ColumnCount = N_dim;
            dataGridViewUans.Rows[0].SetValues(U);

            labelMinI.Text = result.fitness.ToString();
        }

        /// <summary>Запись протокола и его вызов</summary>
        private void buttonProtocol_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("protocol.txt", FileMode.Append, FileAccess.Write);
            StreamWriter r = new StreamWriter(fs);
            r.Write(String.Format(@"|{0, 5} |  -----------------------+------------|", 0));
            r.Write("\r\n");

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
    }
}
