using DriversTestEvaluation.Core.Models;

public interface IVisionAnalyzer
{
    Task<VisionResult> AnalyzeAsync(byte[] screenshot);
}