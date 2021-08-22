using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N_dimensionalPerchOptimizer
{
    public partial class Graphics : Form
    {
        public Graphics(int dim)
        {
            InitializeComponent();
            Result result = Result.GetInstance();

            switch (dim)
            {
                case 1: // случай, когда рисуются только два графика (1,3 примеры)
                    chartX_1.Series[0].Points.Clear();
                    chartU_1.Series[0].Points.Clear();

                    for (int i = 0; i < result.X.Count; i++)
                        chartX_1.Series[0].Points.AddXY(i, result.X[i]);

                    for (int i = 0; i < result.U.Count; i++)
                        chartU_1.Series[0].Points.AddXY(i, result.U[i]);
                    break;
                case 3: // случай, когда рисуются шесть графиков (2 пример)
                    chartX_1.Series[0].Points.Clear(); chartX_2.Series[0].Points.Clear(); chartX_3.Series[0].Points.Clear();
                    chartU_1.Series[0].Points.Clear(); chartU_2.Series[0].Points.Clear(); chartU_3.Series[0].Points.Clear();

                    for (int i = 0; i < result.X.Count; i++)
                    {
                        chartX_1.Series[0].Points.AddXY(i, result.X[i]);
                        chartX_2.Series[0].Points.AddXY(i, result.X2[i]);
                        chartX_3.Series[0].Points.AddXY(i, result.X3[i]);
                    }

                    int N_dim = result.U.Count;
                    List<double> U_2 = new List<double>(N_dim / 3);
                    List<double> U_3 = new List<double>(N_dim / 3);

                    for (int i = 0; i < N_dim / 3; i++)
                    {
                        U_2.Add(result.U[i + N_dim/3]);
                        U_3.Add(result.U[i + 2 * N_dim / 3]);
                    }

                    for (int i = 0; i < result.U.Count / 3; i++)
                    {
                        chartU_1.Series[0].Points.AddXY(i, result.U[i]);
                        chartU_2.Series[0].Points.AddXY(i, U_2[i]);
                        chartU_3.Series[0].Points.AddXY(i, U_3[i]);
                    }
                        
                    break;
                default:
                    break;
            }

            
        }

        public void UpdateGraph(int dim)
        {
            //InitializeComponent();
            Result result = Result.GetInstance();
            switch (dim)
            {
                case 1:
                    chartX_1.Series[0].Points.Clear();
                    chartU_1.Series[0].Points.Clear();

                    for (int i = 0; i < result.X.Count; i++)
                        chartX_1.Series[0].Points.AddXY(i, result.X[i]);

                    for (int i = 0; i < result.U.Count; i++)
                        chartU_1.Series[0].Points.AddXY(i, result.U[i]);
                    break;
                case 3:
                    chartX_1.Series[0].Points.Clear(); chartX_2.Series[0].Points.Clear(); chartX_3.Series[0].Points.Clear();
                    chartU_1.Series[0].Points.Clear(); chartU_2.Series[0].Points.Clear(); chartU_3.Series[0].Points.Clear();

                    for (int i = 0; i < result.X.Count; i++)
                    {
                        chartX_1.Series[0].Points.AddXY(i, result.X[i]);
                        chartX_2.Series[0].Points.AddXY(i, result.X2[i]);
                        chartX_3.Series[0].Points.AddXY(i, result.X3[i]);
                    }

                    int N_dim = result.U.Count;
                    List<double> U_2 = new List<double>(N_dim / 3);
                    List<double> U_3 = new List<double>(N_dim / 3);

                    for (int i = 0; i < N_dim / 3; i++)
                    {
                        U_2.Add(result.U[i + N_dim / 3]);
                        U_3.Add(result.U[i + 2 * N_dim / 3]);
                    }

                    for (int i = 0; i < U_2.Count; i++)
                    {
                        chartU_1.Series[0].Points.AddXY(i, result.U[i]);
                        chartU_2.Series[0].Points.AddXY(i, U_2[i]);
                        chartU_3.Series[0].Points.AddXY(i, U_3[i]);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
