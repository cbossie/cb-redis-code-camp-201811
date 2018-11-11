using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CodeCampCacheLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Remotion.Linq.Utilities;
using WeatherSdk;

namespace RedisApplicationTemplate.Pages
{
    public class FibModel : TimedPageModel
    {
        private IValueService mathSvc;

        [BindProperty]
        public bool? Success { get; set; } = null;
        [BindProperty]
        public string Message { get; set; } = null;
        [BindProperty]
        public string Number { get; set; }
        
        [Required]
        [Range(1, 100)]
        [BindProperty]
        public int Sequence { get; set; }

        public FibModel(IValueService svc)
        {
            mathSvc = svc;
        }

        public async void OnGet()
        {

        }

        public void OnPost()
        {
            Success = null;
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                Start();
                Number = mathSvc.GetNthFibonacci(Sequence);
                End();
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
                Message = $"Couldn't calculate the {Sequence}th Fibonacci Number";
            }

        }


    }
}
