using Relax.Model;

namespace Relax.Repository
{
    public class AsyncDelegates
    {
        public delegate RelaxBuildResults AsyncQueueBuild(RelaxBuildDefinition entity);
    }
}