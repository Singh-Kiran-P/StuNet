// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Hosting;
// using System;
// using Microsoft.Extensions.DependencyInjection;
// using Server.Api.Repositories;
// using Microsoft.Extensions.Configuration;
// using System.Collections.Generic;
// using Server.Api.Dtos;

// namespace Server.Api.Services
// {
//     public class StartMailListener : BackgroundService
//     {
//         IConfiguration _configuration;
//         private readonly IQuestionRepository _questionRepository;
//         private readonly IServiceScopeFactory _serviceScopeFactory;

//         public StartMailListener(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
//         {
//             _serviceScopeFactory = serviceScopeFactory;

//             _configuration = configuration;
//         }
//         protected override Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             new Timer(printPass, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

//             return Task.CompletedTask;
//         }

//         private void printPass(object state)
//         {
//             using (var scope = _serviceScopeFactory.CreateScope())
//             {
//                 var _questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
//                 Console.WriteLine(_configuration.GetSection("Mail")["G-password"]);
//                 var questions = _questionRepository.getAllAsync().GetAwaiter().GetResult();
//                 List<questionDto> res = new List<questionDto>();
//                 foreach (var q in questions)
//                 {
//                     Console.Write(q.title + '\n');
//                 }
//             }

//         }
//     }
// }
