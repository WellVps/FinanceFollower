using System;
using JobsService.Services.Interfaces;

namespace JobsService.Services;

public class TesteService: ITesteService
{
    public async Task ExecTeste()
    {
        Console.WriteLine("Executando teste");
        await Task.CompletedTask;
    }
}
