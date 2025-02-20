namespace Bases
{
    using Bases.Interfaces;
    using Bases.Utilities;
    using Hyland.Unity;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    
    /// <summary>
    /// 
    /// </summary>
    public class $safeprojectname$Script : Interfaces.IWorkflowScript
    {
        #region IWorkflowScript
        /// <summary>
        /// Implementation of <see cref="IWorkflowScript.OnWorkflowScriptExecute" />.
        /// <seealso cref="IWorkflowScript" />
        /// </summary>
        /// <param name="app"></param>
        /// <param name="args"></param>
        public void OnWorkflowScriptExecute(Hyland.Unity.Application app, Bases.Types.WorkflowEventArgs args)
        {
            BasesDiagnostics diag = new BasesDiagnostics(app);
        }
        #endregion
    }
}
