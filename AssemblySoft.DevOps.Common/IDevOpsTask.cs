namespace AssemblySoft.DevOps
{
    public interface IDevOpsTask
    {
        int Order { get; }
        void Run(ref DevOpsTaskStatus status);
    }
}