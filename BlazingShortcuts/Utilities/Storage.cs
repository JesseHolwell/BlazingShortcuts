using BlazingShortcuts.Models;
using System;
using System.Text.Json;

namespace BlazingShortcuts.Utilities
{
    public static class Storage
    {
        public static string Serialize(BindingModel model)
        {
            string output = JsonSerializer.Serialize(model);
            Console.WriteLine(output);
            return output;
        }

        public static BindingModel Deserialize(string input)
        {
            var output = JsonSerializer.Deserialize<BindingModel>(input);
            Console.WriteLine(input);
            return output;
        }
    }
}
