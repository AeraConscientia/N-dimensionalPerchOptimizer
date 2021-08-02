using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_dimensionalPerchOptimizer
{
    public class AlgorithmTask2 : AlgorithmPerch
    {
        public AlgorithmTask2(double U11, double U12, double U21, double U22, double U31, double U32, double x0)
        {
            this.x0 = new List<double>(1) { x0 };   //Список одного элемента
            U = new List<Tuple<double, double>>();
            U.Add(new Tuple<double, double>(U11, U12));
            U.Add(new Tuple<double, double>(U21, U22));
            U.Add(new Tuple<double, double>(U31, U32));
        }
        public override void FormingPopulation()
        {
            for (int i = 0; i < population; i++)
            {
                Perch perch = new Perch(N_dim);
                for (int j = 0; j < N_dim; j++)
                    perch.coords[j] = U[0].Item1 + rand.NextDouble() * (U[0].Item2 - U[0].Item1);

                I(perch);
                individuals.Add(perch);
            }
        }

        public override List<double> Levy()
        {
            List<double> koefLevy = new List<double>(N_dim);
            for (int i = 0; i < N_dim; i++)
            {
                if (i % 2 == 0)
                {
                    R1 = rand.Next(Convert.ToInt32(0), Convert.ToInt32((U[0].Item2 - U[0].Item1) * 100)) / 100f; // (0, b1-a1)
                    thetta1 = R1 * 2 * Math.PI;
                    L1 = Math.Pow(R1 + 0.0001f, -1 / lambda);
                    koefLevy.Add(L1 * Math.Sin(thetta1));
                }
                else
                {
                    R2 = rand.Next(Convert.ToInt32(0), Convert.ToInt32((U[0].Item2 - U[0].Item1) * 100)) / 100f; // (0, b2-a2)
                    thetta2 = R2 * 2 * Math.PI;
                    L2 = Math.Pow(R2 + 0.0001f, -1 / lambda);
                    koefLevy.Add(L2 * Math.Cos(thetta2));
                }
            }
            return koefLevy;

        }

        public override void I(Perch perch, bool flag = false)
        {
            List<double> x = new List<double>();
            x.Add(x0[0]);

            for (int i = 1; i < N_dim; i++)
                x.Add(x[x.Count - 1] + perch.coords[i - 1]);

            double res = 0;
            for (int t = 0; t < N_dim; t++)
                res += (1 / (t + 1)) * perch.coords[t] * perch.coords[t];

            res += 2 * x[x.Count - 1];
            perch.fitness = res;
        }

        public override void MoveEPerchEFlock()
        {
            sigma = rand.NextDouble() * 0.4 + 0.1; // sigma [0.1,  0.5]

            for (int i = 0; i < NumFlocks; i++)
            {
                for (int j = 0; j < NumPerchInFlock; j++)
                {
                    int moveCount = (int)Math.Floor(sigma * NStep);

                    List<Perch> move = new List<Perch>();
                    for (int k = 0; k < moveCount; ++k)
                    {
                        Perch perch = new Perch(N_dim);
                        for (int l = 0; l < N_dim; l++)
                        {
                            double tmp = flock[i, j].coords[l] + k * ((flock[i, 0].coords[l] - flock[i, j].coords[l]) / (NStep));
                            if (tmp < U[0].Item1 || tmp > U[0].Item2)
                                tmp = flock[i, j].coords[l];
                            perch.coords[l] = tmp;
                        }
                        move.Add(perch);
                    }
                    move = move.OrderBy(s => s.fitness).ToList();
                    flock[i, j] = move[0];
                }
                Sort(flock, i);
            }
            SortFlocks();
        }

        protected override void BestFlockSwim()
        {
            sigma = rand.NextDouble() * 0.5 + 1; // sigma [1,  1.5]
            int i = 0;
            for (int j = 0; j < NumPerchInFlock; j++)
            {
                int moveCount = (int)Math.Floor(sigma * NStep);

                List<Perch> move = new List<Perch>();
                for (int k = 0; k < moveCount; ++k)
                {
                    Perch perch = new Perch(N_dim);
                    for (int l = 0; l < N_dim; l++) // если координаты не выйдут за область определения - меняем. Иначе - оставляем.
                    {
                        double tmp = flock[i, j].coords[l] + k * ((flock[i, 0].coords[l] - flock[i, j].coords[l]) / (NStep));
                        if (tmp < U[0].Item1 || tmp > U[0].Item2)
                            tmp = flock[i, j].coords[l];
                        perch.coords[l] = tmp;
                    }
                    I(perch);
                    move.Add(perch);
                }
                //Sort(move);
                move = move.OrderBy(s => s.fitness).ToList();
                flock[i, j] = move[0];
            }
            Sort(flock, i);
        }

        protected override void AverFlockSwim()
        {
            sigma = rand.NextDouble() / 20 + 0.6; // sigma [0.6,  0.8]

            for (int l = 1; l < NumFlocks - 1; l++) // если не изменяет память, передвижение лидеров средних стай
            {
                int moveCount = (int)Math.Floor(sigma * NStep);

                List<Perch> move = new List<Perch>();
                for (int k = 0; k < moveCount; ++k)
                {
                    Perch perch = new Perch(N_dim);
                    for (int m = 0; m < N_dim; m++)
                    {
                        double tmp = flock[l, 0].coords[m] + k * ((flock[0, 0].coords[m] - flock[l, 0].coords[m]) / (NStep));
                        if (tmp < U[0].Item1 || tmp > U[0].Item2)
                            tmp = flock[l, 0].coords[m];
                    }
                    I(perch);
                    move.Add(perch);
                }

                //Sort(move);
                move = move.OrderBy(s => s.fitness).ToList();
                flock[l, 0] = move[0];

                for (int j = 0; j < NumPerchInFlock; j++)
                {
                    int moveCount1 = (int)Math.Floor(sigma * NStep);

                    List<Perch> move1 = new List<Perch>();
                    for (int k = 0; k < moveCount1; ++k)
                    {
                        Perch perch = new Perch(N_dim);
                        for (int p = 0; p < N_dim; p++)
                        {
                            double tmp = flock[l, j].coords[p] + k * ((flock[l, 0].coords[p] - flock[l, j].coords[p]) / (NStep));
                            if (tmp < U[0].Item1 || tmp > U[0].Item2)
                                tmp = flock[l, j].coords[p];
                            perch.coords[p] = tmp;
                        }
                        I(perch);
                        move1.Add(perch);
                    }
                    move1 = move1.OrderBy(s => s.fitness).ToList();
                    flock[l, j] = move1[0];
                }
                Sort(flock, l);
            }
            SortFlocks();
        }

        protected override void PoorFlockSwim()
        {
            PoorLeaderSwim();

            sigma = rand.NextDouble() * 0.4 + 0.1; // sigma [0.1,  0.5]

            List<double> Min = new List<double>(N_dim);

            for (int j = 0; j < N_dim; j++)
                Min.Add(Math.Min((flock[NumFlocks - 1, 0].coords[j] - U[0].Item1), (U[0].Item2 - flock[NumFlocks - 1, 0].coords[j])));

            for (int j = 1; j < NumPerchInFlock; j++)
            {
                Perch perch = new Perch(N_dim);
                for (int k = 0; k < N_dim; k++)
                {
                    double tmp;
                    do
                    {
                        tmp = ((rand.NextDouble()) * 2 - 1) * (flock[NumFlocks - 1, 0].coords[k] - Min[k]);
                    } while (tmp < U[0].Item1 || tmp > U[0].Item2);
                    perch.coords[k] = tmp;
                }

                I(perch);
                flock[NumFlocks - 1, j] = perch;
            }

            int i = 1;

            for (int j = 0; j < NumPerchInFlock; j++) // всех окуней из худших двигаем к лидеру худшей стаи
            {
                int moveCount = (int)Math.Floor(sigma * NStep);

                List<Perch> move = new List<Perch>();
                for (int k = 0; k < moveCount; ++k)
                {
                    Perch perch = new Perch(N_dim);
                    for (int p = 0; p < N_dim; p++)
                    {
                        double tmp = flock[i, j].coords[p] + k * ((flock[i, 0].coords[p] - flock[i, j].coords[p]) / (NStep));
                        if (tmp < U[0].Item1 || tmp > U[0].Item2)
                            tmp = flock[i, j].coords[p];
                        perch.coords[p] = tmp;
                    }
                    I(perch);
                    move.Add(perch);
                }
                //Sort(move);
                move = move.OrderBy(s => s.fitness).ToList();
                flock[i, j] = move[0];
            }
            Sort(flock, i);
        }
    }
}
