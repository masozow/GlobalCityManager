using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GlobalCityManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace GlobalCityManager.Controllers
{
    public class CityController:Controller{
        worldContext _context;
        public CityController(worldContext context){
            _context=context;
        }
        public async Task<IActionResult> Index(){
            IList<City> cities = await _context.City.ToListAsync();
            return View(cities);
        }
        [HttpGet]
        public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(City city){
            if(ModelState.IsValid){
                _context.Add(city);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id){
            return View(await _context.City.FindAsync(id));
        }
        
        [HttpPost]
         public async Task<IActionResult> Edit(City city){
            if(ModelState.IsValid){
                _context.Update(city);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }    
}