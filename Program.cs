using FirebirdSql.Data.FirebirdClient;
namespace Console_FbEvent
{
    class Program
    {
        static string StrConnection = $"User=SYSDBA;Password=masterkey;Database=C:\\demo\\RSFBCLIENT.FDB;DataSource=localhost;Dialect=3;Port=3050;Charset=NONE;Pooling=true;";

        static bool Started { get; set; }
        static FbRemoteEvent? userEvent;

        static void Main(string[] args)
        {
            Started = false;

            if (Started == false)
            {
                try
                {
                    userEvent = new FbRemoteEvent(StrConnection);
                    userEvent.RemoteEventCounts += (sender, e) =>
                    {
                        switch (e.Name)
                        {
                            case "SINC_USUARIO":
                                Console.WriteLine("[FbRemoteEvent] - Evento SINC_USUARIO recebido...");
                                break;
                        }
                    };

                    userEvent.Open();
                    userEvent.QueueEvents(new string[] { "SINC_USUARIO" });

                    Console.WriteLine("[FbRemoteEvent] - Escutando evento: SINC_USUARIO ...");

                    Started = true;

                    Console.ReadLine();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine($"[FbRemoteEvent] - Ocorreu um erro ao tentar iniciar o serviço: {e.Message}");
                    Started = false;
                }
            }
        }
    }
}
