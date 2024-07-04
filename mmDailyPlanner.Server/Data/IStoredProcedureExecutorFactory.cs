namespace mmDailyPlanner.Server.Data
{
    public interface IStoredProcedureExecutorFactory
    {
        IStoredProcedureExecutor Create();
    }
}