using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace N_dimensionalPerchOptimizer
{
    public partial class FormMain : Form
    {

        private AlgorithmPerch algPerch;

        /// <summary>Размерность задачи</summary>
        public int N_dim = 0;

        /// <summary>Ограничение управления</summary>
        public double U = 0;

        private int MaxIteration = 0;
        private Perch resultBest;
        private double[,] obl = new double[2, 2];

        List<Vector> exactPoints = new List<Vector>();

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
            labelX11.Visible = false;
            labelX2.Visible  = false;
            labelX3.Visible  = false;
            labelX22.Visible = false;
            labelX33.Visible = false;
            textBoxX2.Visible = false;
            textBoxX3.Visible = false;

            U = Convert.ToDouble(textBoxU);
            N_dim = Convert.ToInt32(numericUpDownN.Value);

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

        private void comboBoxExample_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxExample.SelectedIndex == 0)
            {
                pictureBoxExample.Image = Properties.Resources.Ex1;
                labelX11.Visible = false;
                labelX2.Visible  = false;
                labelX3.Visible  = false;
                labelX22.Visible = false;
                labelX33.Visible = false;
                textBoxX2.Visible = false;
                textBoxX3.Visible = false;

            }

            if (comboBoxExample.SelectedIndex == 1)
            {
                labelX11.Visible = true;
                labelX2.Visible  = true;
                labelX3.Visible  = true;
                labelX22.Visible = true;
                labelX33.Visible = true;
                textBoxX2.Visible = true;
                textBoxX3.Visible = true;
                //pictureBoxExample.Image = Properties.Resources.Ex2;
            }
            if (comboBoxExample.SelectedIndex == 2)
            {
                pictureBoxExample.Image = Properties.Resources.Ex3;
                labelX11.Visible = false;
                labelX2.Visible = false;
                labelX3.Visible = false;
                labelX22.Visible = false;
                labelX33.Visible = false;
                textBoxX2.Visible = false;
                textBoxX3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

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

        private void textBoxU_TextChanged(object sender, EventArgs e)
        {
            U = Convert.ToDouble(textBoxU);
        }

        private void numericUpDownN_ValueChanged(object sender, EventArgs e)
        {
            N_dim = Convert.ToInt32(numericUpDownN.Value);
        }
    }
}
