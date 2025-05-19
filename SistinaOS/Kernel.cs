using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace SistinaOS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;
        private string currentDirectory = @"0:\";

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
                    Console.Write("SistinaOS > ");
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
                        Console.WriteLine("criardir |   Cria uma pasta nova");
                        Console.WriteLine("renomear |   Renomeia um arquivo de texto na sua pasta atual");
                        Console.WriteLine("mover    |   Move um arquivo de texto para outra pasta");
                        Console.WriteLine("copiar   |   Copia um arquivo de texto para outra pasta");
                        Console.WriteLine("ler      |   Lê um arquivo de texto na sua pasta atual");
                        Console.WriteLine("apagar   |   Apaga um arquivo de texto na sua pasta atual");
                        Console.WriteLine("entrar   |   Entra em uma pasta");
                        Console.WriteLine("sair     |   Sai de uma pasta");
                        Console.WriteLine("desligar |   Desliga o sistema");
                        Console.WriteLine("sobre    |   Mostra informações sobre o SistinaOS");
                    }
                    else if (input == "desligar")
                    {
                        Console.WriteLine("Desligando...");
                        Sys.Power.Shutdown();
                    }
                    else if (input == "list")
                    {
                        var files = fs.GetDirectoryListing(currentDirectory);
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
                    else if (input == "criar")
                    {
                        Console.Write("Nome do arquivo (ex: exemplo.txt): ");
                        var nomeArquivo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeArquivo))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }
                        if (System.IO.File.Exists(currentDirectory + nomeArquivo))
                        {
                            Console.WriteLine("Já existe um arquivo com esse nome.");
                            continue;
                        }
                        Console.WriteLine("Digite o conteúdo do arquivo (finalize com uma linha vazia):");
                        var conteudoBuilder = new StringBuilder();
                        string linha;
                        while (true)
                        {
                            linha = Console.ReadLine();
                            if (string.IsNullOrEmpty(linha))
                                break;
                            conteudoBuilder.AppendLine(linha);
                        }

                        var caminhoCompleto = currentDirectory + nomeArquivo;
                        try
                        {
                            System.IO.File.WriteAllText(caminhoCompleto, conteudoBuilder.ToString());
                            Console.WriteLine($"Arquivo '{nomeArquivo}' criado com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao criar o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "sobre") {
                        Console.WriteLine("SistinaOS v1.0.0");
                        Console.WriteLine("O SistinaOS é um sistema operacional pequeno e portatil feito para executar pentests rapidos");
                        Console.WriteLine("Desenvolvido por: Iago Dórea Pinto");
                    }
                    else if (input == "criardir")
                    {
                        Console.Write("Nome da nova pasta: ");
                        var nomePasta = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomePasta))
                        {
                            Console.WriteLine("Nome de pasta inválido.");
                            continue;
                        }

                        var caminhoCompleto = currentDirectory + nomePasta;

                        try
                        {
                            if (System.IO.Directory.Exists(caminhoCompleto))
                            {
                                Console.WriteLine("Já existe uma pasta com esse nome.");
                                continue;
                            }

                            System.IO.Directory.CreateDirectory(caminhoCompleto);
                            Console.WriteLine($"Pasta '{nomePasta}' criada com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao criar a pasta: " + ex.Message);
                        }
                    }
                    else if (input == "ler")
                    {
                        Console.Write("Nome do arquivo para ler (ex: exemplo.txt): ");
                        var nomeArquivo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeArquivo))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }

                        var caminhoCompleto = currentDirectory + nomeArquivo;
                        try
                        {
                            if (!System.IO.File.Exists(caminhoCompleto))
                            {
                                Console.WriteLine("Arquivo não encontrado.");
                                continue;
                            }

                            var conteudo = System.IO.File.ReadAllText(caminhoCompleto);
                            Console.WriteLine("Conteúdo do arquivo:");
                            Console.WriteLine(conteudo);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao ler o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "apagar")
                    {
                        Console.Write("Nome do arquivo para apagar (ex: exemplo.txt): ");
                        var nomeArquivo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeArquivo))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }

                        var caminhoCompleto = currentDirectory + nomeArquivo;
                        try
                        {
                            if (!System.IO.File.Exists(caminhoCompleto))
                            {
                                Console.WriteLine("Arquivo não encontrado.");
                                continue;
                            }

                            System.IO.File.Delete(caminhoCompleto);
                            Console.WriteLine($"Arquivo '{nomeArquivo}' apagado com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao apagar o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "renomear")
                    {
                        Console.Write("Nome atual do arquivo (ex: antigo.txt): ");
                        var nomeAtual = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeAtual))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }

                        Console.Write("Novo nome do arquivo (ex: novo.txt): ");
                        var novoNome = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(novoNome))
                        {
                            Console.WriteLine("Novo nome de arquivo inválido.");
                            continue;
                        }

                        var caminhoAtual = currentDirectory + nomeAtual;
                        var caminhoNovo = currentDirectory + novoNome;

                        try
                        {
                            if (!System.IO.File.Exists(caminhoAtual))
                            {
                                Console.WriteLine("Arquivo não encontrado.");
                                continue;
                            }
                            if (System.IO.File.Exists(caminhoNovo))
                            {
                                Console.WriteLine("Já existe um arquivo com o novo nome.");
                                continue;
                            }

                            System.IO.File.Move(caminhoAtual, caminhoNovo);
                            Console.WriteLine($"Arquivo renomeado de '{nomeAtual}' para '{novoNome}' com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao renomear o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "mover")
                    {
                        Console.Write("Nome do arquivo para mover (ex: exemplo.txt): ");
                        var nomeArquivo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeArquivo))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }

                        Console.Write("Pasta de destino (ex: pasta1): ");
                        var pastaDestino = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(pastaDestino))
                        {
                            Console.WriteLine("Pasta de destino inválida.");
                            continue;
                        }

                        var caminhoOrigem = currentDirectory + nomeArquivo;
                        var caminhoDestino = currentDirectory + pastaDestino + @"\" + nomeArquivo;

                        try
                        {
                            if (!System.IO.File.Exists(caminhoOrigem))
                            {
                                Console.WriteLine("Arquivo não encontrado.");
                                continue;
                            }
                            if (!System.IO.Directory.Exists(currentDirectory + pastaDestino))
                            {
                                Console.WriteLine("A pasta de destino não existe.");
                                continue;
                            }
                            if (System.IO.File.Exists(caminhoDestino))
                            {
                                Console.WriteLine("Já existe um arquivo com esse nome na pasta de destino.");
                                continue;
                            }

                            System.IO.File.Move(caminhoOrigem, caminhoDestino);
                            Console.WriteLine($"Arquivo '{nomeArquivo}' movido para '{pastaDestino}' com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao mover o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "copiar")
                    {
                        Console.Write("Nome do arquivo para copiar (ex: exemplo.txt): ");
                        var nomeArquivo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeArquivo))
                        {
                            Console.WriteLine("Nome de arquivo inválido.");
                            continue;
                        }

                        Console.Write("Pasta de destino (ex: pasta1): ");
                        var pastaDestino = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(pastaDestino))
                        {
                            Console.WriteLine("Pasta de destino inválida.");
                            continue;
                        }

                        var caminhoOrigem = currentDirectory + nomeArquivo;
                        var caminhoDestino = currentDirectory + pastaDestino + currentDirectory + nomeArquivo;

                        try
                        {
                            if (!System.IO.File.Exists(caminhoOrigem))
                            {
                                Console.WriteLine("Arquivo não encontrado.");
                                continue;
                            }
                            if (!System.IO.Directory.Exists(currentDirectory + pastaDestino))
                            {
                                Console.WriteLine("A pasta de destino não existe.");
                                continue;
                            }
                            if (System.IO.File.Exists(caminhoDestino))
                            {
                                Console.WriteLine("Já existe um arquivo com esse nome na pasta de destino.");
                                continue;
                            }

                            System.IO.File.Copy(caminhoOrigem, caminhoDestino);
                            Console.WriteLine($"Arquivo '{nomeArquivo}' copiado para '{pastaDestino}' com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao copiar o arquivo: " + ex.Message);
                        }
                    }
                    else if (input == "entrar")
                    {
                        Console.Write("Nome da pasta para entrar: ");
                        var nomePasta = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nomePasta))
                        {
                            Console.WriteLine("Nome de pasta inválido.");
                            continue;
                        }

                        var novoCaminho = System.IO.Path.Combine(currentDirectory, nomePasta);

                        if (!System.IO.Directory.Exists(novoCaminho))
                        {
                            Console.WriteLine("A pasta não existe.");
                            continue;
                        }

                        if (!novoCaminho.EndsWith(@"\"))
                            novoCaminho += @"\";

                        currentDirectory = novoCaminho;
                        Console.WriteLine($"Diretório atual: {currentDirectory}");
                    }
                    else if (input == "sair")
                    {
                        // Não permite sair da raiz
                        if (currentDirectory == @"0:\" || currentDirectory == @"0:/")
                        {
                            Console.WriteLine("Você já está no diretório raiz.");
                            continue;
                        }

                        try
                        {
                            var dirInfo = new System.IO.DirectoryInfo(currentDirectory.TrimEnd('\\', '/'));
                            var parent = dirInfo.Parent;
                            if (parent == null || parent.FullName.Length < 3) // "0:\" tem 3 caracteres
                            {
                                currentDirectory = @"0:\";
                            }
                            else
                            {
                                currentDirectory = parent.FullName;
                                if (!currentDirectory.EndsWith(@"\")) currentDirectory += @"\";
                            }
                            Console.WriteLine($"Diretório atual: {currentDirectory}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao sair do diretório: " + ex.Message);
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
