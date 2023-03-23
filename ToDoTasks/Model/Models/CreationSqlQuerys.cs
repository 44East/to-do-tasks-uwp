using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks.Model.Models
{
    internal static class CreationLocalDBSqlQuerys
    {
        public static string DBCreationQuery { get; } = $@"CREATE DATABASE ToDoList ON PRIMARY
                                                           (NAME = ToDoList_Data,
                                                           FILENAME = 'D:\Application Data\ToDoTasks\db_data\ToDoList.mdf',
                                                           SIZE = 2MB, MAXSIZE = 20MB, FILEGROWTH = 10%)
                                                           LOG ON (NAME = ToDoList_Log,
                                                           FILENAME = 'D:\Application Data\ToDoTasks\db_data\ToDoListLog.ldf',
                                                           SIZE = 1MB,
                                                           MAXSIZE = 5MB,
                                                           FILEGROWTH = 10%)";
        public static string CreationPersonsTableQuery { get; } = $@"USE [ToDoList]
                                                             SET ANSI_NULLS ON
                                                             SET QUOTED_IDENTIFIER ON
                                                             CREATE TABLE [dbo].[Persons](
	                                                             [ID] [int] IDENTITY(1,1) NOT NULL,
	                                                             [First_Name] [nvarchar](30) NOT NULL,
	                                                             [Last_Name] [nvarchar](30) NOT NULL,
                                                             CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED
                                                             (
                                                                 [ID] ASC
                                                             )
                                                             WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                                             ) ON [PRIMARY]";
        public static string CreationTasksTableQuery { get; } = $@"USE [ToDoList]
                                                              SET ANSI_NULLS ON
                                                              SET QUOTED_IDENTIFIER ON
                                                              CREATE TABLE [dbo].[Tasks](
	                                                              [ID] [int] IDENTITY(1,1) NOT NULL,
	                                                              [Description] [nvarchar](200) NOT NULL,
	                                                              [Assigned_Person] [int] NOT NULL,
	                                                              [Name] [nvarchar](30) NOT NULL,
                                                              CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
                                                              (
	                                                              [ID] ASC
                                                              )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                                              ) ON [PRIMARY]
                                                              ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Assigned_Persons] FOREIGN KEY([Assigned_Person])
                                                              REFERENCES [dbo].[Persons] ([ID])
                                                              ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Assigned_Persons]";
        public static string AddTestPersonsInDBQuery { get; } = $@"USE [ToDoLIst]
                                                                   SET IDENTITY_INSERT Persons ON
                                                                   INSERT Persons(ID, First_Name, Last_Name) VALUES
                                                                   (1,N'John',N'McLean'),
                                                                   (2,N'Woody',N'Woodpecker'),
                                                                   (3,N'Clark',N'Kent'),
                                                                   (4,N'Peter',N'Parker')
                                                                   SET IDENTITY_INSERT Persons OFF";
        public static string AddTestTasksInDBQuery { get; } = $@"USE [ToDoLIst]
                                                                 SET IDENTITY_INSERT Tasks ON
                                                                 INSERT Tasks(ID, Description,Assigned_Person, Name) VALUES
                                                                 (1, N'Tap into the tree', 2, N'Woody deals'),
                                                                 (2, N'Delivery pizza', 4, N'Spider-Man deals'),
                                                                 (3, N'Write an article and send it to the editor.', 3, N'Super-Man deals'),
                                                                 (4, N'Walking on a broken glass', 1,N'Die-Hard deals')
                                                                 SET IDENTITY_INSERT Tasks OFF";

    }
}
