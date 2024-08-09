using CommentScraping.Model;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentScraping
{
    public class EmotionalAnalyse
    {
        public List<SentimentPrediction> Analyze(List<Comment> comment) 
        {
            var context = new MLContext();

            var dataPath = "C:\\Users\\asame\\Desktop\\githubPersonal\\CommentScraping\\CommentScraping\\analyse.csv";
            var data = context.Data.LoadFromTextFile<Comment>(dataPath, separatorChar: '~', hasHeader: true);
            var pipeline = context.Transforms.Text.FeaturizeText("Features", "CommentText")
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features"));
            var trainTestData = context.Data.TrainTestSplit(data, testFraction: 0.2);
            var model = pipeline.Fit(trainTestData.TrainSet);
            var metrics = context.BinaryClassification.Evaluate(model.Transform(trainTestData.TestSet), "Label");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");
            Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
            var testDataView = context.Data.LoadFromEnumerable(comment);
            var predictions = model.Transform(testDataView);
            var results = context.Data.CreateEnumerable<SentimentPrediction>(predictions, reuseRowObject: false);
            return results.ToList();
        }
    }
}
