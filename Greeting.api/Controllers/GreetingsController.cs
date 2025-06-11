using Greeting.api.Data;
using Greeting.api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greeting.api.Controllers
{
    [Route("api/greeting")]
    public class GreetingsController : Controller
    {
        private readonly GreetingapiContext _context;
        private readonly IMemoryCache _localCache;

        public GreetingsController(GreetingapiContext context, IMemoryCache cache)
        {
            _localCache = cache;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = "World")
        {
            string greetingText = string.Empty;
            if (_localCache.TryGetValue(name, out greetingText))
            {
                return Ok(greetingText);
            }
        
        var greeting = await _context.Greetings
            .Where(g => g.Name == name)
            .OrderByDescending(g => g.CreatedAt)
            .FirstOrDefaultAsync();

            if (greeting != null) //If the record not exists
            {
                greeting = await _context.Greetings
                   .FirstOrDefaultAsync(g => g.Name == name);

                if(greeting != null)
                {
                    greetingText = greeting.Greeting;
                    _localCache.Set(name, greetingText, TimeSpan.FromMinutes(5));
                }
            }
            else
            {
              Post(name);
                //_localCache.Set(name, greetingText, TimeSpan.FromMinutes(5));

                //Assuming the cache is fixed so the POST will read from cache.
            }

            _localCache.TryGetValue(name, out greetingText);
            return Ok(greetingText);

            //if (string.IsNullOrEmpty(name))
            //{
            //    greeting = await _context.Greetings
            //       .FirstOrDefaultAsync(g => g.Name == name);
            //}
            //else (greeting == null)
            //{
            //    Post(name);
            //    greeting = await _context.Greetings
            //        .FirstOrDefaultAsync(g => g.Name == "World");
            //}

            ////string result = greeting?.GreetingText ?? "Hello, World";
            ////_localCache.Set(name, greeting, TimeSpan.FromMinutes(5));

            //return Ok(greeting);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name)
        {
            string greetingText = $"Hello, {name}";
            var greeting = new Greetings
            {
                Name = name,
                Greeting = greetingText,
                CreatedAt = DateTime.UtcNow
            };

            _context.Greetings.Add(greeting);
            await _context.SaveChangesAsync();

            _localCache.Set(name, greetingText, TimeSpan.FromMinutes(5));

            return Ok(greetingText);
        }


        // GET: Greetings

    }
}
