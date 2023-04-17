using System.Net.NetworkInformation;
using System.Text;

int listLimit = 10;

List<long> pingTimesList = new();

//will delete next entry when it exceeds the listLimit and then sum up all elements into the avg variable
//and then subtracts all through the amount of elements stored
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
        //ping instances for the utilites from the librarys
        Ping pingSender = new();
        PingOptions pingOptions = new();
        pingOptions.DontFragment = true;
        
        //some sample data that will be used inside the buffer
        //and will be sind via the buffer inside the send method
        string data = "abcdefghijklmnopqrstuvwxyz";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        int timeOut = 128;

        PingReply reply;

        //while after every runthrough of all statements it will check if escaped was pressed
        //and then going back into asking for an adress to ping to
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
