using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GlobalCityManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalCityManager.Controllers{
    public class CountryController:Controller{
        private worldContext _context;
        private IHostingEnvironment _environment;
        public CountryController(IHostingEnvironment environment,worldContext context){
            _context=context;
            _environment=environment;
        }
        public async Task <IActionResult> Index(){
            IList<Country> countries = await _context.Country.ToListAsync();
            return View(countries);
        }

        public async Task <IActionResult> Detail(string code){
            var country= await _context.Country.FindAsync(code);
            if(country!=null)
                return View(country);
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Country country, IFormFile nationalFlagFile)
        {
            //Divide this code, it violates SRP
            if(ModelState.IsValid)
            {
                if (nationalFlagFile!=null&&nationalFlagFile.Length!=0)
                {
                    var targetFileName=$"{country.Code}{Path.GetExtension(nationalFlagFile.FileName)}";
                    var relativePath=Path.Combine("images",targetFileName);
                    var absolutePath=Path.Combine(_environment.WebRootPath,relativePath);
                    country.NationalFlag=relativePath;
                    using (var stream = new FileStream(absolutePath,FileMode.Create))
                    {
                        await nationalFlagFile.CopyToAsync(stream);
                    }
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string code){
            if(!String.IsNullOrWhiteSpace(code))
            {
                var country = await _context.Country.FindAsync(code);
                _context.Country.Remove(country);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(string code){
            var country = await _context.Country.FindAsync(code);
            if(country!=null)
                return View(country);
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Country country,IFormFile nationalFlagFile){
            if (ModelState.IsValid){
                if (nationalFlagFile != null && nationalFlagFile.Length != 0){
                    var targetFileName=$"{country.Code}{Path.GetExtension(nationalFlagFile.FileName)}";
                    var relativePath=Path.Combine("images",targetFileName);
                    var absolutePath=Path.Combine(_environment.WebRootPath,relativePath);
                    using (var stream = new FileStream(absolutePath,FileMode.Create) )
                    {
                        await nationalFlagFile.CopyToAsync(stream);
                    }
                    country.NationalFlag=relativePath;
                }
                _context.Country.Update(country);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}