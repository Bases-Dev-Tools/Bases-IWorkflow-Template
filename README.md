# Bases IWorkflow Test Harness

The Bases IWorkflow Test Harness is a Visual Studio Project template that lets you quickly create Workflow based Unity Scripts and even allows for testing them through an API connection.

## Using the Template

### Installing the Template

If you have Visual Studio installed, you will have a Visual Studio folder in your Documents  folder. The path could be slightly different based on your version of Visual Studio, but it should look like this:

`C:\Users\{user}\OneDrive\Documents\Visual Studio 2022\Templates\ProjectTemplates`

Clone the repository to this location, or extract the zip file to this location. The path should look like this:

`C:\Users\{user}\OneDrive\Documents\Visual Studio 2022\Templates\ProjectTemplates\Bases-IWorkflow-Test-Harness`

### Creating A New Project

1. Open Visual Studio and choose to create a new project.

   <img src="file:///images/2025-02-08-13-07-47-image.png" title="" alt="" width="420">

2. In the search bar, type "bases" to filter the results.

   ![](C:\Users\jacob\AppData\Roaming\marktext\images\2025-02-08-13-08-48-image.png)

3. Finish setting up your project, and it will open with a Solution Tree that looks like this:

   <img src="file:///images/2025-02-08-13-10-17-image.png" title="" alt="" width="324">

4. You're all ready to go. Your entry point for writing the Unity Script is the "Workflow_Template.cs".

## Connecting the Project to OnBase

In the `App.Config` you'll find the variables for making a connection to OnBase.

> This is only required if you wish to test the script in Visual Studio. If you're just looking to utilize type-ahead, you can skip this section.

### Variables

#### Environment

The environment variable is only used to label output to the console window.

#### Item

Set a document handle that you'd like to test against.

> Currently only Documents are supported, but future released are planned to support WorkView objects and managed folders.

#### AppServer

Provide the Application server address you wish to connect to. Follow the standard convention `http://[server]/AppServer64/service.asmx`

#### DataSource

Provide the data source that you are planning on connecting to.

#### Life Cycle

Provide a Life Cycle that processes the same type of item specified above. The item does not have to currently be in the life cycle.

#### Queue

Provide a queue for the above life cycle. Again, the document does not need to exist in the queue.

#### Username

Provide a valid username.

#### Password

Provide a valid password.

#### Content Type

This variable is set to document and should remain set that way until future release can support other content types.

## Setting Property Bag Key Value Pairs

It is possible to set properties that can be passed into the `args` object of the script. open up the `Program.cs` . You will see the following `Dictionary` objects that represent each property bag with examples on how to add values.

```csharp
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
```

## Using the Diagnostic Utility

Included in the project is a handy Diagnostic utility that simplifies the diagnostic logging interface and is meant to teach better diagnostic logging standards.  The utility will be setup by default:

```csharp
BasesDiagnostics diag = new BasesDiagnostics(app);
```

### Common Logging Standards

Below are the common logging levels considered to be standard. Hyland's logging system uses the same levels, and these levels can and should be implemented when writing scripts in OnBase.

****TRACE:**** Used for debugging and logging a high level of detail of the script execution.

****INFO:**** Significant and noteworthy events in the script.

****WARN:**** Abnormal situations that may indicate future problems.

****ERROR:**** Unrecoverable errors that affect a specific operation.

****CRITICAL:**** Unrecoverable errors that affect the entire application. 

### **Logging Levels in the Utility:**

- PROD is set to `DiagnosticsLevel.Warning` which will only show Warning or Error level messages.

- UAT is set to `DiagnosticsLevel.Info` which will show only diagnostics messages logged at the Info, Warning, or Error level.

- All other non-PROD are automatically set to `DiagnosticsLevel.Verbose` which is ideal for lower environments because it will log everything from the verbose level to the error level.

### Changing the Diagnostic Level

You can override the logging level set for the script in that environment to aid in troubleshooting. This should always be a temporary change and should be removed or commented out once troubleshooting has concluded.

```csharp
diag.diagnosticsLevel = Hyland.Unity.Diagnostics.DiagnosticsLevel.Verbose;
```

### BasesDiagnostics.Trace Level Logging

****Trace level**** is used for debugging and logging a high level of detail of the script execution.

#### BasesDiagnostics.Trace(string message)

Send in a string message to the method to log it in diagnostic console.

```csharp
diag.Trace($"{this.GetType()} Script Starting");
```

> `this.GetType()` will log the class name, which is a safe version of the script name by default.

#### BasesDiagnostics.Trace(Exception ex)

Pass the entire exception message to log the exception message and stack trace in diagnostic console.

```csharp
diag.Trace(ex);
```

#### BasesDiagnostics.Trace(string message, Exception ex)

Pass a custom string message and an exception to log both in diagnostic console.

```csharp
diag.Trace("Script failed to execute.", ex);
```

### Information Level Logging

Information level logging is used for significant and noteworthy events in the script.

#### BasesDiagnostics.Info(string message)

Send in a string message to the method to log it in diagnostic console.

```csharp
diag.Info($"The timer processed {new Random().Next(1, 10)} on this execution.");
```

#### BasesDiagnostics.Info(Exception ex)

Pass the entire exception message to log the exception message and stack trace in diagnostic console.

```csharp
diag.Info(ex);
```

#### BasesDiagnostics.Info(string message, Exception ex)

Pass a custom string message and an exception to log both in diagnostic console.

```csharp
diag.Info("Script failed to execute.", ex);
```

### Warning Level Logging

Warning level logging is used for abnormal situations that may indicate future problems.

### BasesDiagnostics.Warn(string message)

Send in a string message to the method to log it in diagnostic console.

```csharp
diag.Warn($"The document {args.document.id} has been in the queue {queue} for too many days.");
```

#### BasesDiagnostics.Warn(Exception ex)

Pass the entire exception message to log the exception message and stack trace in diagnostic console.

```csharp
diag.Warn(ex);
```

#### BasesDiagnostics.Warn(string message, Exception ex)

Pass a custom string message and an exception to log both in diagnostic console.

```csharp
diag.Warn("Script failed to execute.", ex);
```

## Error Level Logging

Error level logging is used for unrecoverable errors that affect a specific operation.

#### BasesDiagnostics.Error(string message)

Send in a string message to the method to log it in diagnostic console.

```csharp
diag.Error($"Can not find any related documents for the current document, {[args.Document.ID](http://args.document.id/)}");
```

#### BasesDiagnostics.Error(Exception ex)

Pass the entire exception message to log the exception message and stack trace in diagnostic console.

```csharp
diag.Error(ex);
```

#### BasesDiagnostics.Error(string message, Exception ex)

Pass a custom string message and an exception to log both in diagnostic console.

```csharp
diag.Error("Script failed to execute.", ex);
```
