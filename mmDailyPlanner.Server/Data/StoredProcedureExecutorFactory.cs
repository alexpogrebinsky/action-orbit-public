using Microsoft.EntityFrameworkCore;

namespace mmDailyPlanner.Server.Data
{
    public class StoredProcedureExecutorFactory: IStoredProcedureExecutorFactory
    {
        private readonly DailyPlannerContext _context;
        private readonly ILogger<StoredProcedureExecutor> _logger;

        public StoredProcedureExecutorFactory(DailyPlannerContext context, ILogger<StoredProcedureExecutor> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IStoredProcedureExecutor Create()
        {
            return new StoredProcedureExecutor(_context, _logger);
        }
    }
}
