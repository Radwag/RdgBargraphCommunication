using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Services.BargraphDataModel;

namespace Services
{
    public class BargraphEngine : PropertyChangedHelper
    {
        /// <summary>
        /// QueryModel data model 
        /// </summary>
        public QueryModel QueryModel { get; set; } = new QueryModel();
        
        /// <summary>
        /// Serial port sender instance
        /// </summary>
        private RS232Sender _rs232Sender;
        public RS232Sender Sender
        {
            get => _rs232Sender;
            set
            {
                _rs232Sender = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Send settings to device
        /// </summary>
        public async Task<string> SetBargraphSettings() 
        {
            return await Sender.SendMessageAsync(CraftQueryString() + "\r\n");
        }
        
        /// <summary>
        /// Initialize connection with serial port
        /// </summary>
        public Task Connect(string serialPortName)
        {
            return Task.Factory.StartNew(() =>
            {
                using (Sender = new RS232Sender(serialPortName, 115200, Parity.None, Encoding.UTF8, 2000, true))
                {
                    Sender.Connect();
                }
            });
        }
        
        /// <summary>
        /// Dispose connected serial port
        /// </summary>
        public Task Dispose()
        {
            return Task.Factory.StartNew(() =>
            {
                using (Sender)
                {
                    Sender.Dispose();
                    Sender = null;
                }
            });
        }

        /// <summary>
        /// Craft query with calculated crc
        /// </summary>
        public string CraftQueryString()
        {
            CalculateCrc(JsonSerializer.Serialize(QueryModel));
            return JsonSerializer.Serialize(QueryModel);
        }
        
        /// <summary>
        /// Calculate crc
        /// </summary>
        private void CalculateCrc(string query)
        {
            int crc = 0;
            foreach (var t in query)
            {
                crc ^= t;
            }
            
            QueryModel.Crc = crc.ToString("X2");
        }
    }
}