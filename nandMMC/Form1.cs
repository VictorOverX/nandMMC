using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using nandMMC.Properties;
using System.Globalization;
using System.Collections.Generic;

namespace nandMMC
{
    public partial class Form1 : Form
    {
        private static bool _busy;
        private static string _dev, _file;
        private static SafeComboBox _drivelist;
        private static SafeButton _dumpbtn, _updatebtn, _flashbtn;
        private static ulong _size;
        private static SafeToolStripLabel _status;
        private static LoadingCircle _working;
        private bool _incfixed, _abort;

        public Form1()
        {
            InitializeComponent();
            Icon        = Resources.icon;
            Text += string.Format("{0}.{1} (Build: {2})", Assembly.GetExecutingAssembly().GetName().Version.Major, Assembly.GetExecutingAssembly().GetName().Version.Minor, Assembly.GetExecutingAssembly().GetName().Version.Build);
            _status     = status;
            _SizeDrive  = LbSize;
            _dumpbtn    = dumpbtn;
            _updatebtn  = updatebtn;
            _flashbtn   = flashbtn;
            _drivelist  = Devicelist;
            _working    = working;
            _incfixed   = false;
            CheckedChanged(null, null);
            bw.RunWorkerAsync();

        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #region Unmanaged

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            int flags,
            IntPtr template);

        #endregion Unmanaged

        private static bool CheckMagic(ref byte[] data) { return (data[0] == 0xFF && data[1] == 0x4F); }

        private static void Enabledump()
        {
            if (!string.IsNullOrEmpty(_drivelist.Text))
            {
                string sub = _drivelist.Text.Substring(0, 1);

                DriveInfo info  = new DriveInfo(sub);   
                _SizeDrive.Text = GetSizeText(info.TotalSize);

                CultureInfo ci = new CultureInfo("en-us");

                string s = String.Format("0x{0}", info.TotalSize.ToString("x", ci));                
                ulong t = Convert.ToUInt64(s, 16);
                _size = t;
            }


            if (_busy)
                return;
            _dumpbtn.Enabled    = (_size > 0 && !string.IsNullOrEmpty(_drivelist.Text));
            _flashbtn.Enabled   = !string.IsNullOrEmpty(_drivelist.Text);
        }

        private static string GetSizeText(long len) 
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("Tamanho: {0:0.##} {1}", len, sizes[order]);
        }

