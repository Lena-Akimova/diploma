using System;

namespace алглиб2
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. Лин прогр-е
            // This example demonstrates how to minimize
            //
            //     F(x0,x1) = -0.1*x0 - x1
            //
            // subject to box constraints
            //
            //     -1 <= x0,x1 <= +1 
            //
            // and general linear constraints
            //
            //     x0 - x1 >= -1
            //     x0 + x1 <=  1
            //
            // We use dual simplex solver provided by ALGLIB for this task. Box
            // constraints are specified by means of constraint vectors bndl and
            // bndu (we have bndl<=x<=bndu). General linear constraints are
            // specified as AL<=A*x<=AU, with AL/AU being 2x1 vectors and A being
            // 2x2 matrix.
            //
            // NOTE: some/all components of AL/AU can be +-INF, same applies to
            //       bndl/bndu. You can also have AL[I]=AU[i] (as well as
            //       BndL[i]=BndU[i]).
            //
            double[,] a = new double[,] { { 1, -1 }, { 1, +1 } };
            double[] al = new double[] { -1, -System.Double.PositiveInfinity };
            double[] au = new double[] { System.Double.PositiveInfinity, +1 };
            double[] c = new double[] { -0.1, -1 };
            double[] s = new double[] { 1, 1 };
            double[] bndl = new double[] { -1, -1 };
            double[] bndu = new double[] { +1, +1 };
            double[] x;
            alglib.minlpstate state;
            alglib.minlpreport rep;
            
            alglib.minlpcreate(2, out state);

            //
            // Set cost vector, box constraints, general linear constraints.
            //
            // Box constraints can be set in one call to minlpsetbc() or minlpsetbcall()
            // (latter sets same constraints for all variables and accepts two scalars
            // instead of two vectors).
            //
            // General linear constraints can be specified in several ways:
            // * minlpsetlc2dense() - accepts dense 2D array as input; sometimes this
            //   approach is more convenient, although less memory-efficient.
            // * minlpsetlc2() - accepts sparse matrix as input
            // * minlpaddlc2dense() - appends one row to the current set of constraints;
            //   row being appended is specified as dense vector
            // * minlpaddlc2() - appends one row to the current set of constraints;
            //   row being appended is specified as sparse set of elements
            // Independently from specific function being used, LP solver uses sparse
            // storage format for internal representation of constraints.
            //
            alglib.minlpsetcost(state, c);
            alglib.minlpsetbc(state, bndl, bndu);
            alglib.minlpsetlc2dense(state, a, al, au, 2);
            //
            // Set scale of the parameters.
            //
            // It is strongly recommended that you set scale of your variables.
            // Knowing their scales is essential for evaluation of stopping criteria
            // and for preconditioning of the algorithm steps.
            // You can find more information on scaling at http://www.alglib.net/optimization/scaling.php
            //
            alglib.minlpsetscale(state, s);

            // Solve
            alglib.minlpsetalgodss(state, 0.0000001);
            alglib.minlpoptimize(state);
            alglib.minlpresults(state, out x, out rep);
            System.Console.WriteLine("1.{0}", alglib.ap.format(x, 3)); // EXPECTED: [0,1]
            //alglib.trace_file("DUALSIMPLEX.DETAILED,PREC.F6", "C:/Users/Пользователь/source/repos/алглиб2/алглиб2/trace.log");

            //2.
            double[,] aa = new double[,] { { 1,1,1,2}, {0,1,1,1}, {0,0,1,-1 } };
            double[] all = new double[] { 7,5,3 };
            double[] auu = new double[] { 7,5,3};
            double[] cc = new double[] { -1,1,-1,-2 };
            double[] ss = new double[] { 1,1,1,1 };
            double[] bndll = new double[] { 0,0,0,0 };
            double[] bnduu = new double[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity };
            double[] xx;
            alglib.minlpstate statee;
            alglib.minlpreport repp;
            //alglib.trace_file("DUALSIMPLEX.DETAILED,PREC.F6", "C:/Users/Пользователь/source/repos/алглиб2/алглиб2/trace1.log");

            alglib.minlpcreate(4, out statee);
            alglib.minlpsetcost(statee, cc);
            alglib.minlpsetbc(statee, bndll, bnduu);
            alglib.minlpsetlc2dense(statee, aa, all, auu, 2);
            alglib.minlpsetscale(statee, ss);
           

            alglib.minlpoptimize(statee);
            alglib.minlpresults(statee, out xx, out repp);
            System.Console.WriteLine("2.{0}", alglib.ap.format(xx, 3));
            //alglib.trace_file("DUALSIMPLEX.DETAILED,PREC.F6", "C:/Users/Пользователь/source/repos/алглиб2/алглиб2/trace1.log");





            //Квадратичн прогр-е
            //
            // This example demonstrates minimization of F(x0,x1) = x0^2 + x1^2 -6*x0 - 4*x1
            // subject to bound constraints 0<=x0<=2.5, 0<=x1<=2.5
            //
            // Exact solution is [x0,x1] = [2.5,2]
            //
            // We provide algorithm with starting point. With such small problem good starting
            // point is not really necessary, but with high-dimensional problem it can save us
            // a lot of time.
            //
            // Several QP solvers are tried: QuickQP, BLEIC, DENSE-AUL.
            //
            // IMPORTANT: this solver minimizes  following  function:
            //     f(x) = 0.5*x'*A*x + b'*x.
            // Note that quadratic term has 0.5 before it. So if you want to minimize
            // quadratic function, you should rewrite it in such way that quadratic term
            // is multiplied by 0.5 too.
            // For example, our function is f(x)=x0^2+x1^2+..., but we rewrite it as 
            //     f(x) = 0.5*(2*x0^2+2*x1^2) + ....
            // and pass diag(2,2) as quadratic term - NOT diag(1,1)!
            //
            double[,] aaa = new double[,] { { 2, 0 }, { 0, 2 } };
            double[] bbb = new double[] { -6, -4 };
            //double[] x0 = new double[] { 0, 1 };
            double[] sss = new double[] { 1, 1 };
            double[] bndlll = new double[] { 0.0, 0.0 };
            double[] bnduuu = new double[] { 2.5, 2.5 };
            double[] xxx;
            alglib.minqpstate stateee;
            alglib.minqpreport reppp;

            // create solver, set quadratic/linear terms
            alglib.minqpcreate(2, out stateee);
            alglib.minqpsetquadraticterm(stateee, aaa);
            alglib.minqpsetlinearterm(stateee, bbb);
           // alglib.minqpsetstartingpoint(stateee, x0);
            alglib.minqpsetbc(stateee, bndlll, bnduuu);

            // Set scale of the parameters.
            // It is strongly recommended that you set scale of your variables.
            // Knowing their scales is essential for evaluation of stopping criteria
            // and for preconditioning of the algorithm steps.
            // You can find more information on scaling at http://www.alglib.net/optimization/scaling.php
            //
            // NOTE: for convex problems you may try using minqpsetscaleautodiag()
            //       which automatically determines variable scales.
            alglib.minqpsetscale(stateee, sss);

            //
            // Solve problem with QuickQP solver.
            //
            // This solver is intended for medium and large-scale problems with box
            // constraints (general linear constraints are not supported).
            //
            // Default stopping criteria are used, Newton phase is active.
            //
            alglib.minqpsetalgoquickqp(stateee, 0.0, 0.0, 0.0, 0, true);
            alglib.minqpoptimize(stateee);
            alglib.minqpresults(stateee, out xxx, out reppp);
            System.Console.WriteLine("{0} QuickQP", reppp.terminationtype); // EXPECTED: 4
            System.Console.WriteLine("{0} QuickQP", alglib.ap.format(xxx, 2)); // EXPECTED: [2.5,2]

            //
            // Solve problem with BLEIC-based QP solver.
            //
            // This solver is intended for problems with moderate (up to 50) number
            // of general linear constraints and unlimited number of box constraints.
            //
            // Default stopping criteria are used.
            //
            alglib.minqpsetalgobleic(stateee, 0.0, 0.0, 0.0, 0);
            alglib.minqpoptimize(stateee);
            alglib.minqpresults(stateee, out xxx, out reppp);
            System.Console.WriteLine("{0} BLEIC", alglib.ap.format(xxx, 2)); // EXPECTED: [2.5,2]

            //
            // Solve problem with DENSE-AUL solver.
            //
            // This solver is optimized for problems with up to several thousands of
            // variables and large amount of general linear constraints. Problems with
            // less than 50 general linear constraints can be efficiently solved with
            // BLEIC, problems with box-only constraints can be solved with QuickQP.
            // However, DENSE-AUL will work in any (including unconstrained) case.
            //
            // Default stopping criteria are used.
            //
            alglib.minqpsetalgodenseaul(stateee, 1.0e-9, 1.0e+4, 5);
            alglib.minqpoptimize(stateee);
            alglib.minqpresults(stateee, out xxx, out reppp);
            System.Console.WriteLine("{0} DENSE-AUL", alglib.ap.format(xxx, 2)); // EXPECTED: [2.5,2]
            System.Console.ReadLine();

        }
    }
}
