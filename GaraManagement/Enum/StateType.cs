using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GaraManagement.Models
{
    public enum StateType
    {
        [Display(Name = "Đang chuẩn bị")] preparing,
        [Display(Name = "Đang sửa")] repairing,
        [Display(Name = "Hoàn thành")] completed
    }
    public static class GetName {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
    }
    
}
