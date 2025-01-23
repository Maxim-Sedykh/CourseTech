using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.ChatGptApi.Models.RequestModels
{
    public class ChatGptRequest
    {
        public string Model { get; set; }
        public Message[] Messages { get; set; }
    }
}
