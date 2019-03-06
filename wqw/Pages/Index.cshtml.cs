using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace wqw.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration confg;
        private readonly QueueClient client;

        [FromForm]
        public string Data { get; set; }

        [TempData]
        public string Message { get; set; }

        public IndexModel(IConfiguration confg)
        {
            this.confg = confg;
            client = new QueueClient(confg["QueueConnectionString"], "testqueue");
        }

        public async Task OnPost()
        {
            await client.SendAsync(new Message(Encoding.UTF8.GetBytes(Data)));
            Message = "Message Sent.";
        }
    }
}
