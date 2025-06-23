using System.Reflection;
using System.Resources;

namespace ResourceReader
{
    public class Messenger
    {
        public static string GetHello()
        {
            string msg = "";

            Assembly asm = Assembly.GetCallingAssembly();
            string rsFileName = asm.GetName().Name + ".Resources.rs1";
            ResourceManager rm = new ResourceManager(rsFileName, asm);
            var msg2 = rm.GetString("Hello");
            msg = msg2;
            return msg;
        }
    }
}
