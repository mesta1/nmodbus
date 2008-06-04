using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Modbus")]
[assembly: AssemblyProduct("NModbus")]
[assembly: AssemblyCopyright("Licensed under MIT License.")]
[assembly: AssemblyDescription("Provides connectivity to Modbus slave compatible devices and applications.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: Guid("a2ec5378-e1b7-4bb0-b696-1c657407eeb9")]
[assembly: AssemblyVersion("1.7.0.0")]
[assembly: AssemblyFileVersion("1.7.0.0")]

#if SIGNED
// no internals visible when we are building release signed
# else
[assembly: InternalsVisibleTo(@"Modbus.UnitTests")]
[assembly: InternalsVisibleTo(@"DynamicProxyGenAssembly2")]
#endif
