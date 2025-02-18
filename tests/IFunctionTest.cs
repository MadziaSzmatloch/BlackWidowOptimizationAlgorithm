using BlackWidowOptimizationAlgorithm.FitnessFunctions;
using BlackWidowOptimizationAlgorithm.OptimalizationAlgorithms.BlackWidow;
using utils;
namespace tests
{
    public class IFunctionTest
    {
        protected string Name { get; set; }
        protected int numberOfGenes { get; set; }
        protected IFitnessFunction fitnessFunction { get; set; }
        protected List<TestCase> testCases { get; set; }


        public bool RunTests()
        {
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"Starting test for {Name} with parameters{testCase}");

                var BWOA = new BlackWidowAlgorithm(testCase.blackWidowAlgorithmParameters, fitnessFunction, testCase.populationSize, numberOfGenes);

                var result = BWOA.Solve();

                Console.WriteLine($"End with result {result}");
                Console.WriteLine($"Parameters:");
                foreach (var parametr in BWOA.XBest)
                {
                    Console.WriteLine(parametr);
                }
                Console.WriteLine($"Number of evaluations {BWOA.NumberOfEvaluationFitnessFunction}");
            }


            return true;
        }

        public string TestsToCsv()
        {
            Console.WriteLine($"Funkcja testowa; Liczba szukanych parametrów; Współczynnik prokreacji; Współczynnik mutacji; Współczynnik kanibalizmu; Maksymalna liczba iteracji; Rozmiar populacji; Znalezione najlepsze minimum; Odchylenie standardowe poszukiwanych parametrów; Najlepsza znaleziona wartość funkcji celu; Odchylenie standardowe funkcji celu");
            string line = "";
            foreach (var testCase in testCases)
            {
                var iterationResults = new List<Tuple<double, double[]>>();
                for (int i = 0; i <= 10; i++)
                {
                    var BWOA = new BlackWidowAlgorithm(testCase.blackWidowAlgorithmParameters, fitnessFunction, testCase.populationSize, numberOfGenes);

                    var result = BWOA.Solve();
                    iterationResults.Add(new Tuple<double, double[]>(BWOA.FBest, BWOA.XBest));
                }
                var sortedIterationResults = iterationResults.OrderBy(x => x.Item1).ToList();
                var bestOfTen = sortedIterationResults.First();

                line += $"{Name}; ";
                line += $"{numberOfGenes}; ";
                line += $"{testCase.blackWidowAlgorithmParameters.ProcreatingRate}; ";
                line += $"{testCase.blackWidowAlgorithmParameters.MutationRate}; ";
                line += $"{testCase.blackWidowAlgorithmParameters.CanibalismRate}; ";
                line += $"{testCase.blackWidowAlgorithmParameters.MaxIterations}; ";
                line += $"{testCase.populationSize}; ";
                line += $"({string.Join(", ", bestOfTen.Item2)}); ";
                var deviantionOfXs = new List<double>();
                for (int i = 0; i < numberOfGenes; i++)
                {
                    var tempXs = new List<double>();
                    foreach (var iteration in sortedIterationResults)
                    {
                        tempXs.Add(iteration.Item2[i]);
                    }
                    deviantionOfXs.Add(MathExtension.StandardDeviation(tempXs));
                }
                line += $"({string.Join(", ", deviantionOfXs)}); ";
                line += $"{bestOfTen.Item1}; ";
                var tempFs = new List<double>();
                foreach (var iteration in sortedIterationResults)
                {
                    tempFs.Add(iteration.Item1);
                }
                line += $"{MathExtension.StandardDeviation(tempFs)}";
            }
            return line;
        }
    }
}