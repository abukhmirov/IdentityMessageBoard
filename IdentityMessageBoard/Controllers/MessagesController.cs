using IdentityMessageBoard.DataAccess;
using IdentityMessageBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IdentityMessageBoard.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessageBoardContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(UserManager<ApplicationUser> userManager, MessageBoardContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages
                .Include(m => m.Author)
                .OrderBy(m => m.ExpirationDate)
                .ToList()
                .Where(m => m.IsActive()); // LINQ Where(), not EF Where()

            return View(messages);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AllMessages()
        {
            var allMessages = new Dictionary<string, List<Message>>()
            {
                { "active" , new List<Message>() },
                { "expired", new List<Message>() }
            };

            foreach (var message in _context.Messages)
            {
                if (message.IsActive())
                {
                    allMessages["active"].Add(message);
                }
                else
                {
                    allMessages["expired"].Add(message);
                }
            }


            return View(allMessages);
        }

        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> SuperAllMessagesAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var userMessages = _context.Messages.Where(m => m.Author.Id == user.Id.ToString()).ToList();

            var allMessages = new Dictionary<string, List<Message>>()
            {
                { "active" , new List<Message>() },
                { "expired", new List<Message>() }
            };

            foreach (var message in userMessages)
            {
                if (message.IsActive())
                {
                    allMessages["active"].Add(message);
                }
                else
                {
                    allMessages["expired"].Add(message);
                }
            }


            return View(allMessages);
        }




        [Authorize]
        public IActionResult New()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string userId, string content, int expiresIn)
        {
            var user = _context.Users.Find(userId);
            _context.Messages.Add(
                new Message()
                {
                    Content = content,
                    ExpirationDate = DateTime.UtcNow.AddDays(expiresIn),
                    Author = user
                });

            _context.SaveChanges();

            var messageCounter = _context.Messages.Count();
            if (messageCounter == 10)
            {

                await _userManager.AddToRoleAsync(user, "SuperUser");
                
            }

            return RedirectToAction("Index");
        }
    }
}
