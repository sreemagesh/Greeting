using Greeting.api.Data;
using Greeting.api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        /// <summary>
        /// This is default GET method to get the greeting by name.
        /// </summary>
        /// <param name="name">name value that is part of the greeting</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = "World")
        {
            string greetingText = string.Empty;
            try
            {
                //Check if the greeting is already in the cache
                if (_localCache.TryGetValue(name, out greetingText))
                {
                    return Ok(greetingText);
                }

                // If not in cache, fetch from database
                Greetings greeting = null;

                try
                {
                    greeting = await _context.Greetings
                    .Where(g => g.Name == name)
                    .OrderByDescending(g => g.CreatedAt)
                    .FirstOrDefaultAsync();
                }
                catch (DbException)
                {
                    // Log the exception (not implemented here)
                    return StatusCode(500, "Internal server error while accessing the database.");
                }

                //If the record not exists save to the database and add to the cache.
                if (greeting != null)
                {
                    greeting = await _context.Greetings
                       .FirstOrDefaultAsync(g => g.Name == name);

                    if (greeting != null)
                    {
                        greetingText = greeting.Greeting;
                        _localCache.Set(name, greetingText, TimeSpan.FromMinutes(5));
                    }
                }
                else
                {
                    Post(name);
                }

                _localCache.TryGetValue(name, out greetingText);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(greetingText);
        }

        /// <summary>
        /// Creates a new greeting for the specified name and stores it in the database and cache.
        /// </summary>
        /// <remarks>This method performs the following actions: 
        /// Creates a greeting data in the database. Stores the greeting in the
        /// database with the current UTC timestamp.
        /// Caches the greeting for 5 minutes using the provided
        /// name as the key
        /// <param name="name">The name of the person to greet.</param>
        /// <returns>An <see cref="IActionResult"/> containing the greeting text.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(string name)
        {
            string greetingText = $"Hello, {name}";

            try
            {
                var greeting = new Greetings
                {
                    Name = name,
                    Greeting = greetingText,
                    CreatedAt = DateTime.UtcNow
                };

                try
                {
                    _context.Greetings.Add(greeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbException)
                {
                    return StatusCode(500, "Internal server error while saving to the database.");
                }
                _localCache.Set(name, greetingText, TimeSpan.FromMinutes(5));
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return Ok(greetingText);
        }
    }
}
