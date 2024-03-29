
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskBoard.Domain.Enum
{
    public enum UserRole
    {
        [Display(Name = "Пользователь")]
        User = 0,

        [Display(Name = "Менеджер")]
        Manager = 1,

        [Display(Name = "Администратор")]
        Administator = 2,

        [Display(Name = "Заброкирован")]
        Ban = 3
    }
}
