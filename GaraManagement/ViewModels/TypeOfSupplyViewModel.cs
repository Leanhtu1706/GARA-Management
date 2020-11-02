using GaraManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GaraManagement.ViewModels
{
    public class TypeOfSupplyViewModel
    {
        public TypeOfSupplyViewModel() { }
        public TypeOfSupplyViewModel(TypeOfSupply typeOfSupply) 
        {
            if (typeOfSupply != null)
            {
                Id = typeOfSupply.Id;
                Name = typeOfSupply.Name;
                Description = typeOfSupply.Description;
                supplies = typeOfSupply.Supplies;
            }

        }

        public int Id { get; set; }
        [DisplayName("Loại")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Supply> supplies { get; set; }
    }
}
