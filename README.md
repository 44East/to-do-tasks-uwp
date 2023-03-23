For the first connection to the MSSQL Server you should do some steps for the preapre this process.
You should use the Sytem.Data.SqlClient NuGet package (NOT Microsoft.Data.SqlClient!)
Information links ->
To the prepare TCP/IP:
https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/configure-a-server-to-listen-on-a-specific-tcp-port?view=sql-server-2017
To the prepare property app and the SQL:
https://learn.microsoft.com/en-us/windows/uwp/data-access/sql-server-databases
