using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.ML;
using System.IO;
using Project2.Models;


namespace Project2.Services
{
    public class MLModelService
    {
        private static readonly string MLNetModelPath = Path.GetFullPath("MLModel.mlnet");
        private static readonly Lazy<PredictionEngine<MLModel.ModelInput, MLModel.ModelOutput>> PredictEngine =
            new Lazy<PredictionEngine<MLModel.ModelInput, MLModel.ModelOutput>>(() => CreatePredictEngine(), true);

        private static PredictionEngine<MLModel.ModelInput, MLModel.ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<MLModel.ModelInput, MLModel.ModelOutput>(mlModel);
        }

        public PredictionOutput Predict(PredictionInput input)
        {
            var predictionInput = new MLModel.ModelInput
            {
                Time = input.Time,
                Airline = input.Airline,
                Destination = input.Destination
            };

            var predEngine = PredictEngine.Value;
            var predictionResult = predEngine.Predict(predictionInput);

            return new PredictionOutput
            {
                PredictedStatus = predictionResult.Score // Ensure you use the correct property here
            };
        }
    }
}