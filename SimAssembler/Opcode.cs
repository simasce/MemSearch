using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SimAssembler.OpcodeParameters;

namespace SimAssembler
{
    internal class Opcode
    {
        public byte Opbyte { get; set; }
        public string Name { get; set; }
        public List<OpcodeParameter> Parameters { get; set; }

        public virtual string GetName(MemoryStream ms)
        {
            return Name;
        }

        public virtual List<OpcodeParameter> GetParameters(MemoryStream ms)
        {
            return Parameters;
        }

        public virtual bool SuitableOpcode(string name, int numParameters)
        {
            bool parametersGood = numParameters == Parameters.Count;
            if (Parameters.Count > 0 && Parameters[0].GetType() == typeof(Register))
                parametersGood = (parametersGood || (numParameters == Parameters.Count + 1));       

            return Name.Equals(name, StringComparison.OrdinalIgnoreCase) && parametersGood;
        }

        public virtual bool Compile(string code, List<string> parameters, ref List<byte> result, ref List<LinkerRequestEntry> linkerRequests)
        {
            if (!SuitableOpcode(code, parameters.Count))
                return false;

            List<byte> compiledBytes = new List<byte>() { Opbyte };
            List<byte> extraStart = new List<byte>();
            List<LinkerRequestEntry> linkers = new List<LinkerRequestEntry>();

            for(int i = 0; i < Parameters.Count; i++)
            {
                if(Parameters[i].GetType() == typeof(Register))
                {
                    if (!Parameters[i].Compile(string.Join(',', parameters), ref compiledBytes, ref extraStart, ref linkers))
                        return false;
                }
                else
                {
                    if (!Parameters[i].Compile(parameters[i], ref compiledBytes, ref extraStart, ref linkers))
                        return false;
                }              
            }

            extraStart.AddRange(compiledBytes);
            FinalizeCompilation(code, parameters.Count, ref extraStart);

            result = extraStart;
            linkerRequests = linkers;
            return true;
        }

        protected virtual void FinalizeCompilation(string name, int numParameters, ref List<byte> compiledBytes)
        { 
        }
    }
}
