using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Models
{
    public class BanModel
    {
        public Guid UserId { get; set; }
        public UserModel? User { get; set; }
        public DateTime DateBan { get; set; } = DateTime.Now;
        public string Сause { get; set; } = string.Empty;
        
    }
}
