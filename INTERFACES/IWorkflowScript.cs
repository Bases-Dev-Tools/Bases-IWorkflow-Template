using Hyland.Unity;

namespace Bases.Interfaces
{
    internal interface IWorkflowScript
    {
        void OnWorkflowScriptExecute(Application app, Bases.Types.WorkflowEventArgs args);
    }
}
