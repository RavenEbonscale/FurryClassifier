using System.Reflection;

using ArryStuff;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.ML;


namespace FurryClassifier
{
    public class Prediction
    {

        /// <summary>
        /// Input a name a get back a Predction based on that name
        /// Out Puts a Sexuality
        /// </summary>
        /// <remarks>Work in Progress</remarks>

        /// <param name="Image">Is a Byte Arry of an Image </param>
        /// 
        /// <returns>Sexuality Precicton</returns>
        public static string Sexuality(byte[] Image)
        {
            using (var stream = new MemoryStream(OnnxModels.gender))
            {
                #region Loading The Onnx Model
                var mlcontext = new MLContext();
                var pipeline = mlcontext.Transforms.ResizeImages("image", 128, 128, nameof(ModelInput.Image))
                 .Append(mlcontext.Transforms.ExtractPixels("Images", "image"))
                .Append(mlcontext.Transforms.ApplyOnnxModel("dense_1", "Images", "gender.onnx"));
                
                IDataView data = mlcontext.Data.LoadFromEnumerable(new List<ModelInput>());
                
                Microsoft.ML.Data.TransformerChain<Microsoft.ML.Transforms.Onnx.OnnxTransformer> model = pipeline.Fit(data);
                
                PredictionEngine<ModelInput, ModelOutput> predictionEngine = mlcontext.Model.CreatePredictionEngine<ModelInput, ModelOutput>((ITransformer)stream);
                #endregion

                #region Predction
                MemoryStream ms = new MemoryStream(Image);
                Bitmap bitmap = new Bitmap(ms);
                ms.Dispose();
                ModelInput input = new ModelInput { Image = bitmap };
                ModelOutput prediction = predictionEngine.Predict(input);
                int classification = prediction.Prediction.ArgsMax();
                #endregion

                return Classification.SexualityClassification[classification];
            }
           
        }

    }
}
