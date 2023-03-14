﻿using Processador.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Timers;
using Dapper;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace Processador
{
    class Program
    {

        static System.Timers.Timer timer;

        public static List<Module> modules = new List<Module>();


        static void Main(string[] args)
        {
            loadingModule();
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;


            // replace the IP with your system IP Address...
            Servidor myserver = new Servidor(IPAddress.Any, int.Parse(ConfigurationManager.AppSettings.Get("PORT")));

        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loadingModule();
            Console.WriteLine("Modulos carregados");
        }

        private static void loadingModule()
        {
            try
            {
                modules.Clear();

                using (SqlConnection connSystem = DataBase.connectPlugAndPlay(true))
                {
                    var response = connSystem.Query<Module>(@"select 
                                                o.serverName [Server],
                                                o.[Database] ,
                                                o.username [User],
                                                o.[Password] ,
                                                ou.UnitId
                                                from organizations o 
                                                join OrganizationUnits ou on ou.OrgId = o.OrgId", connSystem).ToList();


                    modules = response;


                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine(modules.Count() + " modulos carregados");
                    Console.ResetColor();
                }
                    
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao consultar modulos");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

        }
    }
}
