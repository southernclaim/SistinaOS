using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace SistinaOS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            var available_space = fs.GetAvailableFreeSpace(@"0:\");
            Console.WriteLine("Espaço Livre: " + available_space);
            Console.WriteLine("Sistina OS iniciado! Bem vindo!");
            Console.WriteLine("Digite 'ajuda' para ver os comandos disponíveis.");
        }

        protected override void Run()
        {
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();
                    if (input == null)
                    {
                        Console.WriteLine("Entrada inválida ou não suportada. Reiniciando prompt...");
                        continue;
                    }

                    if (input == "ajuda")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("ajuda    |   Mostra esse menu de ajuda");
                        Console.WriteLine("list     |   Lista os arquivos atuais da sua pasta");
                        Console.WriteLine("criar    |   Cria um arquivo de texto na sua pasta atual");
                        Console.WriteLine("ler      |   Lê um arquivo de texto na sua pasta atual");
                        Console.WriteLine("apagar   |   Apaga um arquivo de texto na sua pasta atual");
                        Console.WriteLine("desligar |   Desliga o sistema");
                    }
                    else if (input == "desligar")
                    {
                        Console.WriteLine("Desligando...");
                        Sys.Power.Shutdown();
                    }
                    else if (input == "list")
                    {
                        var files = fs.GetDirectoryListing(@"0:\");
                        if (files.Count == 0)
                        {
                            Console.WriteLine("Nenhum arquivo encontrado no diretório atual.");
                        }
                        else
                        {
                            Console.WriteLine("Arquivos no diretório atual:");
                            foreach (var file in files)
                            {
                                Console.WriteLine(file.mName);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Comando não reconhecido. Digite 'ajuda' para ver os comandos disponíveis.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }
    }
}
