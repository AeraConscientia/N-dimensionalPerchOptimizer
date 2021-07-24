using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_dimensionalPerchOptimizer
{
    public class Perch
    {
        public Vector coords;
        public double fitness = 0;

        public Perch(int dim)
        {
            coords = new Vector(dim);

#if DEBUG
            for (int i = 0; i < dim; i++)
                coords[i] = 0;
#endif
            fitness = 0;
        }

        //TODO: переписать на н-мерное.
        //public Perch(double x, double y, double fitness)
        //{
        //    coords = new Vector(x, y);
        //    this.fitness = fitness;
        //}
    }
}
