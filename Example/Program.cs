﻿using System;
using System.Threading;
using System.Net.Mail;
using FluentLogger;
using FluentLogger.Smtp;
using System.Threading.Tasks;
using System.IO;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var smtpClient = new SmtpClient("localhost");
            var userLogDir = Path.Combine("MaxFileSizeRoller", Environment.UserName);
            LogFactory.Init(
                //Add as many loggers as you like
                //Daily is deprecated, use MaximumFileSizeRoller instead
                //new DailyLogRoller(@"DailyLogRoller", LogLevel.Trace),
                new MaximumFileSizeRoller(userLogDir, LogLevel.Trace, false, 2, 3, "log")
                ,new ConsoleLogger(LogLevel.Trace)
                ,new SmtpLogger(smtpClient, "errors@fluentlogger.com", "support@somewhere.com", LogLevel.Critical)
            );
            //LogFactory.Init(new ConsoleLogger(LogLevel.Fatal));
            var logger = LogFactory.GetLogger();

            Task.Run(() =>
            {
                while (true)
                {
                    logger.Trace("Test Serialization", new { Name = "name" });
                    logger.Fatal("Fatal Error");
                   // Thread.Sleep(100);
                }
            }).Wait();
            
            
        }
    }
}
