namespace ok.Service
{
    public class BwmService
    {
        public double[] CalculateWeights(double[] bestToOthers, double[] othersToWorst)
        {
            if (bestToOthers.Length != othersToWorst.Length)
                throw new ArgumentException("Arrays must have same length");

            int n = bestToOthers.Length;
            double[] weights = new double[n];

            double sum = 0;

            for (int i = 0; i < n; i++)
            {
                if (bestToOthers[i] <= 0 || othersToWorst[i] <= 0)
                    throw new ArgumentException("Values must be > 0");

                weights[i] = 1.0 / (bestToOthers[i] + othersToWorst[i]);
                sum += weights[i];
            }

            for (int i = 0; i < n; i++)
            {
                weights[i] /= sum;
            }

            return weights;
        }

        public double CalculateScore(double[] criteriaValues, double[] weights)
        {
            if (criteriaValues.Length != weights.Length)
                throw new ArgumentException("Length mismatch");

            double score = 0;

            for (int i = 0; i < criteriaValues.Length; i++)
            {
                score += criteriaValues[i] * weights[i];
            }

            return score;
        }
    }
}