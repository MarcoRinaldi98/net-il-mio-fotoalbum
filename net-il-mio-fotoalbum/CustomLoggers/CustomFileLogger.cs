﻿namespace net_il_mio_fotoalbum.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            // File che terrà conto delle LOG
            File.AppendAllText("C:\\Users\\Marco\\source\\repos\\net-il-mio-fotoalbum\\net-il-mio-fotoalbum\\my-log.txt", $"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} LOG: {message}\n");
        }
    }
}
