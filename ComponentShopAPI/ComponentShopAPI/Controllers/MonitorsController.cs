﻿using ComponentShopAPI.Helpers;
using ComponentShopAPI.Models;
using ComponentShopAPI.Services.Image;
using ComponentShopAPI.Services.Monitor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComponentShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorsController : ControllerBase
    {
        private readonly ComponentShopDBContext _context;
        private readonly IMonitorService _monitorService;
        private readonly IImageService _imageService;

        public MonitorsController(ComponentShopDBContext context, IMonitorService monitorService, IImageService imageService)
        {
            _context = context;
            _monitorService = monitorService;
            _imageService = imageService;
        }

        [HttpGet]
        public ActionResult GetMonitors
            ([FromQuery] MonitorQueryParameters queryParameters)
        {
            var monitors = _monitorService.Search(_context.Monitors.ToList(), queryParameters);


            Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
            Response.Headers.Append("X-Total-Count", monitors.Count.ToString());

            if (monitors.Count == 0)
            {
                return Ok();
            }

            monitors = _monitorService.Paginate(monitors, queryParameters);

            return Ok(monitors.Select(monitor => new
            {
                monitor.Id,
                monitor.Name,
                monitor.Price,
                monitor.Availability,
                monitor.Size,
                monitor.Width,
                monitor.Height,
                monitor.RefreshRate,
                monitor.ImageName,
                ImageFile = _imageService.Download(monitor.ImageName, ProductType.Monitor)
            }));
        }


        // GET: api/Monitors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Monitor>> GetMonitor(int id)
        {
            var monitor = await _context.Monitors.FindAsync(id);

            if (monitor == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                monitor.Id,
                monitor.Name,
                monitor.Price,
                monitor.Availability,
                monitor.Size,
                monitor.Width,
                monitor.Height,
                monitor.RefreshRate,
                monitor.ImageName,
                ImageFile = _imageService.Download(monitor.ImageName, ProductType.Monitor)
            });
        }

        // PUT: api/Monitors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMonitor(int id, Models.Monitor monitor)
        {
            if (id != monitor.Id)
            {
                return BadRequest();
            }

            if (monitor.ImageFile != null)
            {
                monitor.ImageName = await _imageService.Upload(monitor.ImageFile, ProductType.Monitor);
            }

            _context.Entry(monitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonitorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Monitors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.Monitor>> PostMonitor(Models.Monitor monitor)
        {
            if (monitor.ImageFile != null)
            {
                monitor.ImageName = await _imageService.Upload(monitor.ImageFile, ProductType.Monitor);
            }

            _context.Monitors.Add(monitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonitor", new { id = monitor.Id }, monitor);
        }

        // DELETE: api/Monitors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMonitor(int id)
        {
            var monitor = await _context.Monitors.FindAsync(id);
            if (monitor == null)
            {
                return NotFound();
            }

            if (monitor.ImageName != null)
            {
                _imageService.Delete(monitor.ImageName, ProductType.Monitor);
            }

            _context.Monitors.Remove(monitor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("supportedProperties")]
        public ActionResult<IEnumerable<string>> GetSupportedProperties()
        {
            return Ok(
                new
                {
                    resolutions = _context.Monitors.Select(monitor => $"{monitor.Width}x{monitor.Height}").Distinct(),
                    refreshRates = _context.Monitors.Select(monitor => monitor.RefreshRate).Distinct()
                }
            );
        }

        private bool MonitorExists(int id)
        {
            return _context.Monitors.Any(e => e.Id == id);
        }
    }
}
