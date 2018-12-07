using System;

namespace DIContainers
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var container = new DIContainer();
            container.Register(typeof(Bread), typeof(SesameSeedBun));
            container.Register(typeof(Cheese), typeof(SwissCheese));
            container.Register(typeof(Sauce), typeof(Ketchup));
            container.Register(typeof(Patty), typeof(ChickenPatty));
            var burger = container.Build(typeof(Burger));
            Console.ReadKey(true);
        }

    }

    public class Burger
    {
        public Burger(Bread bread, Patty patty, Sauce sauce, Cheese cheese)
        {
            Bread = bread;
            Patty = patty;
            Sauce = sauce;
            Cheese = cheese;
        }

        public Bread Bread { get; set; }

        public Patty Patty { get; set; }

        public Sauce Sauce { get; set; }

        public Cheese Cheese { get; set; }
    }

    public class WheatBread : Bread { }

    public class SesameSeedBun : Bread { }

    public class ChickenPatty : Patty { }

    public class MixedVegPatty : Patty { }

    public class Mustard : Sauce { }

    public class Ketchup : Sauce { }

    public class CheeddarCheese : Cheese { }

    public class SwissCheese : Cheese { }

    public abstract class Bread { }
    public abstract class Patty { }
    public abstract class Sauce { }
    public abstract class Cheese { }

}
