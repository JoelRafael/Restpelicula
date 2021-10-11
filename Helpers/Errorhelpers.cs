using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Restpelicula.Helpers
{
    public class Errorhelpers
    {

        public static List<ModelErrors> GetModelStateErrors(ModelStateDictionary Model)
        {
            return Model.Select(x => new ModelErrors() { Key = x.Key, Messages = x.Value.Errors.Select(y => y.ErrorMessage).ToList() }).ToList();
        }
    }

    public class ModelErrors
    {

        public string Key { get; set; }
        public List<string> Messages { get; set; }
    }
}