using System;
using Google.OrTools.LinearSolver;

namespace ok.Service
{
    public class BwmService
    {
        public double[] CalculateWeights(double[] bestToOthers, double[] othersToWorst, int bestIndex, int worstIndex)
        {
            int n = bestToOthers.Length;

            Solver solver = Solver.CreateSolver("GLOP");
            if (solver == null)
            {
                throw new Exception("Не вдалося створити LP-солвер.");
            }

            Variable[] w = solver.MakeNumVarArray(n, 0.0, 1.0, "w");
            Variable xi = solver.MakeNumVar(0.0, double.PositiveInfinity, "xi");

            Objective objective = solver.Objective();
            objective.SetCoefficient(xi, 1);
            objective.SetMinimization();

            Constraint sumConstraint = solver.MakeConstraint(1.0, 1.0);
            for (int i = 0; i < n; i++)
            {
                sumConstraint.SetCoefficient(w[i], 1);
            }

            for (int j = 0; j < n; j++)
            {
                double a_Bj = bestToOthers[j];
                double a_jW = othersToWorst[j];

                Constraint c1 = solver.MakeConstraint(double.NegativeInfinity, 0.0);
                c1.SetCoefficient(w[bestIndex], 1);
                c1.SetCoefficient(w[j], -a_Bj);
                c1.SetCoefficient(xi, -1);

                Constraint c2 = solver.MakeConstraint(double.NegativeInfinity, 0.0);
                c2.SetCoefficient(w[bestIndex], -1);
                c2.SetCoefficient(w[j], a_Bj);
                c2.SetCoefficient(xi, -1);

                Constraint c3 = solver.MakeConstraint(double.NegativeInfinity, 0.0);
                c3.SetCoefficient(w[j], 1);
                c3.SetCoefficient(w[worstIndex], -a_jW);
                c3.SetCoefficient(xi, -1);

                Constraint c4 = solver.MakeConstraint(double.NegativeInfinity, 0.0);
                c4.SetCoefficient(w[j], -1);
                c4.SetCoefficient(w[worstIndex], a_jW);
                c4.SetCoefficient(xi, -1);
            }

            Solver.ResultStatus resultStatus = solver.Solve();

            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                throw new Exception("Неможливо знайти оптимальні ваги для заданих умов.");
            }

            double[] weights = new double[n];
            for (int i = 0; i < n; i++)
            {
                weights[i] = w[i].SolutionValue();
            }

            return weights;
        }

        public double CalculateScore(double[] criteriaValues, double[] weights)
        {
            if (criteriaValues.Length != weights.Length)
                throw new ArgumentException("Кількість критеріїв та їхніх ваг має збігатися.");

            double score = 0;
            for (int i = 0; i < criteriaValues.Length; i++)
            {
                score += criteriaValues[i] * weights[i];
            }
            return score;
        }
    }
}