using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyDescription("You can translate both ways between coordinate systems: RT90, SWEREF99 andWGS84.\r\n\r\nThe WGS84 object can parse out positions from strings in DMS (degrees, minutes seconds, and DM (degrees, minutes) format. \r\n\r\nAll in all it’s a pretty easy to use and powerful library.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Björn Sållarp")]
[assembly: AssemblyProduct("MightyLittleGeodesy")]
[assembly: AssemblyCopyright("Copyright © Björn Sållarp 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if PORTABLE45
[assembly: AssemblyTitle("MightyLittleGeodesy Portable .NET 4.5")]
#elif NET40
[assembly: AssemblyTitle("MightyLittleGeodesy .NET 4.0")]
[assembly: AllowPartiallyTrustedCallers]
#elif NET45
[assembly: AssemblyTitle("MightyLittleGeodesy .NET 4.5")]
[assembly: AllowPartiallyTrustedCallers]
#elif NETSTANDARD1_2
[assembly: AssemblyTitle("MightyLittleGeodesy netstandard1.2")]
[assembly: AllowPartiallyTrustedCallers]
#else
[assembly: AssemblyTitle("MightyLittleGeodesy")]
[assembly: AllowPartiallyTrustedCallers]
#endif


#if !(PORTABLE45)
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM componenets.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("41ec09fd-a85a-4309-9caf-1b7dfbc472ea")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
