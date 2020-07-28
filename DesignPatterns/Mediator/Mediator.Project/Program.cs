using Mediator.Structural; 

namespace Mediator
{
    internal class Program
    {
        private static void Main()
        {
            //var mediator = new ConcreteMediator();
            //var c1 = new Colleague1(mediator);
            //var c2 = new Colleague1(mediator);

            //mediator.Colleague1 = c1;
            //mediator.Colleague2 = c2;

            //c1.Send("Hello, World from c1");
            //c2.Send("Hello, World from c2");

            var mediator = new ConcreteMediator();
            var c1 = new Colleague1();
            var c2 = new Colleague1();

            mediator.Register(c1);
            mediator.Register(c2);

            c1.Send("Hello from c1");
            c2.Send("Hello from c2");

        }
    }
}
