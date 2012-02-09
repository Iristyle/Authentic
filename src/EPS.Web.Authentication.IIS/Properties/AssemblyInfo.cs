using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using EPS.Web.Authentication;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("EPS.Web.Authentication.IIS")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("EPS.Web.Authentication.IIS")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("88ce6aeb-f46e-4a9e-93ec-afa0a9235844")]


[assembly: PreApplicationStartMethod(typeof(HttpAuthenticationModule), "RegisterModule")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
