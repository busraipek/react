using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.ML;
using System.IO;


namespace Project2.Services
{
    public class FlightPrediction
    {
        public float Time { get; set; }
        public string Airline { get; set; }
        public string Destination { get; set; }
    }

    public class FlightPredictionResult
    {
        public float Delay { get; set; }
    }

    public interface IMLModelService
    {
        FlightPredictionResult Predict(FlightPrediction input);
    }

    public class MLModelService : IMLModelService
    {
        private readonly PredictionEngine<FlightPrediction, FlightPredictionResult> _predictionEngine;

        public MLModelService()
        {
            var mlContext = new MLContext();
            var modelPath = Path.Combine(Environment.CurrentDirectory, "MLModel.mlnet");

            // Modeli yükleme
            DataViewSchema modelInputSchema;
            ITransformer mlModel = mlContext.Model.Load(modelPath, out modelInputSchema);

            // PredictionEngine yaratma
            _predictionEngine = mlContext.Model.CreatePredictionEngine<FlightPrediction, FlightPredictionResult>(mlModel);
        }

        public FlightPredictionResult Predict(FlightPrediction input)
        {
            return _predictionEngine.Predict(input);
        }
    }
}