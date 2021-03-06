﻿using Petecat.Configuring;
using Petecat.Formatter;
using Petecat.DependencyInjection;
using Petecat.Extending;
using System.Text;
using System;
namespace Petecat.ConsoleApp.Configuring
{
    class StaticFileConfigurerTest
    {
        public void Run()
        {
            var configurer = DependencyInjector.GetObject<IStaticFileConfigurer>();
            configurer.Append("apple", "./configuring/apple.json".FullPath(), "json", typeof(AppleClass));

            while (Console.ReadLine() != "quit")
            {
                Console.WriteLine(new JsonFormatter().WriteString(configurer.GetValue("apple"), Encoding.UTF8));
            }

            configurer.Remove("apple");
        }

        public void Run1()
        {
            var configurer = DependencyInjector.GetObject<IStaticFileConfigurer>();

            while (Console.ReadLine() != "quit")
            {
                Console.WriteLine(new JsonFormatter().WriteString(configurer.GetValue<IBananaInterface>(), Encoding.UTF8));
            }

            configurer.Remove("banana");
        }
    }
}
