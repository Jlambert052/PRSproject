﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSproject.Migrations;
using PRSproject.Models;

namespace PRSproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public VendorsController(PRSDbContext context)
        {
            _context = context;
        }
        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }
        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }
            return vendor;
        }
        // GET: api/po/{vendorId}
        [HttpGet("po/{vendorId}")]
        public async Task<ActionResult<Po>> CreatePo(int vendorId) {
            Po po = new();
            // instance po, then assign vendorId from the passthrough for the method
            po.Vendor = await _context.Vendors.FindAsync(vendorId);
            
            if(po.Vendor == null) {
                throw new Exception(
                "VendorId did not match."
                );
            }
            var poline = from v in _context.Vendors
                         join p in _context.Products
                            on v.Id equals p.VendorId
                         join rl in _context.RequestLines
                            on p.Id equals rl.ProductId
                         join r in _context.Requests
                            on rl.RequestId equals r.Id
                         where r.Status == "APPROVED"
                         where v.Id == vendorId
                         select new {
                             p.Id,
                             Product = p.Name,
                             rl.Quantity,
                             p.Price,
                             LineTotal = p.Price * rl.Quantity
                         };
            var sortedLines = new SortedList<int, Poline>();
            foreach (var line in poline) {
                if (!sortedLines.ContainsKey(line.Id)) {
                    var pol = new Poline() {
                        Product = line.Product,
                        Quantity = 0,
                        Price = line.Price,
                        LineTotal = line.LineTotal
                    };
                    sortedLines.Add(line.Id, pol);
                }
                sortedLines[line.Id].Quantity += line.Quantity;
            }
            //takes the data from sortedLines, specifies just the value column (ignores the key column) and adds up for each linetotal.
            po.PoTotal = sortedLines.Values.Sum(sl => sl.LineTotal);
            po.Polines = sortedLines.Values;
            return po;
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }
            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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
        // POST: api/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }
        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
