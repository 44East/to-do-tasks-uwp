The simplier task manager apllication.

This App use MS SQL Server v16.0 for deployment a local database.
Type of the connection [localhost]. 
It uses a Sytem.Data.SqlClient NuGet package v.4.6.1 (NOT Microsoft.Data.SqlClient!)

For the first connection to the MSSQL Server you should do some steps for the preapre this process.

1. To prepare the network configuration:
   Please turn on a TCP/IP protocols and turn on a SQL Server Browser. Link with the detailed information:  
   https://learn.microsoft.com/en-us/windows/uwp/data-access/sql-server-databases

2. You should prepare the TCP/IP ports:
  Please using SQL Server Configuration Manager 
  to assign a TCP/IP port number to the SQL Server Database Engine. Link with the detailed information:  
  https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/configure-a-server-to-listen-on-a-specific-tcp-port?view=sql-server-ver16
  
3. (Optional) It could be and you have to turn on loopback for a UWP application. 
  Link with the detailed information: 
  https://learn.microsoft.com/en-us/windows/iot-core/develop-your-app/loopback#enabling-loopback-for-a-uwp-application



