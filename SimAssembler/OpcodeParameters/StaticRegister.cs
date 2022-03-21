using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class StaticRegister : OpcodeParameter
    {      
        private RegisterType Type;
        private int Index;

        public string RegisterName { get; private set; }
        public StaticRegister(string registerName)
        {
            this.RegisterName = registerName;

            RegisterInfo inf = RegisterTools.ParseRegister(registerName);
            this.Type = inf.Type;
            this.Index = inf.Index;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            string ret = "";

            if (info.OverrideByte == 0x66)
                ret = RegisterTools.StepRegisterString(Index, Type, -1);
            else
                ret = RegisterName;

            return new OpcodeReturnInfo()
            {
                Offset = info.Offset,
                Bytes = new List<byte>(),
                Result = ret
            };
        }

        public override bool Compile(string parameter, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            return parameter.Equals(RegisterName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
