using System;
using System.Collections.Generic;

namespace YourNamespace
{
    public class Menus
    {
        private readonly List<string> _menuItems;

        public Menus()
        {
            _menuItems = new List<string>();
        }

        public void AddMenuItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                _menuItems.Add(item);
            }
        }

        public void RemoveMenuItem(string item)
        {
            _menuItems.Remove(item);
        }

        public IEnumerable<string> GetMenuItems()
        {
            return _menuItems;
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Menu:");
            foreach (var item in _menuItems)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }
}
