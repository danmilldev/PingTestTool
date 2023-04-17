using System.Net.NetworkInformation;
using System.Text;

int listLimit = 10;

List<long> pingTimesList = new();

void GetAveragePing()
{
    long avg = 0;

    if (pingTimesList.Count > listLimit)
    {
        pingTimesList.Remove(pingTimesList.First());
    }

    foreach (long t in pingTimesList)
    {
        avg += t;
    }

    avg /= pingTimesList.Count;

    Console.WriteLine("average ping: " + avg + " ms");
}

while (true)
{
    Console.Write("address:");
    string hostName = Console.ReadLine().TrimEnd();

    if (!string.IsNullOrEmpty(hostName))
    {
        Ping pingSender = new();

        PingOptions pingOptions = new();

        pingOptions.DontFragment = true;

        string data = "abcdefghijklmnopqrstuvwxyz";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        int timeOut = 128;

        PingReply reply;

        do
        {
            while (!Console.KeyAvailable)
            {
                reply = pingSender.Send(hostName, timeOut, buffer, pingOptions);

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("Address: {0}", reply.Address.ToString());
                    Console.WriteLine("RoundTrip time: {0}ms", reply.RoundtripTime);
                    Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                    pingTimesList.Add(reply.RoundtripTime);
                    GetAveragePing();
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}
