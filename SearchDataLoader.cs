using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace MemSearch
{
    public class SearchDataLoader
    {
        private ASCIIEncoding ASCIIEncoder = new ASCIIEncoding();

        public SearchDataLoader()
        {

        }

        public bool LoadFile(string filename, out string targetProcessName, out List<SearchEntry> retList)
        {
            retList = new List<SearchEntry>();
            targetProcessName = "";
            if(!File.Exists(filename))
            {
                MessageBox.Show("File does not exist!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (FileStream fs = File.OpenRead(filename))
            {
                try
                {
                    byte[] processNameLenBuffer = new byte[sizeof(int)];
                    byte[] numSearchEntriesBuffer = new byte[sizeof(int)];

                    fs.Read(processNameLenBuffer, 0, sizeof(int));
                    fs.Read(numSearchEntriesBuffer, 0, sizeof(int));

                    byte[] processNameBuffer = new byte[BitConverter.ToInt32(processNameLenBuffer)];
                    int numEntries = BitConverter.ToInt32(numSearchEntriesBuffer);

                    fs.Read(processNameBuffer, 0, processNameBuffer.Length);

                    targetProcessName = ASCIIEncoder.GetString(processNameBuffer);
                    for (int i = 0; i < numEntries; i++)
                    {
                        retList.Add(ReadSearchEntry(fs));
                    }
                }
                catch(IOException e)
                {
                    MessageBox.Show("Error while reading file!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error while parsing file data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    fs.Close();
                    return false;
                }
            }

            return true;
        }

        public bool SaveFile(string filename, List<SearchEntry> writelist, string processname)
        {
            using (FileStream fs = File.Create(filename))
            {
                try
                {
                    fs.Write(BitConverter.GetBytes((int)processname.Length));
                    fs.Write(BitConverter.GetBytes((int)writelist.Count));
                    fs.Write(ASCIIEncoder.GetBytes(processname));

                    foreach (SearchEntry entry in writelist)
                        OutputSearchEntry(fs, entry);

                    fs.Flush();
                    fs.Close();
                }
                catch (IOException e)
                {
                    MessageBox.Show("Error while writing file!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error while parsing writtable file data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    fs.Close();
                    if(File.Exists(filename))
                        File.Delete(filename);
                    return false;
                }
            }

            return true;
        }

        private void OutputSearchEntry(FileStream fs, SearchEntry entry)
        {
            byte[] addressData = BitConverter.GetBytes(entry.OriginalAddress);
            byte[] valueData = ASCIIEncoder.GetBytes(entry.Value);
            byte[] valueDataLength = BitConverter.GetBytes((int)valueData.Length);

            fs.WriteByte(entry.Frozen ? (byte)0x01 : (byte)0x00);
            fs.WriteByte((byte)entry.ValueType);
            fs.Write(addressData, 0, addressData.Length);
            fs.Write(valueDataLength, 0, valueDataLength.Length);
            fs.Write(valueData, 0, valueData.Length);
        }

        private SearchEntry ReadSearchEntry(FileStream fs)
        {
            byte frozen = (byte)fs.ReadByte();
            byte type = (byte)fs.ReadByte();
            byte[] addressData = new byte[sizeof(UInt64)];
            byte[] valueDataLength = new byte[sizeof(int)];
            
            fs.Read(addressData, 0, sizeof(UInt64));
            fs.Read(valueDataLength, 0, sizeof(int));

            UInt64 addy = BitConverter.ToUInt64(addressData);
            int valLength = BitConverter.ToInt32(valueDataLength);
            byte[] valueData = new byte[valLength];

            fs.Read(valueData, 0, valLength);

            SearchEntry ret = new SearchEntry();
            ret.OriginalAddress = addy;
            ret.Frozen = frozen == 0x01;
            ret.Value = ASCIIEncoder.GetString(valueData);
            ret.Address = addy.ToString("X8");
            ret.ValueType = (SearchType)type;
            ret.ValueTypeString = SearchTypeConverter.SearchTypeStrings[(int)ret.ValueType];
            return ret;
        }
    }
}
