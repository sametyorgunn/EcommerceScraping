using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentScraping.Model
{
    public class SentimentPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }

        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public string CommentText { get; set; }
    }
}
