
using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Domain.Enum
{
    public enum StatusTask
    {
        [Display(Name = "Ожидание")]
        Expectation = 0,

        [Display(Name = "Выполняется")]
        InProgress = 1,

        [Display(Name = "Выполнена")]
        Сompleted = 2
    }
}
