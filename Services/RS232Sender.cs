using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class RS232Sender : IDisposable, INotifyPropertyChanged
    {
        private readonly SerialPort _serialPort;

        public RS232Sender(string portName, int boundRate, Parity parity, bool openPort = true)
        {
            _serialPort = new SerialPort(portName, boundRate, parity);
            _serialPort.Close();
        }

        public RS232Sender(string portName, int boundRate, Parity parity, Encoding encoding, int readTimeout,
            bool openPort = true)
        {
            _serialPort = new SerialPort(portName, boundRate, parity);
            _serialPort.Encoding = encoding;
            _serialPort.ReadTimeout = readTimeout;
            _serialPort.Close();


            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = true;
            _serialPort.DtrEnable = true;
            _serialPort.ReceivedBytesThreshold = 1024;
            // _serialPort.ReadTimeout = 1000;
            // _serialPort.WriteTimeout = 1000;

            Thread.Sleep(50);
            if (openPort)
            {
                try
                {
                    _serialPort.Open();
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            _serialPort.Close();
        }

        public void Connect()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    Thread.Sleep(100);
                }
            }
            catch
            {
                _serialPort.Close();
            }
        }

        public Task<string> SendMessageAsync(string message, int timeout = 1000)
        {
            return Task.Run(() => SendMessage(message));
        }

        public string SendMessage(string message, int timeout = 1000)
        {
            lock (_serialPort)
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();

                if (_serialPort.BaseStream != null)
                    if (_serialPort.BaseStream.CanRead)
                    {
                        _serialPort.ReadExisting();
                    }

                Thread.Sleep(10);
                _serialPort.ReadTimeout = timeout;
                _serialPort.WriteTimeout = 1000;
                _serialPort.Write(message);
                try
                {
                    Thread.Sleep(80);
                    string respond = _serialPort.ReadLine();
                    return respond;
                }
                catch
                {
                    return null;
                }
            }
        }


        public string ReadMessageWhile(string command, Func<string, bool> predicate, int timeout)
        {
            CancellationTokenSource cts = new CancellationTokenSource(timeout);
            Task<string> task = Task<string>.Run(() =>
            {
                if (predicate == null) throw new ArgumentException("Argument can not be null " + nameof(predicate));
                string respond = SendMessage(command);
                StringBuilder builder = new StringBuilder();
                builder.Append(respond);
                while (!predicate(respond) && !cts.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                    respond = ReadMessage();
                    builder.Append(respond);
                }

                return builder.ToString();
            }, cts.Token);
            if (task.Wait(timeout)) return task.Result;
            cts.Cancel();
            throw new TimeoutException();
        }

        private string ReadMessage()
        {
            return _serialPort.ReadExisting();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}