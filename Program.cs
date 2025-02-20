using Bases;
using Bases.Interfaces;
using Bases.Types;
using System;
using System.Collections.Generic;


namespace $safeprojectname$
{
    internal class Program
    {              
        static void Main(string[] args)
        {
            /* ADD ANY SESSION PROPERTIES */
            IDictionary<string, object> sessionPb = new Dictionary<string, object>();
            /*
             sessionPb.Add("pbFoo", "Foo Bar");             
             sessionPb.Add("pbHello", "Hello World");
             */

            /* ADD ANY SCOPED PROPERTIES */
            IDictionary<string, object> scopedPb = new Dictionary<string, object>();
            /*
             scopedPb.Add("pbFoo", "Foo Bar");             
             scopedPb.Add("pbHello", "Hello World");
             */

            /* ADD ANY PERSISTANT PROPERTIES */
            IDictionary<string, object> persistentPb = new Dictionary<string, object>();
            /*
             persistentPb.Add("pbFoo", "Foo Bar");             
             persistentPb.Add("pbHello", "Hello World");
             */

            /*------ DON NOT MODIFY BEYONG THIS LINE ------*/
            #region Connect To OnBase
            // Use the App.config to set your variables.
            OnBaseConnection conn = OnBaseConnection.Create(
                $safeprojectname$.Properties.Settings.Default.UserName, 
                $safeprojectname$.Properties.Settings.Default.Password, 
                $safeprojectname$.Properties.Settings.Default.AppServer,
                $safeprojectname$.Properties.Settings.Default.DataSource);
            Console.WriteLine($"|------- Attempting to connect with username {$safeprojectname$.Properties.Settings.Default.UserName} to {$safeprojectname$.Properties.Settings.Default.Evironment} with datasource {$safeprojectname$.Properties.Settings.Default.DataSource}");
            conn.Connect();
            #endregion

            #region Initiate New Script
            /* New Script */
            IWorkflowScript script = new $safeprojectname$Script();
            /* Build Script Args */ 
            Hyland.Unity.Document doc = conn.Application.Core.GetDocumentByID($safeprojectname$.Properties.Settings.Default.Item);
            Hyland.Unity.Workflow.LifeCycle lifecycle = conn.Application.Workflow.LifeCycles.Find($safeprojectname$.Properties.Settings.Default.LifeCycle);
            Hyland.Unity.Workflow.Queue queue = conn.Application.Workflow.Queues.Find($safeprojectname$.Properties.Settings.Default.Queue);
            Bases.Types.PropertyBag sessionPropertyBag = new PropertyBag(sessionPb);
            Bases.Types.PropertyBag scopedPropertyBag = new PropertyBag(scopedPb);
            Bases.Types.PropertyBag persistentPropertyBag = new PropertyBag(persistentPb);
            WorkflowEventArgs wfArgs = new WorkflowEventArgs(doc, queue, sessionPropertyBag, scopedPropertyBag, persistentPropertyBag, 1, false);
            #endregion

            #region Execute Script
            try
            {
                script.OnWorkflowScriptExecute(conn.Application, wfArgs);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
            }
            Console.WriteLine("|------- Disconnected");
            Console.ReadLine();
            #endregion
        }
    }
}
