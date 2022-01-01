// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Server.Api.DataBase;
// using Server.Api.Models;

// namespace Server.Api.Repositories
// {
//     public interface IUserRepository//: IRestfulRepository<User> // VERVANGEN DOOR GEBRUIK VAN ASP.NET UserManager
//     {
//         Task<User> getByEmailAsync(string email);
//     }
// }

// namespace Server.Api.Repositories
// {
//     public class PgUserRepository //: IUserRepository // VERVANGEN DOOR GEBRUIK VAN ASP.NET UserManager
//     {
//         // private readonly IDataContext _context;
//         // public PgUserRepository(IDataContext context)
//         // {
//         //     _context = context;
//         // }
//         // public async Task<IEnumerable<User>> getAllAsync()
//         // {
//         //     return await _context.Users.ToListAsync();
//         // }

//         // public async Task<User> getByEmailAsync(string email)
//         // {
//         //     return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
//         // }

//         // public async Task createAsync(User user)
//         // {
//         //     _context.Users.Add(user);
//         //     await _context.SaveChangesAsync();
//         // }

//         // public async Task updateAsync(User user)
//         // {
//         //     // var userToUpdate = await _context.Users.FindAsync(user.id);
//         //     // if (userToUpdate == null)
//         //     //     throw new NullReferenceException();
//         //     // userToUpdate.email = user.email;
//         //     // userToUpdate.password = user.password;
//         //     // await _context.SaveChangesAsync();
//         // }

//         // public async Task deleteAsync(int userId)
//         // {
//         //     var userToRemove = await _context.Users.FindAsync(userId);
//         //     if (userToRemove == null)
//         //         throw new NullReferenceException();

//         //     _context.Users.Remove(userToRemove);
//         //     await _context.SaveChangesAsync();
//         // }
//         // public async Task<User> getAsync(int id)
//         // {
//         //     return await _context.Users.FindAsync(id);
//         // }
//     }
// }
