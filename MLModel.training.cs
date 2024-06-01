﻿// This file was auto-generated by ML.NET Model Builder.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;
using System.Data.SqlClient;

namespace Project2
{
    public partial class MLModel
    {
        public const string RetrainConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlightData;Integrated Security=True";
        public const string RetrainCommandString = @"SELECT CAST([Time] as REAL), CAST([Destination] as NVARCHAR(MAX)), CAST([Airline] as NVARCHAR(MAX)), CAST([Status] as REAL) FROM [dbo].[egitimverisi]";

        /// <summary>
        /// Train a new model with the provided dataset.
        /// </summary>
        /// <param name="outputModelPath">File path for saving the model. Should be similar to "C:\YourPath\ModelName.mlnet"</param>
        /// <param name="connectionString">Connection string for databases on-premises or in the cloud.</param>
        /// <param name="commandText">Command string for selecting training data.</param>
        public static void Train(string outputModelPath, string connectionString = RetrainConnectionString, string commandText = RetrainCommandString)
        {
            var mlContext = new MLContext();

            var data = LoadIDataViewFromDatabase(mlContext, connectionString, commandText);
            var model = RetrainModel(mlContext, data);
            SaveModel(mlContext, model, data, outputModelPath);
        }

        /// <summary>
        /// Load an IDataView from a database source.For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext">The common context for all ML.NET operations.</param>
        /// <param name="connectionString">Connection string for databases on-premises or in the cloud.</param>
        /// <param name="commandText">Command string for selecting training data.</param>
        /// <returns>IDataView with loaded training data.</returns>
        public static IDataView LoadIDataViewFromDatabase(MLContext mlContext, string connectionString, string commandText)
        {
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();
            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, commandText);

            return loader.Load(dbSource);
        }
        ///
        /// <summary>
        /// Save a model at the specified path.
        /// </summary>
        /// <param name="mlContext">The common context for all ML.NET operations.</param>
        /// <param name="model">Model to save.</param>
        /// <param name="data">IDataView used to train the model.</param>
        /// <param name="modelSavePath">File path for saving the model. Should be similar to "C:\YourPath\ModelName.mlnet.</param>
        public static void SaveModel(MLContext mlContext, ITransformer model, IDataView data, string modelSavePath)
        {
            // Pull the data schema from the IDataView used for training the model
            DataViewSchema dataViewSchema = data.Schema;

            using (var fs = File.Create(modelSavePath))
            {
                mlContext.Model.Save(model, dataViewSchema, fs);
            }
        }


        /// <summary>
        /// Retrain model using the pipeline generated as part of the training process.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new []{new InputOutputColumnPair(@"Destination", @"Destination"),new InputOutputColumnPair(@"Airline", @"Airline")}, outputKind: OneHotEncodingEstimator.OutputKind.Indicator)      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(@"Time", @"Time"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"Destination",@"Airline",@"Time"}))      
                                    .Append(mlContext.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options(){NumberOfLeaves=3437,NumberOfIterations=985,MinimumExampleCountPerLeaf=20,LearningRate=0.999999776672986,LabelColumnName=@"Status",FeatureColumnName=@"Features",Booster=new GradientBooster.Options(){SubsampleFraction=0.0199488529292958,FeatureFraction=0.878941347195788,L1Regularization=2.97000976522576E-10,L2Regularization=0.318793289359714},MaximumBinCountPerFeature=250}));

            return pipeline;
        }
    }
 }
