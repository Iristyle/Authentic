using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("EPS.Web.Authentication")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("EPS.Web.Authentication")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3BFA6D41-DA95-448F-A0EB-67CC4CF67673")]

[assembly: InternalsVisibleTo("EPS.Web.Authentication.Tests.Unit")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Target = "EPS.Web.Authentication.Security", Scope = "namespace", Justification = "Most appropriate namespace structure")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Target = "EPS.Web.Authentication.Utility", Scope = "namespace", Justification = "Most appropriate namespace structure")]

//[assembly: SuppressMessage("Gendarme.Rules.Naming", "AvoidDeepNamespaceHierarchyRule", Target = "EPS.Web.Authentication", Scope = "assembly", Justification = "Configuration is a reasonable special name for a fifth level")]
