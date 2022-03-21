using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal class AssemblerCompiler
    {
        enum CompilerSegment
        {
            UNKNOWN = 0,
            CODE = 1,
            DATA = 2
        }

        enum DataMode
        {
            DefineByte = 0,
            DefineWord = 1,
            DefineDWord = 2,
            DefineFloat = 3,
            DefineString = 4          
        }

        private UInt64 _pointer = 0;
        private UInt64 _basePointer = 0;
        public UInt64 Pointer { get { return _pointer; } }
        public UInt64 BasePointer { get { return _basePointer; } } 
        public UInt64 Size { get { return _pointer - _basePointer; } }

        private List<OpcodeReturnInfo> _opcodes = new List<OpcodeReturnInfo>();
        public List<OpcodeReturnInfo> Opcodes { get { return _opcodes; } }

        private CompilerSegment _currentSegment = CompilerSegment.UNKNOWN;

        private Dictionary<string, UInt64> _linkerPointers = new Dictionary<string, UInt64>();
        private List<LinkerRequestEntry> _linkerRequestEntries = new List<LinkerRequestEntry>();

        private ASCIIEncoding _stringEncoder = new ASCIIEncoding();

        public AssemblerCompiler(UInt64 baseAddress=0)
        {
            _pointer = _basePointer = baseAddress;
        }

        public bool Compile(string assemblerText, out string errorCode)
        {
            errorCode = "";

            var processedText = ProcessTextStage(assemblerText);

            if (!CompilerStage(processedText, out errorCode))
                return false;

            return LinkerStage(out errorCode);
        }

        /// <summary>
        /// First step - Process text to a suitable form
        /// </summary>
        /// <param name="assemblerText"></param>
        /// <returns></returns>
        private List<string> ProcessTextStage(string assemblerText)
        {
            return assemblerText.Split('\n')
                .Select(x => x.RemoveFromStringIfExistsIgnoreString(";").Trim())
                .ToList();
        }

        /// <summary>
        /// Second stage - compilation
        /// </summary>
        /// <param name="processedText"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private bool CompilerStage(List<string> processedText, out string errorCode)
        {
            bool returnValue = false;
            string partError = "";
            for(int i = 0; i < processedText.Count; i++) 
            {
                string line = processedText[i].ToUpper();
                if (line.Length == 0)
                    continue;

                if (line[0] == '.')
                {
                    if (line == ".CODE")
                        _currentSegment = CompilerSegment.CODE;
                    else if (line == ".DATA")
                        _currentSegment = CompilerSegment.DATA;
                    else
                    {
                        _currentSegment = CompilerSegment.UNKNOWN;
                        errorCode = "[Compiler] Unknown segment: " + line;
                        return false;
                    }
                    continue;
                }

                if(line.EndsWith(":"))
                {
                    string ptrString = line.Substring(0, line.Length - 1).Trim();
                    if(ptrString.Contains(' '))
                    {
                        errorCode = string.Format("[Compiler] Linker names cannot contain spaces '{0}' (line {1})", ptrString, i + 1);
                        return false;
                    }
                    _linkerPointers.Add(ptrString, _pointer);
                    continue;
                }

                if (_currentSegment == CompilerSegment.CODE)
                    returnValue = CompileCodeLine(line, out partError);
                else if(_currentSegment == CompilerSegment.DATA)
                    returnValue = CompileDataLine(processedText[i], out partError); //give it lowercase to support DS
                else
                {
                    errorCode = string.Format("[Compiler] Unknown code '{0}' found at line {1}", line, i + 1);
                    return false;
                }

                if(!returnValue)
                {
                    errorCode = string.Format("[Compiler] Error at line {0} - {1}", i + 1, partError);
                    return false;
                }
            }
            errorCode = "";
            return true;
        }

        private bool CompileDataLine(string line_nonupper, out string partErrorCode)
        {
            string line = line_nonupper.ToUpper();
            string[] values = line.Split(' ');
            List<byte> outByteList = new List<byte>();

            UInt64 pointerBak = _pointer;

            DataMode mode;

            switch(values[0])
            {
                case "DB": //define bytes
                    mode = DataMode.DefineByte;
                    break;
                case "DW": //define words
                    mode = DataMode.DefineWord;
                    break;
                case "DD": //define dword
                    mode = DataMode.DefineDWord;
                    break;
                case "DF": //define float - custom
                    mode = DataMode.DefineFloat;
                    break;
                case "DS": //define string - custom
                    mode = DataMode.DefineString;
                    break;
                default:
                    partErrorCode = "Unknown data segment command: " + values[0];
                    return false;
            }

            if (mode == DataMode.DefineString)
            {
                string remString = line_nonupper.Substring(3);
                if (!remString.StartsWith('"') || !remString.EndsWith('"'))
                {
                    partErrorCode = string.Format("Unknown data string definition '{0}' found", remString);
                    return false;
                }
                remString = remString.Substring(1, remString.Length - 2);

                byte[] bytes = _stringEncoder.GetBytes(remString);
                outByteList.AddRange(bytes);
                _pointer += (ulong)bytes.Length;
            }           
            else
            {
                for (int i = 1; i < values.Length; i++)
                {
                    string bVal = values[i];
                    bool hex = false;
                    if (values[i].StartsWith("0X"))
                    {
                        hex = true;
                        bVal = bVal.Substring(2);
                    }

                    if (mode == DataMode.DefineByte)
                    {
                        byte val = 0;
                        if (hex ? !byte.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out val) : !byte.TryParse(bVal, out val))
                        {
                            partErrorCode = string.Format("Unknown data byte definition '{0}' found", bVal);
                            return false;
                        }

                        outByteList.Add(val);
                        _pointer++;

                    }
                    else if (mode == DataMode.DefineWord)
                    {
                        Int16 val = 0;
                        if (hex ? !Int16.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out val) : !Int16.TryParse(bVal, out val))
                        {
                            partErrorCode = string.Format("Unknown data word definition '{0}' found", bVal);
                            return false;
                        }

                        outByteList.AddRange(BitConverter.GetBytes(val));
                        _pointer += 2;

                    }
                    else if (mode == DataMode.DefineDWord)
                    {
                        Int32 val = 0;
                        if (hex ? !Int32.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out val) : !Int32.TryParse(bVal, out val))
                        {
                            partErrorCode = string.Format("Unknown data double word definition '{0}' found", bVal);
                            return false;
                        }

                        outByteList.AddRange(BitConverter.GetBytes(val));
                        _pointer += 4;
                    }
                    else if (mode == DataMode.DefineFloat)
                    {
                        float val = 0;
                        if (hex ? !float.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out val) : !float.TryParse(bVal, out val))
                        {
                            partErrorCode = string.Format("Unknown data float definition '{0}' found", bVal);
                            return false;
                        }

                        outByteList.AddRange(BitConverter.GetBytes(val));
                        _pointer += 4;
                    }
                }
            }

            _opcodes.Add(new OpcodeReturnInfo()
            {
                Bytes = outByteList,
                Offset = pointerBak,
                Result = line
            });

            partErrorCode = "";
            return true;
        }

        private bool CompileCodeLine(string line, out string partErrorCode)
        {
            string name = line.Split(' ')[0];
            List<string> parameters = line.Replace(name, "").Split(',').Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToList();

            if (CompileFromContainer(line, name, parameters, OpcodeContainers.OpcodeContainer.Opcodes))
            {
                partErrorCode = "";
                return true;
            }

            if (CompileFromContainer(line, name, parameters, OpcodeContainers.ExtendedOpcodeContainer.Opcodes_0F))
            {
                _opcodes[_opcodes.Count - 1].Bytes.Insert(0, 0x0F);
                partErrorCode = "";
                return true;
            }

            if (CompileFromContainer(line, name, parameters, OpcodeContainers.ExtendedOpcodeContainer.Opcodes_F3))
            {
                _opcodes[_opcodes.Count - 1].Bytes.Insert(0, 0xF3);
                partErrorCode = "";
                return true;
            }

            partErrorCode = "Could not find such opcode";
            return false;
        }

        private bool CompileFromContainer(string line, string name, List<string> parameters, List<Opcode> opcodes)
        {            
            var possibleReturns = new List<Tuple<OpcodeReturnInfo, List<LinkerRequestEntry>>>();
           
            foreach (Opcode op in opcodes)
            {
                if (op.SuitableOpcode(name, parameters.Count))
                {
                    List<byte> result = new List<byte>();
                    List<LinkerRequestEntry> linkerRequestEntries = new List<LinkerRequestEntry>();
                    if (op.Compile(name, parameters, ref result, ref linkerRequestEntries))
                    {
                        var returnInfo = new OpcodeReturnInfo()
                        {
                            Bytes = result,
                            Offset = _pointer,
                            Result = line
                        };

                        foreach (var e in linkerRequestEntries)
                            e.CompiledOpcode = returnInfo;

                        possibleReturns.Add(Tuple.Create(returnInfo, linkerRequestEntries));
                    }
                }
            }

            if (possibleReturns.Count == 0)
                return false;

            var bestOption = possibleReturns.OrderBy(f => f.Item1.Bytes.Count).FirstOrDefault();
            if (bestOption == null)
                return false;

            _opcodes.Add(bestOption.Item1);

            _linkerRequestEntries.AddRange(bestOption.Item2);
            _pointer += (ulong)bestOption.Item1.Bytes.Count;

            return true;
        }

        /// <summary>
        /// Last step - Link required code 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private bool LinkerStage(out string errorCode)
        {
            foreach(var lreq in _linkerRequestEntries)
            {
                UInt64 ptr = 0;
                if(!_linkerPointers.TryGetValue(lreq.PointerName, out ptr))
                {
                    errorCode = string.Format("[Linker] Unresolved pointer \"{0}\" at address {1}", lreq.PointerName, lreq.CompiledOpcode.Offset);
                    return false;
                }

                if (lreq.Relative)
                    ptr = ptr - (ulong)lreq.CompiledOpcode.Offset - (ulong)lreq.CompiledOpcode.Bytes.Count;               

                byte[] bytes;
                switch (lreq.Size)
                {
                    case 1:
                        lreq.CompiledOpcode.Bytes[lreq.Offset] = (byte)ptr;
                        lreq.CompiledOpcode.Result = lreq.CompiledOpcode.Result.Replace(lreq.PointerName, $"0x{((byte)ptr).ToString("X")}  ({lreq.PointerName})");
                        break;
                    case 2:
                        bytes = BitConverter.GetBytes((UInt16)ptr);
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 0] = bytes[0];
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 1] = bytes[1];
                        lreq.CompiledOpcode.Result = lreq.CompiledOpcode.Result.Replace(lreq.PointerName, $"0x{((UInt16)ptr).ToString("X")}  ({lreq.PointerName})");
                        break;
                    case 4:
                        bytes = BitConverter.GetBytes((UInt32)ptr);
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 0] = bytes[0];
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 1] = bytes[1];
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 2] = bytes[2];
                        lreq.CompiledOpcode.Bytes[lreq.Offset + 3] = bytes[3];
                        lreq.CompiledOpcode.Result = lreq.CompiledOpcode.Result.Replace(lreq.PointerName, $"0x{((UInt32)ptr).ToString("X")}  ({lreq.PointerName})");
                        break;
                    default:
                        errorCode = string.Format("[Linker] Wrong linker pointer size requested: {0}", lreq.Size);
                        return false;
                }
            }
            errorCode = "";
            return true;
        }
    }
}
