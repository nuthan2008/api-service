namespace BusinessProvider.Models.DataSvc
{
    public interface ISoloPractice : IBasePractice
    {
         bool IsSoloPractice { get; }
    }
}