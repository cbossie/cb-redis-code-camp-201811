using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RedisApplicationTemplate
{
    public class TimedPageModel : PageModel
    {
        public void Start() => TickStart = DateTime.Now;
        public void End() => TickEnd = DateTime.Now;

        [BindProperty]
        public DateTime TickStart { get; set; }
        [BindProperty]
        public DateTime TickEnd { get; set; }

        [BindProperty] public long MillisecondDuration => (long)(TickEnd - TickStart).TotalMilliseconds;

    }
}