        private static string GetSizeReadable(ulong i)
        {
            double readable;
            string suffix;
            if (i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = i >> 30;
            }
            else if (i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = i >> 20;
            }
            else if (i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = i >> 10;
            }
            else if (i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            readable = readable / 1024;
            return readable.ToString("0.## ") + suffix;
        }

        private void AbortbtnClick(object sender, EventArgs e) { _abort = true; }

        private void BwDoWork(object sender, DoWorkEventArgs e)
        {
            _busy               = true;
            _status.Text        = "Lista de dispositivos...";
            _working.Active     = true;
            _updatebtn.Enabled  = false;
            _dumpbtn.Enabled    = false;
            _flashbtn.Enabled   = false;
            _drivelist.Clear();
            var drives = DriveInfo.GetDrives();
            
            foreach (var drive in drives)
            {
                switch (drive.DriveType)
                {
                    case DriveType.Removable:
                        _drivelist.AddItem(new ComboBoxItem(string.Format("{0} [Removível]", drive.Name), drive.Name.Substring(0, 1)));
                        break;

                    case DriveType.Fixed:
                        if (_incfixed)
                            _drivelist.AddItem(new ComboBoxItem(string.Format("{0} [Não-removível]", drive.Name), drive.Name.Substring(0, 1)));
                        break;
                }
            }
            _updatebtn.Enabled = true;
            _working.Active = false;
            _status.Text = "Aguardando a seleção do dispositivo...";
            _busy = false;
        }

        private void BwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { Enabledump(); }

        private void CheckedChanged(object sender, EventArgs e)
        {
            foreach (var c in size.Controls)
            {
                if (!(c is RadioButton))
                {
                    continue;
                }
                var rb = c as RadioButton;
                if (!rb.Checked)
                {
                    continue;
                }
             
                Enabledump();
                switch (rb.Name.ToLower())
                {
                    case "sysonly":
                        _size = 0x3000000;
                        break;

                    case "full":
                        _size = 0xe0400000;
                        break;
                }
            }
        }

        private void DevicelistSelectedIndexChanged(object sender, EventArgs e) { Enabledump(); }

        private void DevicelistTextChanged(object sender, EventArgs e) { Enabledump(); }

        private void DumpbtnClick(object sender, EventArgs e)
        {
            if (_busy)
                return;
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            size.Enabled = false;
            _file = sfd.FileName;
            _dev = ((ComboBoxItem)_drivelist.SelectedItem).Value;
            Dumper.RunWorkerAsync();
        }

        private void DumperDoWork(object sender, DoWorkEventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            var handle = GetStarted();
            var fs          = new FileStream(handle, FileAccess.Read);
            ulong offset    = 0;
            try
            {
                using (var binaryWriter = new BinaryWriter(File.Open(_file, FileMode.OpenOrCreate)))
                {
                    while (offset < _size)
                    {
                        if (_abort)
                            break;
                        var buff = new byte[32768];
                        fs.Read(buff, 0, 32768);
                    
                        if (offset == 0)
                        {
                            if (!CheckMagic(ref buff))
                            {
                                _abort = true;
                                MessageBox.Show(Resources.BadMagicErrorDevice, Resources.BadMagicError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                       
                        binaryWriter.Write(buff);
                        offset += 32768;
                        _status.Text = string.Format("Dumping NAND {0} of {1} dumped ({2:F2}%)", GetSizeReadable(offset), GetSizeReadable(_size),
                                                     (double)offset / _size * 100.0);
                    }
                    fs.Close();
                    binaryWriter.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.errormsg + ex, Resources.errortitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _abort = true;
            }
            if (_abort)
                try { File.Delete(_file); }
                catch { }
            _working.Active         = false;
            _dumpbtn.Enabled        = true;
            _updatebtn.Enabled      = true;
            abortbtn.Enabled        = false;
            sw.Stop();
            _status.Text            = _abort ? "Dump Cancelado!" : string.Format("Dumpado com sucesso {0}! levou {1} Minutos {2} Segundos {3} Milliseconds", GetSizeReadable(offset), sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
            _abort = false;
            _busy = false;
        }

        private void DumperRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { size.Enabled = true; }

        private void FlashbtnClick(object sender, EventArgs e)
        {
            if (_busy)
                return;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            size.Enabled = false;
            _file = ofd.FileName;
            _dev = ((ComboBoxItem)_drivelist.SelectedItem).Value;
            Flasher.RunWorkerAsync();
        }

        private void FlasherDoWork(object sender, DoWorkEventArgs e)
        {
            _busy = true;
            var sw = new Stopwatch();
            sw.Start();
            var handle = GetStarted();
            var fs = new FileStream(handle, FileAccess.ReadWrite);
            ulong offset = 0;
            try
            {
                var fi = new FileInfo(_file);
                using (var binaryReader = new BinaryReader(File.Open(fi.FullName, FileMode.OpenOrCreate)))
                {
                    var tsize = (ulong)fi.Length;
                    while (offset < tsize)
                    {
                        if (_abort)
                            break;
                        var buff = binaryReader.ReadBytes(32768);
                        if (offset == 0)
                        {
                            if (!CheckMagic(ref buff))
                            {
                                _abort = true;
                                MessageBox.Show(Resources.BadMagicErrorImage, Resources.BadMagicError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        fs.Write(buff, 0, 32768);
                        offset += 32768;
                        _status.Text = string.Format("Gravando NAND {0} de {1} gravados ({2:F2}%)", GetSizeReadable(offset), GetSizeReadable(tsize),
                                                     (double)offset / tsize * 100.0);
                    }
                    fs.Close();
                    binaryReader.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(Resources.errormsg + ex, Resources.errortitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _abort = true;
            }

            _working.Active = false;
            _dumpbtn.Enabled = true;
            _updatebtn.Enabled = true;
            abortbtn.Enabled = false;
            sw.Stop();
            _status.Text = _abort ? "Flash Aborted!" : string.Format("Successfully wrote {0}! It took {1} Minutes {2} Seconds and {3} Milliseconds", GetSizeReadable(offset), sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
            _busy = false;
        }

        private SafeFileHandle GetStarted()
        {
            _busy               = true;
            _working.Active     = true;
            _dumpbtn.Enabled    = false;
            _updatebtn.Enabled  = false;
            abortbtn.Enabled    = true;
            var handle = CreateFile(string.Format("\\\\.\\{0}:", _dev.ToUpper()), FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (handle.IsInvalid)
                throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());
            return handle;
        }

        private void IncfixedCheckedChanged(object sender, EventArgs e) { _incfixed = incfixed.Checked; }

        private void UpdatebtnClick(object sender, EventArgs e)
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
        }

        public static Label _SizeDrive { get; set; }

        public static byte[] ReverseBytes(int value)
        {
            byte[] b = new byte[4];
            b[0] = (byte)value;
            b[1] = (byte)(((uint)value >> 8) & 0xFF);
            b[2] = (byte)(((uint)value >> 16) & 0xFF);
            b[3] = (byte)(((uint)value >> 24) & 0xFF);
            return b;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ConvertStringToHex(string input, bool reverse = false)
        {
            char[] temp = input.ToCharArray();
            string tempstr = string.Empty;
            foreach (char c in temp)
            {
                int charval = Convert.ToInt32(c);
                tempstr += string.Format("{0:X}", charval);
            }
            if (!reverse)
                return tempstr;
            else
            {
                temp = tempstr.ToCharArray();
                tempstr = string.Empty;
                for (int i = temp.Length - 1; i >= 0; i--)
                {
                    tempstr += temp[i];
                }
                return tempstr;
            }
        }

        public static string ConverIntToHex(int input, bool reverse = false)
        {
            string temp = input.ToString("X");
            if (!reverse)
                return temp;
            else
            {
                char[] holder = temp.ToCharArray();
                temp = string.Empty;
                for (int i = holder.Length - 1; i >= 0; i -= 2)
                {
                    if (holder.Length > 1 && (holder.Length % 2 == 0) && i != 0)
                        temp += holder[i - 1];
                    temp += holder[i];
                }
                return temp;
            }
        }

      

 
        private void btnOpen_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();

            openFile.Filter = "BIN Files (.bin)|*.bin|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.Title = ("Abrir arquivo nor.bin");
            openFile.FileName = ("nor.bin");

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbNor.Text = openFile.FileName;

                try
                {
                    string FileName = openFile.FileName;     
               
                    using (BinaryReader reader = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read))) 
                    {
                        reader.BaseStream.Position = 0;

                        int length      = (int)reader.BaseStream.Length;
                        byte[] myArray  = reader.ReadBytes(length);

                        Array.Reverse(myArray);
                        ByteArrayToFile("dump.bin", myArray);    
                    }
                     
                    /* 
                   

                    if (File.Exists(FileName))
                    {
                        FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);

                    
                        int length = (int)fs.Length;

                        BinaryReader br = new BinaryReader(fs);

                        byte[] myArray = br.ReadBytes(4);

                        Array.Reverse(myArray);

                        
                        ByteArrayToFile("dump.bin", myArray);

                        for (long i = myArray.Length - 1; i > 0; i--)
                        {
                            //Console.Write(Convert.ToChar(myArray[i]));                            
                        }
                        //Console.WriteLine();
                         
                    }
                    else
                    {
                        Console.WriteLine("Arquivo não existe");
                    }
                    

                    /*
                    string FileName = openFile.FileName;
                    if (File.Exists(FileName))
                    {
                        FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        int length = (int)fs.Length;

                        BinaryReader br = new BinaryReader(fs);

                        byte[] myArray = br.ReadBytes((int)fs.Length);
                        byte[] reversedFileContents = Array.Reverse(fileContents);

                        ByteArrayToFile("dump.bin", myArray);

                        for (long i = myArray.Length - 1; i > 0; i--)
                        {
                            //Console.Write(Convert.ToChar(myArray[i]));                            
                        }
                        //Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Arquivo não existe");
                    }
                    */

                    /*
                    BinaryReader read = new BinaryReader(new FileStream(openFile.FileName, FileMode.Open));
                    int length = (int)read.Length;

                    //Posição da Leitura
                    read.BaseStream.Position = 0x840;

                    //Lendo os offset
                    //metldr = BitConverter.ToString(read.ReadBytes(59680)).Replace("-", null); //reader.ReadBytes(12)
                    read.ReadBytes(59680);


                    byte[] myArray = read.ReadBytes((int)read.Length);

                    for (long i = myArray.Length - 1; i > 0; i--)
                    {
                        Console.Write(Convert.ToChar(myArray[i]));
                    }
                    Console.WriteLine();
                     * */

                }
                catch
                {
                    MessageBox.Show("Sorry the application seems to have encountered a problem", "Error");
                }
            }
        }

    

        private bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }






    }
}