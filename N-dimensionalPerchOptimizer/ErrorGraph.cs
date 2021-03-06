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
    public partial class ErrorGraph : Form
    {
        public ErrorGraph(int TaskNumber, int N_dim)
        {
            InitializeComponent();
            Result result = Result.GetInstance();

            CleanErrorGraphics();

            UpdateErrorGraph(TaskNumber, N_dim);

            
        }

        public void UpdateErrorGraph(int TaskNumber, int N_dim)
        {
            Result result = Result.GetInstance();
            CleanErrorGraphics();

            switch (TaskNumber)
            {
                case 6:
                    double[] AnaliticU = new double[N_dim];
                    for (int i = 0; i < N_dim; i++)
                    {
                        AnaliticU[i] = ((N_dim - i - 1) / (double)N_dim);
                    }

                    for (int i = 0; i < N_dim; i++)
                        chartU_Error.Series[0].Points.AddXY(i, Math.Abs(AnaliticU[i] - result.U[i]));
                    break;
                default:
                    break;
            }
        }

        private void CleanErrorGraphics()
        {
            chartU_Error.Series[0].Points.Clear(); 
        }
            

        private void ErrorGraph_Load(object sender, EventArgs e)
        {

        }
    }
}


