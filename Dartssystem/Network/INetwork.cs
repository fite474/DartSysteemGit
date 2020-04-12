using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartssystem.Network
{

	public interface INetwork : IDisposable
	{
		event ReceivedData OnReceivedData;

		string Send(string request);

		void SendDatagram(Datagram data);

		void StartReceivingDatagrams();

		void StopReceivingDatagrams();
	}
}
