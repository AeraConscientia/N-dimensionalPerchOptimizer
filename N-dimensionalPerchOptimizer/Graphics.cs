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
        public Graphics()
        {
            InitializeComponent();
            Result result = Result.GetInstance();

            chartX_1.Series[0].Points.Clear();
            chartU_1.Series[0].Points.Clear();

            for (int i = 0; i < result.X.Count; i++)
                chartX_1.Series[0].Points.AddXY(i, result.X[i]);

            for (int i = 0; i < result.U.Count; i++)
                chartU_1.Series[0].Points.AddXY(i, result.U[i]);
        }

        public void UpdateGraph()
        {
            //InitializeComponent();
            chartX_1.Series[0].Points.Clear();
            chartU_1.Series[0].Points.Clear();
            Result result = Result.GetInstance();
            for (int i = 0; i < result.X.Count; i++)
                chartX_1.Series[0].Points.AddXY(i, result.X[i]);

            for (int i = 0; i < result.U.Count; i++)
                chartU_1.Series[0].Points.AddXY(i, result.U[i]);
        }
    }
}
